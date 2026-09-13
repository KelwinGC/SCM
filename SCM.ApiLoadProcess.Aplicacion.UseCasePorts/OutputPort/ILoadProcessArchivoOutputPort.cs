using SCM.ApiLoadProcess.Aplicacion.DTO.Response;

namespace SCM.ApiLoadProcess.Aplicacion.UseCasePorts.OutputPort
{
    public interface ILoadProcessArchivoOutputPort
    {
        Task Handle(ResponseHeaderDTO requestHeader, CargaArchivoDTO cargaArchivoDTO);
    }
}
