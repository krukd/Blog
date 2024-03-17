using Blog.DAL.Data.DB;
using Blog.DAL.Data.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.DAL.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _appContext;

        private Dictionary<Type, object>? _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _appContext = dbContext;
        }

        public void Dispose()
        {
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = _appContext.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_appContext);
            }

            return (IRepository<TEntity>)_repositories[type];

        }
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }
    }
}
