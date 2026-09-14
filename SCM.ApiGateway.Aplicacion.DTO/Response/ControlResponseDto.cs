using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM.ApiGateway.Aplicacion.DTO.Response
{
    public class ControlResponseDto
    {
        public int IdCarga { get; set; }
        public string NombreArchivo { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public long TamanoBytes { get; set; }
        public string RutaArchivo { get; set; }
    }
}
