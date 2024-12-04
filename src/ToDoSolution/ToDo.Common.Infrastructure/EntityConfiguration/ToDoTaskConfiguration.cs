using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Common.Domain.Entities;

namespace ToDo.Common.Infrastructure.EntityConfiguration
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            BaseConfiguration(builder);
            EntityConfiguration(builder);
        }

        protected void BaseConfiguration(EntityTypeBuilder<T> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Id).ValueGeneratedNever();

            entityTypeBuilder.Property(x => x.CreatedAt).IsRequired();
            entityTypeBuilder.Property(x => x.UpdatedAt).IsRequired(false);
        }

        public abstract void EntityConfiguration(EntityTypeBuilder<T> entityTypeBuilder);
    }
}
