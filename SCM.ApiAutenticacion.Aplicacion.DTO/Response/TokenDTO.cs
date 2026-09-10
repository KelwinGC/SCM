using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM.ApiAutenticacion.Aplicacion.DTO.Response
{
    public class TokenDTO
    {
        public string Token { get; set; } = null!;
        //public string SessionId { get; set; }
        public string Usuario { get; set; } = null!;

    }
}
