using Blog.DAL.Data.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();
            Set = set;
        }

        public async Task Add(T item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            
            return await Set.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Set.ToListAsync();
        }

        
        public async Task Delete(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            var currentItem = await Set.FindAsync(GetKeyValue(item));

            if (currentItem != null)
            {
                _db.Entry(currentItem).State = EntityState.Detached;
            }

            Set.Update(item);
            await _db.SaveChangesAsync();
        }

        private object GetKeyValue(T item)
        {
            var key = _db.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            return item.GetType().GetProperty(key.Name).GetValue(item);
        }

        
    }
}
