using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChampionsChromo.Core.Clients.Interfaces;
using ChampionsChromo.Core.Models;
using ChampionsChromo.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChampionsChromo.Infrastructure.Clients;

public class AbacatePayClient : IAbacatePayClient
{
    private readonly HttpClient _httpClient;
    private readonly AbacatePayOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;

    public AbacatePayClient(HttpClient httpClient, IOptions<AbacatePayOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        // Configure o cliente HTTP
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiToken);

        // Configurar opções de serialização JSON
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<GeneratePixAbacatePayResponse?> GeneratePix(string endpoint, GeneratePixAbacatePayRequest generatePixAbacatePayRequest, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, generatePixAbacatePayRequest, _jsonOptions, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<GeneratePixAbacatePayResponse>(body);
    }
}
