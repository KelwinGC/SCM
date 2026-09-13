namespace SCM.ApiControl.Framework.Presentador
{
    public interface IPresenteDataResponse<FormatoDataType>
    {
        public FormatoDataType Contenido { get; }
    }
}
