using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using ChampionsChromo.Core.Options;
using ChampionsChromo.Core.Services;
using ChampionsChromo.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChampionsChromo.Infrastructure.Payments;

// Talks to the Mercado Pago Orders API (Checkout Transparente) over plain HTTP.
// Docs: https://www.mercadopago.com.br/developers/pt/docs/checkout-api-orders/payment-integration/websites/pix
public class MercadoPagoGateway(
    HttpClient httpClient,
    IOptions<MercadoPagoOptions> options,
    ILogger<MercadoPagoGateway> logger) : IPaymentGateway
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _httpClient = httpClient;
    private readonly MercadoPagoOptions _options = options.Value;
    private readonly ILogger<MercadoPagoGateway> _logger = logger;

    public async Task<GatewayOrder> CreatePixOrderAsync(CreatePixOrderRequest request, CancellationToken cancellationToken = default)
    {
        var amount = request.Amount.ToString("0.00", CultureInfo.InvariantCulture);

        var body = new
        {
            type = "online",
            total_amount = amount,
            external_reference = request.ExternalReference,
            processing_mode = "automatic",
            transactions = new
            {
                payments = new[]
                {
                    new
                    {
                        amount,
                        payment_method = new { id = "pix", type = "bank_transfer" },
                        expiration_time = XmlConvert.ToString(request.ExpiresIn)
                    }
                }
            },
            payer = new
            {
                email = request.PayerEmail,
                first_name = request.PayerFirstName
            }
        };

        // Mercado Pago answers 423 (resource_locked) while another request with the same idempotency key
        // is still being processed. Retrying returns the same order instead of an error.
        const int maxAttempts = 4;
        HttpResponseMessage response;
        string content;
        var attempt = 0;
        while (true)
        {
            attempt++;
            using var message = NewRequest(HttpMethod.Post, "/v1/orders");
            message.Headers.Add("X-Idempotency-Key", request.IdempotencyKey);
            message.Content = JsonContent.Create(body);

            response = await _httpClient.SendAsync(message, cancellationToken);
            content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.StatusCode != HttpStatusCode.Locked || attempt >= maxAttempts)
                break;

            response.Dispose();
            await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken);
        }

        using var _ = response;

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Mercado Pago rejected order {Reference}: {Status} {Body}",
                request.ExternalReference, (int)response.StatusCode, content);
            throw new PaymentGatewayException($"Mercado Pago retornou {(int)response.StatusCode}.");
        }

        return Map(Deserialize(content));
    }

    public async Task<GatewayOrder?> GetOrderAsync(string providerOrderId, CancellationToken cancellationToken = default)
    {
        using var message = NewRequest(HttpMethod.Get, $"/v1/orders/{Uri.EscapeDataString(providerOrderId)}");
        using var response = await _httpClient.SendAsync(message, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Mercado Pago failed to return order {OrderId}: {Status} {Body}",
                providerOrderId, (int)response.StatusCode, content);
            throw new PaymentGatewayException($"Mercado Pago retornou {(int)response.StatusCode}.");
        }

        return Map(Deserialize(content));
    }

    private HttpRequestMessage NewRequest(HttpMethod method, string path)
    {
        if (string.IsNullOrWhiteSpace(_options.AccessToken))
            throw new PaymentGatewayException("Mercado Pago não configurado (MercadoPago__AccessToken).");

        var message = new HttpRequestMessage(method, new Uri(new Uri(_options.ApiBaseUrl), path));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return message;
    }

    private static MpOrder Deserialize(string content) =>
        JsonSerializer.Deserialize<MpOrder>(content, JsonOptions)
        ?? throw new PaymentGatewayException("Resposta vazia do Mercado Pago.");

    private static GatewayOrder Map(MpOrder order)
    {
        var payment = order.Transactions?.Payments?.FirstOrDefault();

        return new GatewayOrder(
            order.Id ?? string.Empty,
            order.Status ?? string.Empty,
            order.StatusDetail,
            order.ExternalReference,
            decimal.TryParse(order.TotalAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out var total) ? total : 0m,
            payment?.Id,
            payment?.PaymentMethod?.QrCode,
            payment?.PaymentMethod?.QrCodeBase64,
            payment?.PaymentMethod?.TicketUrl);
    }

    private sealed class MpOrder
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
        public string? StatusDetail { get; set; }
        public string? ExternalReference { get; set; }
        public string? TotalAmount { get; set; }
        public MpTransactions? Transactions { get; set; }
    }

    private sealed class MpTransactions
    {
        public List<MpPayment>? Payments { get; set; }
    }

    private sealed class MpPayment
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
        public string? StatusDetail { get; set; }
        public MpPaymentMethod? PaymentMethod { get; set; }
    }

    private sealed class MpPaymentMethod
    {
        public string? Id { get; set; }
        public string? QrCode { get; set; }
        public string? QrCodeBase64 { get; set; }
        public string? TicketUrl { get; set; }
    }
}
