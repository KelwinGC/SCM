namespace SCM.ApiAutenticacion.Framework.Presentador
{
    public interface IPresenteDataResponse<FormatoDataType>
    {
        public FormatoDataType Contenido { get; }
    }
}
