using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using UserManager.Application.DTOs;
using UserManager.Application.Interfaces;
using UserManager.Domain.Entities;
using UserManager.Infrastructure.ExternalServices.Api.Models;

namespace UserManager.Infrastructure.ExternalServices.Api
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<JsonPlaceholderService> _logger;

        public JsonPlaceholderService(HttpClient httpClient, ILogger<JsonPlaceholderService> logger)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            _logger = logger;
        }

        public async Task<UserDetailsDTO?> GetUserDetailsAsync(string email)
        {
            var path = $"users?email={Uri.EscapeDataString(email)}";
            _logger.LogInformation("Getting user details from external API {uri}{path}", _httpClient.BaseAddress?.ToString(), path);
            var users = await _httpClient.GetFromJsonAsync<UserApiResponse[]>(path, _jsonSerializerOptions);
            var userData = users?.FirstOrDefault();
            _logger.LogInformation("Response from API: {@Users}", userData);
            if (userData == null)
            {
                return null;
            }
            var address = userData.Address == null ? null : new Address
            {
                City = userData.Address.City,
                Suite = userData.Address.Suite,
                Street = userData.Address.Street,
                ZipCode = userData.Address.ZipCode,
                Lat = userData.Address.Geo?.Lat,
                Lng = userData.Address.Geo?.Lng,
            };
            var company = userData.Company == null ? null : new Company
            {
                Name = userData.Company.Name,
                CatchPhrase = userData.Company.CatchPhrase,
                Bs = userData.Company.Bs
            };
            return new UserDetailsDTO(userData.Username, userData.Phone, userData.Website, address, company);
        }
    }
}
