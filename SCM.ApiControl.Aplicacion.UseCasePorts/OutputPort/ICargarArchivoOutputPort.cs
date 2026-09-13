using SCM.ApiControl.Aplicacion.DTO.Response;

namespace SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort
{
    public interface ICargarArchivoOutputPort
    {
        Task Handle(ResponseHeaderDTO requestHeader, CargaArchivoDTO cargaArchivoDTO);
    }
}
