using System;
using System.Threading.Tasks;
using Arta.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Arta.Infrastructure.Configuration;

namespace Arta.Infrastructure
{
    public class GetJwtResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
    public class JwtService
    {
        private readonly IHttpService _httpService;
        private readonly string _baseUri;
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IMemoryCache _cache;

        public JwtService(IHttpService httpService, ClientConfiguration clientConfiguration, AuthenticationConfiguration authenticationConfiguration, IMemoryCache cache)
        {
            _httpService = httpService;
            _baseUri = $"{clientConfiguration.AuthenticationApi.RemoveTrailingSlash()}";
            _authenticationConfiguration = authenticationConfiguration;
            _cache = cache;
        }

        public async Task<HttpServiceResult<GetJwtResponse>> GetJwt() 
        {           
            return await _cache.GetOrCreate<Task<HttpServiceResult<GetJwtResponse>>>("MicroserviceJwt",
            cacheEntry =>
            {
                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", _authenticationConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", _authenticationConfiguration.ClientSecret),
                    new KeyValuePair<string, string>("grant_type", _authenticationConfiguration.GrantType),
                    new KeyValuePair<string, string>("scope", _authenticationConfiguration.Scope)
                };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(55));
                return _httpService.PostAsFormData<GetJwtResponse>(new Uri($"{_baseUri}/connect/token"), keyValues);
            });
        }
    }
}