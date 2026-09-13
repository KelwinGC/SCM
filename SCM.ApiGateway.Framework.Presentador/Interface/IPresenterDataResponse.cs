namespace SCM.ApiGateway.Framework.Presentador.Interfase
{
    public interface IPresenterDataResponse<FormatoDataType>
    {
        public FormatoDataType Contenido { get; }
    }
}
