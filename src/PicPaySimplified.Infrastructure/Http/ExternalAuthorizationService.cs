using System.Text.Json;
using Microsoft.Extensions.Logging;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Infrastructure.Http;

public class ExternalAuthorizationService : IExternalAuthorizationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalAuthorizationService> _logger;

    public ExternalAuthorizationService(HttpClient httpClient, ILogger<ExternalAuthorizationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> Authorize()
    {
        // Get Request for external authorization
        var url = "https://util.devi.tools/api/v2/authorize";
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<HttpResultModel<HttpAuthorization>>(responseBody);
            if (authResponse?.Data == null) return false;
            return authResponse.Data.Authorization;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }
}