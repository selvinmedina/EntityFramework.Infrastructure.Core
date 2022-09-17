using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.Repository
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EntityRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public bool Delete(int id)
        {
            var entity = Get(id);

            if (entity is null) return false;

            _dbSet.Remove(entity);

            return true;
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> filter)
        {
            var entities = _dbSet.Where(filter).AsEnumerable();

            foreach (var entity in entities)
            {
                _dbSet.Remove(entity);
            }
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters);
        }

        public IEnumerable<TEntity>? Get()
        {
            return _dbSet.AsEnumerable();
        }

        public TEntity? Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AttachRange(entities);
            _dbContext.Entry(entities).State = EntityState.Modified;
        }
    }
}
