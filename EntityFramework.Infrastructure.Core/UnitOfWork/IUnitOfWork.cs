using SelvinMedina.EntityFramework.Infrastructure.Core.Repository;
using System.Data.Common;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        IEnumerable<T> SqlQuery<T>(string query, Func<DbDataReader, T> map, params object[] parameters);
        void BeginTransaction();
        void Commit();
        void RollBack();
        bool Save();
        Task<bool> SaveAsync();
    }
}
