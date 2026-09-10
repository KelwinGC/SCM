using SCM.ApiAutenticacion.Aplicacion.DTO.Request;

namespace SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort
{
    public interface IGetAutenticacionInputPort
    {
        Task Handle(GetAutenticacionDTO resquest);
    }
}
