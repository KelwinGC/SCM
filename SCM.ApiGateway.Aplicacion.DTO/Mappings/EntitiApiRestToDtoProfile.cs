using AutoMapper;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Infraestructura.WebServices.Response;


namespace SCM.ApiGateway.Aplicacion.DTO.Mappings
{
    public class EntitiApiRestToDtoProfile : Profile
    {
        public EntitiApiRestToDtoProfile()
        {
            //CreateMap<ConsultaPorNumeroDocumentoReniecIrmaResponse, ResponseGenericoDto<PersonaReniecIrmaResponseDto>>()
            //    .ForMember(des => des.Codigo, ori => ori.MapFrom(src => src.Codigo))
            //    .ForMember(des => des.Mensaje, ori => ori.MapFrom(src => src.Mensaje))
            //    .ForMember(des => des.Data, ori => ori.MapFrom(src => src.Data));
            CreateMap<AutenticacionKeyScmResponse, ResponseGenericoDto<TokenAutenticacionResponseDto>>();
            CreateMap<TokenAcceso, TokenAutenticacionResponseDto>();


        }
    }
}
