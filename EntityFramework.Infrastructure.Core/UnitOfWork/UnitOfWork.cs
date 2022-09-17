using SelvinMedina.EntityFramework.Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace SelvinMedina.EntityFramework.Infrastructure.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new EntityRepository<TEntity>(_dbContext);
        }

        public void RollBack()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        protected virtual void SaveChangesException(Exception ex)
        {
            throw ex;
        }

        public bool Save()
        {
            if (_transaction == null)
            {
                try
                {
                    BeginTransaction();
                    _dbContext.SaveChanges();
                    Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    RollBack();
                    SaveChangesException(ex);
                    return false;
                }
            }
            else
            {
                try
                {
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    SaveChangesException(ex);
                    return false;
                }
            }
        }

        public async Task<bool> SaveAsync()
        {
            if (_transaction == null)
            {
                try
                {
                    BeginTransaction();
                    await _dbContext.SaveChangesAsync();
                    Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    RollBack();
                    SaveChangesException(ex);
                    return false;
                }
            }
            else
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    SaveChangesException(ex);
                    return false;
                }
            }
        }

        public IEnumerable<T> SqlQuery<T>(string query, Func<DbDataReader, T> map, params object[] parameters)
        {
            using (DbCommand command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(parameters);

                using (DbDataReader result = command.ExecuteReader())
                {
                    List<T> entities = new List<T>();
                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }
                    return entities;
                }
            }
        }
    }
}
