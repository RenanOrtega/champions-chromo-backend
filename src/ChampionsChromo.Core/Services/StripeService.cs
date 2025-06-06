using System.Text.Json;
using ChampionsChromo.Core.Entities;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Core.Repositories.Interfaces;
using ChampionsChromo.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace ChampionsChromo.Core.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StripeService> _logger;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly IPaymentRepository _paymentRepository;
        public StripeService(
            IConfiguration configuration, 
            ILogger<StripeService> logger, 
            IPaymentRepository paymentRepository)
        {
            _configuration = configuration;
            _logger = logger;

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            _paymentIntentService = new PaymentIntentService();
            _paymentRepository = paymentRepository;
        }

        public async Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request)
        {
            try
            {
                var metadata = new Dictionary<string, string>();

                if (request.Items?.Count > 0)
                {
                    var itemsJson = JsonSerializer.Serialize(request.Items.Select(item => new
                    {
                        AlbumId = item.Album.Id,
                        AlbumName = item.Album.Name,
                        Stickers = item.Stickers.Select(sticker => new
                        {
                            sticker.Id,
                            sticker.Number,
                            sticker.Name,
                            sticker.Type,
                            sticker.Price
                        })
                    }));

                    metadata["items"] = itemsJson;
                }

                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = request.Currency,
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = metadata,
                };

                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                return new CreatePaymentIntentResponse
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Erro ao criar Payment Intent: {Message}", ex.Message);
                throw new ApplicationException($"Erro no pagamento: {ex.Message}");
            }
        }

        public async Task<PaymentDetailsResponse?> GetPaymentDetailsAsync(string paymentIntentId)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId);

                return new PaymentDetailsResponse
                {
                    Id = paymentIntent.Id,
                    Amount = paymentIntent.Amount,
                    Currency = paymentIntent.Currency,
                    Status = paymentIntent.Status,
                    Created = paymentIntent.Created,
                    Metadata = paymentIntent.Metadata
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Erro ao buscar Payment Intent {PaymentIntentId}: {Message}",
                    paymentIntentId, ex.Message);
                return null;
            }
        }

        public async Task<bool> ValidateWebhookAsync(string payload, string signature)
        {
            try
            {
                var webhookSecret = _configuration["Stripe:WebhookSecret"];
                var stripeEvent = EventUtility.ConstructEvent(payload, signature, webhookSecret);
                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Erro na validação do webhook: {Message}", ex.Message);
                return false;
            }
        }

        public async Task ProcessWebhookEventAsync(string eventType, object eventData)
        {
            try
            {
                switch (eventType)
                {
                    case "payment_intent.succeeded":
                        await HandlePaymentSucceededAsync(eventData);
                        break;

                    case "payment_intent.payment_failed":
                        await HandlePaymentFailedAsync(eventData);
                        break;

                    default:
                        _logger.LogInformation("Evento não tratado: {EventType}", eventType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar evento do webhook: {EventType}", eventType);
            }
        }

        private async Task HandlePaymentSucceededAsync(object eventData)
        {
            var paymentIntent = JsonSerializer.Deserialize<PaymentIntent>(eventData.ToString() ?? "");

            if (paymentIntent != null)
            {
                _logger.LogInformation("Pagamento bem-sucedido: {PaymentIntentId}", paymentIntent.Id);
                var payment = new Payment
                {
                    Amount = paymentIntent.Amount,
                    CanceledAt = paymentIntent.CanceledAt,
                    CancellationReason = paymentIntent.CancellationReason,
                    ClientSecret = paymentIntent.ClientSecret,
                    Currency = paymentIntent.Currency,
                    PaymentGatewayId = paymentIntent.Id,
                    Metadata = paymentIntent.Metadata,
                    Status = paymentIntent.Status,
                };

                await _paymentRepository.AddAsync(payment);

                if (paymentIntent.Metadata?.ContainsKey("items") == true)
                {
                    _logger.LogInformation("Itens do pedido: {Items}", paymentIntent.Metadata["items"]);
                }
            }
        }

        private async Task HandlePaymentFailedAsync(object eventData)
        {
            var paymentIntent = JsonSerializer.Deserialize<PaymentIntent>(eventData.ToString() ?? "");

            if (paymentIntent != null)
            {
                _logger.LogWarning("Pagamento falhou: {PaymentIntentId}", paymentIntent.Id);

                var payment = new Payment
                {
                    Amount = paymentIntent.Amount,
                    CanceledAt = paymentIntent.CanceledAt,
                    CancellationReason = paymentIntent.CancellationReason,
                    ClientSecret = paymentIntent.ClientSecret,
                    Currency = paymentIntent.Currency,
                    PaymentGatewayId = paymentIntent.Id,
                    Metadata = paymentIntent.Metadata,
                    Status = paymentIntent.Status,
                };

                await _paymentRepository.AddAsync(payment);
            }
        }
    }
}
