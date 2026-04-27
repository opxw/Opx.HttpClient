namespace Opx.Http
{
    internal class HttpHandler : HttpClientHandler
    {
        public HttpHandler()
        {
            UseProxy = false;
            ServerCertificateCustomValidationCallback += (sender, certificate, chain, SslPolicyErrors) => true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}