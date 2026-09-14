namespace SCM.ApiNotifications.Framework.Presentador
{
    public class JSON<T>
    {
        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public T? Data { get; set; }
    }
}
