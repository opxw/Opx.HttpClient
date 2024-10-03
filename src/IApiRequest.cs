namespace Opx.Http
{
    public interface IApiRequest
    {
        Task<T?> RequestJsonAsync<T>(HttpMethod method, string path, ApiRequestParameter? parameter = null);
        T? RequestJson<T>(HttpMethod method, string path, ApiRequestParameter? parameter = null);
    }
}