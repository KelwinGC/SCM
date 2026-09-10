using SCM.ApiAutenticacion.Dominio.Entidad;

namespace SCM.ApiAutenticacion.Dominio.Interface
{
    public interface IUserRepositorio
    {
        public Task<User> Authenticate(string userName, string password);
    }
}
