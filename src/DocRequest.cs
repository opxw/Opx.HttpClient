namespace Opx.Http
{
    public class DocRequest : HttpRequestBase
    {
        public DocRequest(string baseAddress, HttpRequestVersion httpVersion = HttpRequestVersion.Http20, 
            HttpVersionPolicy httpVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher) 
            : base(httpVersion, httpVersionPolicy)
        {
            BaseAddress = baseAddress;
        }

        public async Task<string?> RequestStringAsync(HttpMethod method, string path = "", HttpRequestParameter? parameter = null)
        {
            var result = default(string?);
            var request = GetRequest(method, path, parameter);

            request.Headers.Add("Accept", "*/*");

            try
            {
                if (request.Method == HttpMethod.Post)
                {
                    var content = GetFormContent(parameter?.FromBody);
                    Response = await HttpClient.PostAsync(request.RequestUri, content);
                }
                else
                {
                    Response = await HttpClient.SendAsync(request);
                }

                if (Response.IsSuccessStatusCode)
                    result = await Response.Content.ReadAsStringAsync();
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

        public async Task<Stream?> RequestStreamAsync(HttpMethod method, string path = "", HttpRequestParameter? parameter = null)
        {
            var result = default(Stream?);
            var request = GetRequest(method, path, parameter);

            request.Headers.Add("Accept", "*/*");

            try
            {
                if (request.Method == HttpMethod.Post)
                {
                    var content = GetFormContent(parameter?.FromBody);
                    Response = await HttpClient.PostAsync(request.RequestUri, content);
                }
                else
                {
                    Response = await HttpClient.SendAsync(request);
                }

                if (Response.IsSuccessStatusCode)
                    result = await Response.Content.ReadAsStreamAsync();
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

        public string? RequestString(HttpMethod method, string path = "", HttpRequestParameter? parameter = null)
        {
            return HttpHelper.RunSync<string>(() => RequestStringAsync(method, path, parameter));
        }

        public Stream? RequestStream(HttpMethod method, string path = "", HttpRequestParameter? parameter = null)
        {
            return HttpHelper.RunSync<Stream>(() => RequestStreamAsync(method, path, parameter));
        }
    }
}