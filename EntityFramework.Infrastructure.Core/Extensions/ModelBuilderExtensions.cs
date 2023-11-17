using Microsoft.EntityFrameworkCore;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddConfiguration<TEntity>(
          this ModelBuilder modelBuilder) where TEntity : class
        {
            var entityMap = (IEntityTypeConfiguration<>)Activator.CreateInstance(typeof(TEntity))!;
            var configureMethod = entityMap.GetType().GetMethod("Configure")!;

            modelBuilder.Entity<TEntity>(configureMethod.Invoke();

            return modelBuilder;
        }
    }
}
