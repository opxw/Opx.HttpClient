namespace Opx.Http
{
    public interface IDocRequest
    {
        Task<string?> RequestStringAsync(HttpMethod method, string path,
            HttpRequestParameter? parameter = null);
        Task<Stream?> RequestStreamAsync(HttpMethod method, string path,
            HttpRequestParameter? parameter = null);
        string? RequestString(HttpMethod method, string path,
            HttpRequestParameter? parameter = null);
        Stream? RequestStream(HttpMethod method, string path,
            HttpRequestParameter? parameter = null);
    }
}