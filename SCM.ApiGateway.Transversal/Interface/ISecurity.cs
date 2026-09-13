namespace SCM.ApiGateway.Transversal.Interface
{
    public interface ISecurity
    {
        Dictionary<string, string> GetUser();
    }
}
