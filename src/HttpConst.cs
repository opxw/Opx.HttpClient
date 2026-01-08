namespace Opx.Http
{
    public enum HttpRequestVersion
    {
        Http10,
        Http11,
        Http20,
        Http30
    }

    public enum HttpHandlerKind
    {
        HttpClient,
        Socket
    }
}