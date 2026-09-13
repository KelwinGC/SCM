using SCM.ApiLoadProcess.Dominio.Base;


namespace SCM.ApiLoadProcess.Dominio.Entidad
{
    public class DataProcesada: BaseAuditableEntity
    {
        public int IdData { get; set; }
        public int IdCarga { get; set; }
        public string Periodo { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }

    }
}
