using SCM.ApiControl.Aplicacion.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort
{
    public interface IEventTestOutputPort
    {
        Task Handle(ResponseHeaderDTO requestHeader, ResponseEventTestDTO eventTestDTO);

    }
}
