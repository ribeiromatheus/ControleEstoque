using AutoMapper;
using ControleEstoque.Web.Models;

namespace ControleEstoque.Web
{
    // Aqui vamos criar todos os mapeamentos entre classes (de Domain para ViewModel e vice-versa)
    public static class AutoMapperProfile
    {
        public static IMapper CreateConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CidadeViewModel, CidadeModel>().ReverseMap();
                cfg.CreateMap<EstadoViewModel, EstadoModel>().ReverseMap();
                cfg.CreateMap<FornecedorViewModel, FornecedorModel>().ReverseMap();
                cfg.CreateMap<GrupoProdutoViewModel, GrupoProdutoModel>().ReverseMap();
                cfg.CreateMap<LocalArmazenamentoViewModel, LocalArmazenamentoModel>().ReverseMap();
                cfg.CreateMap<MarcaProdutoViewModel, MarcaProdutoModel>().ReverseMap();
                cfg.CreateMap<PaisViewModel, PaisModel>().ReverseMap();
                cfg.CreateMap<PerfilViewModel, PerfilModel>().ReverseMap();
                cfg.CreateMap<ProdutoViewModel, ProdutoModel>().ReverseMap();
                cfg.CreateMap<UnidadeMedidaViewModel, UnidadeMedidaModel>().ReverseMap();
                cfg.CreateMap<UsuarioViewModel, UsuarioModel>().ReverseMap();
                cfg.CreateMap<PaisViewModel, PaisModel>().ReverseMap();
            }).CreateMapper();

            return config;
        }
    }
}