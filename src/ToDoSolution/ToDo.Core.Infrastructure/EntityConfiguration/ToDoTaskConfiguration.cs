using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Common.Infrastructure.EntityConfiguration;
using ToDo.Core.Domain.Entities;

namespace ToDo.Core.Infrastructure.EntityConfiguration
{
    public class ToDoTaskConfiguration : BaseEntityConfiguration<ToDoTaskEntity>
    {
        public override void EntityConfiguration(EntityTypeBuilder<ToDoTaskEntity> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.Title).IsRequired();
            entityTypeBuilder.Property(x => x.Description).IsRequired(false);
            entityTypeBuilder.Property(x => x.ExpiryAt).IsRequired();
            entityTypeBuilder.Property(x => x.CompletionPercentage).IsRequired();
        }
    }
}
