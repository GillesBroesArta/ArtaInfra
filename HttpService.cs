using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Arta.Infrastructure
{
    /// <summary>
    /// Generic http service used to perform http calls (GET, POST, PUT, PATCH, DELETE ...)
    /// and to have the httpclient only instantiated once. 
    /// (Creating a new HttpClient instance per request can exhaust the available sockets.)
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Returns a resource from the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the returned object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<Result<T>> Get<T>(Uri uri) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is posted.</param>
        /// <returns></returns>
        Task<Result<T>> PostAsJson<T>(Uri uri, T resource) where T : class;

        /// <summary>
        /// Puts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the object to put.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is put.</param>
        /// <returns></returns>
        Task<Result> PutAsJson<T>(Uri uri, T resource) where T : class;
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Result<T>> Get<T>(Uri uri) where T : class
        {
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return Result<T>.Fail($"Error occurred while performing get from {uri}: {response}");
            return Result<T>.Ok(JsonConvert.DeserializeObject<T>(content));
        }

        public async Task<Result<T>> PostAsJson<T>(Uri uri, T resource) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return Result<T>.Fail($"Error occurred while performing post to {uri}: {response}");
            return Result<T>.Ok(JsonConvert.DeserializeObject<T>(result));
        }

        public async Task<Result> PutAsJson<T>(Uri uri, T resource) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(uri, content);
            if (!response.IsSuccessStatusCode)
                return Result.Fail($"Error occurred while performing put to {uri}: {response}");
            return Result.Ok();
        }
    }
}

