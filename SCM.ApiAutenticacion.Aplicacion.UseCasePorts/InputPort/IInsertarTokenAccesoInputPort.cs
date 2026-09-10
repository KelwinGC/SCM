namespace SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort
{
    public interface IInsertarTokenAccesoInputPort
    {
        Task Handle(Guid ClienteAppId, string Token, string UsuarioAplicacion);
    }
}
