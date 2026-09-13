using SCM.ApiLoadProcess.Aplicacion.DTO.Request;

namespace SCM.ApiLoadProcess.Aplicacion.UseCasePorts.InputPort
{
    public interface ILoadProcessArchivoInputPort
    {
        Task Handle(PeticionLoadProcessArchivoDTO resquest);
    }
}
