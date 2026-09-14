using Microsoft.AspNetCore.Http;
using SCM.ApiNotifications.Transversal.Interface;
using System.Security.Claims;

namespace SCM.ApiNotifications.Transversal.Service
{
    public class Security : ISecurity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Security(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, string> GetUser()
        {
            var UserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            var Username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string Authorization = _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(x=>x.Key == "Authorization").Value;
            var token = Authorization.Split(" ")[1];

            return new Dictionary<string, string> {
                {"UserId",UserId! },
                {"Username",Username! },
                {"Token",token }
            };
        }


    }
}
