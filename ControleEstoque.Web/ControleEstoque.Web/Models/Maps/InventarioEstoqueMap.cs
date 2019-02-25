using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class InventarioEstoqueMap : EntityTypeConfiguration<InventarioEstoqueModel>
    {
        public InventarioEstoqueMap()
        {
            // Nome da tabela
            ToTable("inventario_estoque");

            // Chave primária
            HasKey(x => x.Id);
            // Dando nome a coluna e autoincremento
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome a coluna 
            Property(x => x.Data).HasColumnName("data").IsRequired();
            Property(x => x.QuantidadeEstoque).HasColumnName("quant_estoque").IsRequired();
            Property(x => x.QuantidadeInventario).HasColumnName("quant_inventario").IsRequired();
            Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(100).IsOptional();

            
            // Dando nome a coluna 
            Property(x => x.IdProduto).HasColumnName("id_produto").IsRequired();
            // Associação ao objeto id_estado. Chave estrangeira. WillCascadeOnDelete é se
            // quando eu apagar a tabela cidade, ele deveria apagar o id_estado ou não.
            HasRequired(x => x.Produto).WithMany().HasForeignKey(x => x.IdProduto).WillCascadeOnDelete(false);
        }
    }
}