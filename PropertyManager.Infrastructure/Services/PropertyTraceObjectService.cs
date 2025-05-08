using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Services;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Common;

namespace PropertyManager.Infrastructure.Services
{
    public class PropertyTraceObjectService : IPropertyTraceObjectService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PropertyTraceObjectService> _logger;

        public PropertyTraceObjectService(IHttpClientFactory httpClientFactory, ILogger<PropertyTraceObjectService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<(bool result, List<PropertyTraceModelOut> propertyTraces, string errorMessage)> GetByPropertyTraceByIdAsync(int idProperty)
        {
            try
            {
                var jsonData = new Dictionary<string, object>
                {
                    { "idProperty", idProperty }
                };
                var json = JsonSerializer.Serialize(jsonData);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient("PropertyTrace");
                var response = await client.PostAsync($"api/PropertyTrace/find_by", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<TResponse>(content, options);

                    var list = JsonSerializer.Deserialize<List<PropertyTraceModelOut>>(result?.Data!.ToString());
                    return (true, list, string.Empty);

                }
                return (false, null, response.ReasonPhrase);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return (false, null, ex.Message);
            }
        }
    }
}
