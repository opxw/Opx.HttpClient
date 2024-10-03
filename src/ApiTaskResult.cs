namespace Opx.Http
{
    public class ApiTaskResult
    {
        public bool Result { get; set; }
        public dynamic? Data { get; set; }
        public string StatusCode { get; set; }
    }
}