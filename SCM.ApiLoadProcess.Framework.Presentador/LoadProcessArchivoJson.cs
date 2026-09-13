using SCM.ApiLoadProcess.Aplicacion.DTO.Response;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiLoadProcess.Framework.Presentador
{
    internal class LoadProcessArchivoJson : ILoadProcessArchivoOutputPort, IPresenteDataResponse<JSON<CargaArchivoDTO>>
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
