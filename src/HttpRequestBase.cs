using System.Net;

namespace Opx.Http
{
    public class HttpRequestBase
    {
        private HttpClient _httpClient;
        private HttpResponseMessage? _response;
        private string _baseAddress = string.Empty;
        private string _requestedUrl = string.Empty;
        private string _errorMessage = string.Empty;
        private HttpRequestVersion _httpVersion { get; set; }
        private HttpVersionPolicy _httpVersionPolicy { get; set; }

        public HttpRequestBase(HttpRequestVersion httpVersion = HttpRequestVersion.Http20, 
            HttpVersionPolicy httpVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher)
        {
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler)
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan
            };
            _httpVersion = httpVersion;
            _httpVersionPolicy = httpVersionPolicy;
        }

        private static string ValidateBaseUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "";

            if (url.Last().Equals('/'))
                return url;
            else
                return $"{url}/";
        }

        protected virtual string GetUrl(string path)
        {
            return _baseAddress + path;
        }

        protected virtual HttpClient HttpClient => _httpClient;
        protected virtual HttpResponseMessage? Response
        {
            get => _response;
            set => _response = value;
        }

        protected virtual string GetUrlFromRoute(string url, object route)
        {
            var result = url;

            if (route == null)
                return result;

            var props = route.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(route, null)?.ToString();
                result = result.Replace("{" + prop.Name + "}", value);
            }

            return result;
        }

        protected virtual string GetUrlFromQuery(string url, object query)
        {
            var result = url;

            if (query == null)
                return result;

            var queryPath = string.Empty;
            var props = query.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(query, null)?.ToString();
                queryPath += $"{prop.Name}={value}&";
            }

            if (!string.IsNullOrWhiteSpace(queryPath))
                result += $"?{queryPath.Substring(0, queryPath.Length - 1)}";

            return result;
        }

        protected virtual FormUrlEncodedContent? GetFormContent(object content)
        {
            var result = default(FormUrlEncodedContent);
            var props = content.GetType().GetProperties();
            var keys = new List<KeyValuePair<string, string>>();

            foreach (var prop in props)
            {
                var value = prop.GetValue(content, null)?.ToString();
                keys.Add(new KeyValuePair<string, string>(prop.Name, value));
            }

            if (keys.Count > 0)
            {
                result = new FormUrlEncodedContent(keys.ToArray());
            }

            return result;
        }

        protected virtual void SetRequestedUrl(string url)
        {
            _requestedUrl = url;
        }

        protected virtual void SetErrorFromResponse()
        {
            _errorMessage = !string.IsNullOrWhiteSpace(_response.ReasonPhrase) ? _response.ReasonPhrase : _response.ToString();
        }

        protected virtual void SetError(string message)
        {
            _errorMessage = message;
        }

        protected virtual HttpRequestMessage GetRequest(HttpMethod method, string path, HttpRequestParameter? parameter = null)
        {
            var url = GetUrl(path);

            if (parameter != null)
            {
                if (parameter.FromRoute != null)
                    url = GetUrlFromRoute(url, parameter.FromRoute);

                if (parameter.FromQuery != null)
                    url = GetUrlFromQuery(url, parameter.FromQuery);
            }

            if (BaseAddress.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var uri = new Uri(url);
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = method,
            };

            if (parameter != null)
            {
                if (parameter.Headers != null && parameter.Headers.Count > 0)
                {
                    foreach (var header in parameter.Headers)
                        request.Headers.Add(header.Key, header.Value);
                }
            }

            _requestedUrl = url;

            request.Version = _httpVersion.GetVersionNumber();
            request.VersionPolicy = _httpVersionPolicy;

            return request;
        }

        public string BaseAddress
        {
            get => _baseAddress;
            set => _baseAddress = ValidateBaseUrl(value);
        }

        public string RequestedUrl => _requestedUrl;
        public string ErrorMessage => _errorMessage;
        public TimeSpan TimeOut
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }
        public HttpVersionPolicy HttpVersionPolicy
        {
            get => _httpVersionPolicy;
            set => _httpVersionPolicy = value;
        }
        public HttpRequestVersion HttpVersion
        {
            get => _httpVersion;
            set => _httpVersion = value;
        }
        public HttpResponseMessage? ResponseMessage => _response;
    }
}
