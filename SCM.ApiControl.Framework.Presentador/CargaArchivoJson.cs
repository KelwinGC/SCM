using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiControl.Framework.Presentador
{
    internal class CargaArchivoJson : ICargarArchivoOutputPort, IPresenteDataResponse<JSON<CargaArchivoDTO>>
    {
        public JSON<CargaArchivoDTO> Contenido { get; private set; }

        public Task Handle(ResponseHeaderDTO requestHeader, CargaArchivoDTO cargaArchivoDTO)
        {
            Contenido = new JSON<CargaArchivoDTO>
            {
                Codigo = requestHeader.Codigo,
                Mensaje = requestHeader.Mensaje,
                Data = cargaArchivoDTO
            };

            return Task.CompletedTask;
        }
    }
}
