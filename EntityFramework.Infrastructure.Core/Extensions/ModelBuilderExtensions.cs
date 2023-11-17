using Microsoft.EntityFrameworkCore;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddConfiguration<TConfig>(this ModelBuilder modelBuilder)
            where TConfig : class, new()
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder), "ModelBuilder cannot be null.");
            }

            try
            {
                var configInterface = typeof(TConfig).GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

                if (configInterface == null)
                {
                    throw new InvalidOperationException("The provided class does not implement IEntityTypeConfiguration<TEntity>.");
                }

                var entityType = configInterface.GetGenericArguments()[0];
                var configInstance = new TConfig();

                var configureMethod = configInterface.GetMethod("Configure");
                if (configureMethod == null)
                {
                    throw new InvalidOperationException("The Configure method was not found on the IEntityTypeConfiguration interface.");
                }

                var type = modelBuilder.GetType();
                var genericEntityMethod = type.GetMethods()
                    .FirstOrDefault(m => m.Name == "Entity" && m.IsGenericMethod && m.GetParameters().Length == 0);

                if (genericEntityMethod == null)
                {
                    throw new InvalidOperationException("The Entity method was not found on the ModelBuilder class.");
                }

                var entityTypeBuilder = genericEntityMethod.MakeGenericMethod(entityType).Invoke(modelBuilder, null);
                if (entityTypeBuilder == null)
                {
                    throw new InvalidOperationException("Failed to create an instance of EntityTypeBuilder.");
                }

                configureMethod.Invoke(configInstance, new[] { entityTypeBuilder });

                return modelBuilder;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // Consider rethrowing or handling the exception based on your use case
                throw;
            }
        }
    }
}
