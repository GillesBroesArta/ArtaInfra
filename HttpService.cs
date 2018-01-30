using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Arta.Infrastructure;
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
        Task<HttpServiceResult<T>> Get<T>(Uri uri) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is posted.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> PostAsJson<T>(Uri uri, T resource) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> Post<T>(Uri uri) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> PostWithErrorResult<T>(Uri uri) where T : class;



        /// <summary>
        /// Puts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the object to put.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is put.</param>
        /// <returns></returns>
        Task<HttpServiceResult> PutAsJson<T>(Uri uri, T resource) where T : class;
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpServiceResult<T>> Get<T>(Uri uri) where T : class
        {
            var response = await _client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> PostAsJson<T>(Uri uri, T resource) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> Post<T>(Uri uri) where T : class
        {
            var response = await _client.PostAsync(uri, null);
            var resultSerialized = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
            return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(resultSerialized), (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> PostWithErrorResult<T>(Uri uri) where T : class
        {
            var response = await _client.PostAsync(uri, null);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult> PutAsJson<T>(Uri uri, T resource) where T : class
        {
            var content = JsonConvert.SerializeObject(resource);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(uri, stringContent);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return HttpServiceResult.Fail($"Error occurred while performing put to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
            return HttpServiceResult.Ok((int)response.StatusCode);
        }
    }
}







