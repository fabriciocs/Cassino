using AutoMapper;
using Cassino.Application.Dtos.V1.Base;
using Cassino.Domain.Entities;
using Cassino.Domain.Paginacao;

namespace Cassino.Application.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Administrador

        CreateMap<Dtos.V1.Administrador.AdministradorDto, Administrador>().ReverseMap();
        CreateMap<Dtos.V1.Administrador.AdicionarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<Dtos.V1.Administrador.AlterarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<PagedDto<Dtos.V1.Administrador.BuscarAdministradorDto>, ResultadoPaginado<Administrador>>().ReverseMap();

        #endregion
        
        
        CreateMap<Dtos.V1.Usuario.CadastrarUsuarioDto, Usuario>().ReverseMap();
        CreateMap<Dtos.V1.Usuario.AlterarUsuarioDto, Usuario>().ReverseMap();
        CreateMap<Dtos.V1.Usuario.UsuarioDto, Usuario>().ReverseMap();
        CreateMap<Dtos.V1.Usuario.BuscarUsuarioDto, ResultadoPaginado<Usuario>>().ReverseMap();
        CreateMap<Dtos.V1.Aposta.AdicionarApostaDto, Aposta>().ReverseMap();
        CreateMap<Dtos.V1.Saldo.SaldoUsuarioDto, Usuario>().ReverseMap();
        CreateMap<Dtos.V1.Aposta.VMApostaDto, Aposta>().ReverseMap();
    }
}