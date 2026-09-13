namespace SCM.ApiGateway.Aplicacion.DTO.Response
{
    public class ResponseGenericoDto<T>
    {
        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public T Data { get; set;}
    }
}
