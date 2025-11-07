using AutoMapper;
using ChallangeMottu.Domain;

namespace ChallangeMottu.Application.Mappings;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        // Usuario -> UsuarioDto (Response)
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.MotoId, opt => opt.MapFrom(src => src.MotoId));

        // CreateUsuarioDto -> Usuario (usando construtor)
        CreateMap<CreateUsuarioDto, Usuario>()
            .ConstructUsing(dto => new Usuario(
                dto.Nome,
                dto.Email,
                dto.Senha,  // ✅ Passa a senha - construtor faz o hash
                dto.MotoId
            ))
            .ForAllMembers(opt => opt.Ignore()); // Ignora mapeamento de propriedades (já foi tudo pelo construtor)

        // UpdateUsuarioDto -> Usuario (NÃO CRIA novo usuário, só atualiza dados)
        CreateMap<UpdateUsuarioDto, Usuario>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Não altera o ID
            .AfterMap((dto, usuario) => 
            {
                // Usa o método público da entidade para atualizar
                usuario.AtualizarDados(dto.Nome, dto.Email, dto.MotoId);
            });

        // Moto -> MotoResumoDto
        CreateMap<Moto, MotoResumoDto>();
    }
}