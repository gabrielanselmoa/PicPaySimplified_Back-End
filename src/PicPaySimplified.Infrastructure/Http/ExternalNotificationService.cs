using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PicPaySimplified.Application.Interfaces;

namespace PicPaySimplified.Infrastructure.Http;

public class ExternalNotificationService : IExternalNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalAuthorizationService> _logger;

    public ExternalNotificationService(HttpClient httpClient, ILogger<ExternalAuthorizationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> Notify()
    {
        // Post Request for external notification
        var url = "https://util.devi.tools/api/v1/notify";
        try
        {
            var jsonPayload = JsonSerializer.Serialize(new {});
            var content = new StringContent(jsonPayload, Encoding.UTF8, @"application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var code = response.StatusCode;
            if (code == HttpStatusCode.NoContent)
                return true;
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }
}