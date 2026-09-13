namespace SCM.ApiLoadProcess.Transversal.Interface
{
    public interface ISecurity
    {
        Dictionary<string, string> GetUser();
        //string GetIpTerminal();
    }
}
