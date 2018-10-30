using AutoMapper;
using MyMVC.Models;

namespace MyMVC
{
    // Aqui vamos criar todos os mapeamentos entre classes (de Domain para ViewModel e vice-versa)
    public class AutoMapperProfile : Profile // Profile é a classe que mapeia perfil de mapeamentos
    {
        public AutoMapperProfile()
        {
            CreateMap<CidadeViewModel, CidadeModel>();
            CreateMap<EstadoViewModel, EstadoModel>();
            CreateMap<FornecedorViewModel, FornecedorModel>();
            CreateMap<GrupoProdutoViewModel, GrupoProdutoModel>();
            CreateMap<LocalArmazenamentoViewModel, LocalArmazenamentoModel>();
            CreateMap<MarcaProdutoViewModel, MarcaProdutoModel>();
            CreateMap<PaisViewModel, PaisModel>();
            CreateMap<PerfilViewModel, PerfilModel>();
            CreateMap<ProdutoViewModel, ProdutoModel>();
            CreateMap<UnidadeMedidaViewModel, UnidadeMedidaModel>();
            CreateMap<UsuarioViewModel, UsuarioModel>();
            CreateMap<PaisViewModel, PaisModel>();
        }
    }
}