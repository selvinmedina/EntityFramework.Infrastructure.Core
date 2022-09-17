using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Infrastructure.Core.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddConfiguration<TEntity>(
          this ModelBuilder modelBuilder,
          IEntityTypeConfiguration<TEntity> entityConfiguration) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);

            return modelBuilder;
        }
    }
}
