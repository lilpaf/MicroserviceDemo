using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpCommandDataClient> _logger;

        public HttpCommandDataClient(
            HttpClient httpClient, 
            IConfiguration configuration,
            ILogger<HttpCommandDataClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform), 
                Encoding.UTF8,
                "application/json");

            var url = _configuration["CommandServicePlatformsEndpoint"];

            var response = await _httpClient.PostAsync(url, httpContent);

            if (!response.IsSuccessStatusCode) 
            {
                _logger.LogError("--> Sync POST to CommandService was NOT OK!");
                return;
            }

            _logger.LogInformation("--> Sync POST to CommandService was OK!");
        }
    }
}
