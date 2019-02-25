using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class UnidadeMedidaMap : EntityTypeConfiguration<UnidadeMedidaModel>
    {
        public UnidadeMedidaMap()
        {
            // Nome da tabela
            ToTable("tb_unidadeMedida");

            // Chave primaria e autoincremento
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Dando nome aos campos
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
            Property(x => x.Sigla).HasColumnName("sigla").HasMaxLength(3).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();
        }
    }
}