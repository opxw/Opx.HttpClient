namespace Opx.Http
{
    public class HttpRequestParameter
    {
        public object? FromQuery { get; set; }
        public object? FromRoute { get; set; }
        public object? FromBody { get; set; }
        public Dictionary<string, string?>? Headers { get; set; }
    }
}
