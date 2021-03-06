using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanSist.Domain.Entities;

namespace FinanSist.Database.Mappings
{
    public class BaseMapping<TEntity> where TEntity : BaseEntity
    {
#nullable disable
        protected EntityTypeBuilder<TEntity> entity;

        public virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.entity = modelBuilder.Entity<TEntity>(); // Retorna o objeto, para configurar a entidade no modelo.
            entity.ToTable(typeof(TEntity).Name).HasCharSet("utf8"); //Nome da tabela
            entity.HasKey(t => t.Id); //Definindo Id
            entity.Property(a => a.Id).HasColumnType("char(36)").IsRequired(); //Definindo o tipo para ser um id do tipo guid
        }
    }
}