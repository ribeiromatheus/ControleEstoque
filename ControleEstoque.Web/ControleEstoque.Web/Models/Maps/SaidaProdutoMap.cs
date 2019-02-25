using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class SaidaProdutoMap : EntityTypeConfiguration<SaidaProdutoModel>
    {
        public SaidaProdutoMap()
        {
            // Nome tabela
            ToTable("saida_produto");

            // Chave primária e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome aos campos
            Property(x => x.Numero).HasColumnName("numero").HasMaxLength(10).IsRequired();
            Property(x => x.Data).HasColumnName("data").IsRequired();
            Property(x => x.Quantidade).HasColumnName("quant").IsRequired();

            // Chave estrangeira
            Property(x => x.IdProduto).HasColumnName("id_produto").IsRequired();
            HasRequired(x => x.Produto).WithMany().HasForeignKey(x => x.IdProduto).WillCascadeOnDelete(false);
        }
    }
}