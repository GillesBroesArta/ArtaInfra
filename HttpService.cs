using System;
using System.Collections.Generic;
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
        Task<HttpServiceResult<T>> Get<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="keyValues">List of keyvalues to be posted as formdata.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> PostAsFormData<T>(Uri uri, List<KeyValuePair<string, string>> keyValues, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is posted.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> PostAsJson<T>(Uri uri, T resource, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> Post<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Posts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the posted object.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<T>> PostWithErrorResult<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Puts to the given uri without a resource.
        /// </summary>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult> Put(Uri uri, Action<HttpRequestMessage> action = null);

        /// <summary>
        /// Puts to the given uri without a resource.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<TResponse>> Put<TResponse>(Uri uri, Action<HttpRequestMessage> action = null) where TResponse : class;

        /// <summary>
        /// Puts a resource to the given uri.
        /// </summary>
        /// <typeparam name="TRequest">Type of the object to put.</typeparam>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<TResponse>> PutWithErrorResult<TRequest, TResponse>(Uri uri, TRequest resource, Action<HttpRequestMessage> action = null) where TRequest : class
                                                                                                               where TResponse : class;

        /// <summary>
        /// Puts a resource to the given uri.
        /// </summary>
        /// <typeparam name="T">Type of the object to put.</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <param name="resource">Resource that is put.</param>
        /// <returns></returns>
        Task<HttpServiceResult> PutAsJson<T>(Uri uri, T resource, Action<HttpRequestMessage> action = null) where T : class;

        /// <summary>
        /// Patches a resource at the given uri.
        /// </summary>
        /// <typeparam name="TRequest">Type of the patch object.</typeparam>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        /// <param name="uri">Uri where to the request is done.</param>
        /// <returns></returns>
        Task<HttpServiceResult<TResponse>> PatchWithErrorResult<TRequest, TResponse>(Uri uri, TRequest resource, Action<HttpRequestMessage> action = null) where TRequest : class
                                                                                                                                                           where TResponse : class;
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpServiceResult<T>> Get<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> PostAsFormData<T>(Uri uri, List<KeyValuePair<string, string>> keyValues, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new FormUrlEncodedContent(keyValues);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> PostAsJson<T>(Uri uri, T resource, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            if(action != null) action(request);
            request.Content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> Post<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
            return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(resultSerialized), (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<T>> PostWithErrorResult<T>(Uri uri, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return HttpServiceResult<T>.Ok(JsonConvert.DeserializeObject<T>(result), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(result);

            return failedJson != null
                ? HttpServiceResult<T>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<T>.Fail($"Error occurred while performing post to {uri}: {response} - {result}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<TResponse>> PutWithErrorResult<TRequest, TResponse>(Uri uri, TRequest resource, Action<HttpRequestMessage> action = null) where TRequest : class
                                                                                                                                                                      where TResponse : class
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            if(action != null) action(request);
            var content = JsonConvert.SerializeObject(resource);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<TResponse>.Ok(JsonConvert.DeserializeObject<TResponse>(resultSerialized), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(resultSerialized);

            return failedJson != null
                ? HttpServiceResult<TResponse>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode) 
                : HttpServiceResult<TResponse>.Fail($"Error occurred while performing put to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult> PutAsJson<T>(Uri uri, T resource, Action<HttpRequestMessage> action = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            if(action != null) action(request);
            var content = JsonConvert.SerializeObject(resource);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return HttpServiceResult.Fail($"Error occurred while performing put to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
            return HttpServiceResult.Ok((int)response.StatusCode);
        }

        public async Task<HttpServiceResult> Put(Uri uri, Action<HttpRequestMessage> action = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return HttpServiceResult.Fail($"Error occurred while performing put to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
            return HttpServiceResult.Ok((int)response.StatusCode);
        }

        public async Task<HttpServiceResult<TResponse>> Put<TResponse>(Uri uri, Action<HttpRequestMessage> action = null) where TResponse : class
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            if(action != null) action(request);
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<TResponse>.Ok(JsonConvert.DeserializeObject<TResponse>(resultSerialized), (int)response.StatusCode);
            return HttpServiceResult<TResponse>.Fail($"Error occurred while performing put to {uri} : {response}", null, (int)response.StatusCode);
        }

        public async Task<HttpServiceResult<TResponse>> PatchWithErrorResult<TRequest, TResponse>(Uri uri, TRequest resource, Action<HttpRequestMessage> action = null) where TRequest : class
                                                                                                                                                                        where TResponse : class
        {
            var content = JsonConvert.SerializeObject(resource);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            if(action != null) action(request);
            request.Content = stringContent;
            var response = await _client.SendAsync(request);
            var resultSerialized = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return HttpServiceResult<TResponse>.Ok(JsonConvert.DeserializeObject<TResponse>(resultSerialized), (int)response.StatusCode);

            var failedJson = JsonConvert.DeserializeObject<ErrorWithErrorCode>(resultSerialized);

            return failedJson != null
                ? HttpServiceResult<TResponse>.Fail(failedJson.ErrorMessage, failedJson.ErrorCode, (int)response.StatusCode)
                : HttpServiceResult<TResponse>.Fail($"Error occurred while performing patch to {uri}: {response} - {resultSerialized}", null, (int)response.StatusCode);
        }


    }
}
