namespace SCM.ApiNotifications.Framework.Presentador
{
    public interface IPresenteDataResponse<FormatoDataType>
    {
        public FormatoDataType Contenido { get; }
    }
}
