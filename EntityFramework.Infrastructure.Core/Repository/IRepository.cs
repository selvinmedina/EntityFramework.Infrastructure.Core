using System.Linq.Expressions;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        IEnumerable<TEntity>? Get();
        TEntity? Get(int id);

        void Add(TEntity entity);
        void AddRange(params TEntity[] entities);

        bool Delete(int id);
        void DeleteWhere(Expression<Func<TEntity, bool>> filter);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
