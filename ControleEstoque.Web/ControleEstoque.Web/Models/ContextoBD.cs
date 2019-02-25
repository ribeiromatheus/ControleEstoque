using System.Data.Entity;

namespace ControleEstoque.Web.Models
{
    public class ContextoBD : DbContext  // Classe de contexto do banco de dados (gerencia conexão)
    {
        public ContextoBD() : base("name=principal") // ConnectionString
        {
        }

        // Mapeamento dos Domains(Models)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CidadeMap());
            modelBuilder.Configurations.Add(new EntradaProdutoMap());
            modelBuilder.Configurations.Add(new EstadoMap());
            modelBuilder.Configurations.Add(new FornecedorMap());
            modelBuilder.Configurations.Add(new GrupoProdutoMap());
            modelBuilder.Configurations.Add(new InventarioEstoqueMap());
            modelBuilder.Configurations.Add(new LocalArmazenamentoMap());
            modelBuilder.Configurations.Add(new MarcaProdutoMap());
            modelBuilder.Configurations.Add(new PaisMap());
            modelBuilder.Configurations.Add(new PerfilMap());
            modelBuilder.Configurations.Add(new ProdutoMap());
            modelBuilder.Configurations.Add(new SaidaProdutoMap());
            modelBuilder.Configurations.Add(new UnidadeMedidaMap());
            modelBuilder.Configurations.Add(new UsuarioMap());

        }

        // Por onde vamos fazer o acesso a dados
        public DbSet<CidadeModel> Cidades { get; set; }
        public DbSet <EntradaProdutoModel> EntradasProdutos { get; set; }
        public DbSet <EstadoModel> Estados { get; set; }
        public DbSet <FornecedorModel> Fornecedores { get; set; }
        public DbSet <GrupoProdutoModel> GruposProdutos { get; set; }
        public DbSet <InventarioEstoqueModel> InventariosEstoques { get; set; }
        public DbSet <LocalArmazenamentoModel> LocaisArmazenamentos { get; set; }
        public DbSet <MarcaProdutoModel> MarcasProdutos { get; set; }
        public DbSet <PaisModel> Paises { get; set; }
        public DbSet <PerfilModel> Perfis { get; set; }
        public DbSet <ProdutoModel> Produtos { get; set; }
        public DbSet <SaidaProdutoModel> SaidasProdutos { get; set; }
        public DbSet <UnidadeMedidaModel> UnidadesMedidas { get; set; }
        public DbSet <UsuarioModel> Usuarios { get; set; }
    }
}