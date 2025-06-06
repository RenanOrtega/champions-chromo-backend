using ChampionsChromo.Core.Models;

namespace ChampionsChromo.Core.Services.Interfaces
{
    public interface IStripeService
    {
        Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request);
        Task<PaymentDetailsResponse?> GetPaymentDetailsAsync(string paymentIntentId);
        Task<bool> ValidateWebhookAsync(string payload, string signature);
        Task ProcessWebhookEventAsync(string eventType, object eventData);
    }   
}