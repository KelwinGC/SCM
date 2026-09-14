using AutoMapper;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;


namespace SCM.ApiGateway.Aplicacion.DTO.Mappings
{
    public class EntitiApiRestToDtoProfile : Profile
    {
        public EntitiApiRestToDtoProfile()
        {
            CreateMap<AutenticacionScmResponse, ResponseGenericoDto<TokenAutenticacionResponseDto>>();
            CreateMap<TokenAcceso, TokenAutenticacionResponseDto>();
            CreateMap<ControlScmResponse, ResponseGenericoDto<ControlResponseDto>>();
            CreateMap<CargaArchivoResponse, ControlResponseDto>();
        }
    }
}
