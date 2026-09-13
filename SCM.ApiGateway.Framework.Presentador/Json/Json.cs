
namespace SCM.ApiGateway.Framework.Presentador.Json
{
    public class Json<T>
    {
        public string? codigo { get; set; }
        public string? mensaje { get; set; }
        public T? data { get; set; }
    }
}
