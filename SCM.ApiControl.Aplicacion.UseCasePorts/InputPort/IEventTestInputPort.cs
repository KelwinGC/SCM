using SCM.ApiControl.Aplicacion.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM.ApiControl.Aplicacion.UseCasePorts.InputPort
{
    public interface IEventTestInputPort
    {
        Task Handle(PeticionEventTestDTO request);
    }
}
