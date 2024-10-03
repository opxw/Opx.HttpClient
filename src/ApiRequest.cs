using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Opx.Http
{
    public class ApiRequest : HttpRequestBase, IApiRequest
    {
        public ApiRequest(string baseAddress, HttpRequestVersion httpVersion = HttpRequestVersion.Http20,
            HttpVersionPolicy httpVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher)
            : base(httpVersion, httpVersionPolicy)
        {
            BaseAddress = baseAddress;
        }

        public async Task<T?> RequestJsonAsync<T>(HttpMethod method, string path, ApiRequestParameter? parameter = null)
        {
            var result = default(T);
            var request = GetRequest(method, path, parameter);

            request.Headers.Add("Accept", "*/*");
            request.Headers.Accept.Add((new MediaTypeWithQualityHeaderValue("application/json")));
            request.Content = new StringContent(string.Empty, Encoding.UTF8, new MediaTypeWithQualityHeaderValue("application/json"));

            if (parameter != null)
            {
                if (parameter.FromBody != null)
                {
                    var json = JsonSerializer.Serialize(parameter.FromBody);

                    request.Content = new StringContent(JsonSerializer.Serialize(parameter.FromBody), Encoding.UTF8,
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }

                if (!string.IsNullOrWhiteSpace(parameter.BearerToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", parameter.BearerToken);
            }

            try
            {
                Response = await HttpClient.SendAsync(request);

                if (Response.IsSuccessStatusCode)
                {
                    var responseStream = await Response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<T>(responseStream, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    responseStream.Dispose();
                }
                else
                    SetErrorFromResponse();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
            finally
            {
                request.Dispose();
            }

            return result;
        }

        public T? RequestJson<T>(HttpMethod method, string path, ApiRequestParameter? parameter = null)
        {
            return HttpHelper.RunSync<T>(() => RequestJsonAsync<T>(method, path, parameter));
        }
    }
}