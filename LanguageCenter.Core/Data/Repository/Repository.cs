using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Data.Repository
{
    public class Repository : IRepository
    {
       
        protected DbContext Context { get; set; }

        protected DbSet<T> DbSet<T>() where T : class
        {
            return Context.Set<T>();
        }

        public async Task<bool> Delete<T>(string id) where T : class
        {
            var entity = await GetByIdAsync<T>(id);

            var result = false;

            var entry = DbSet<T>().Remove(entity);

            if (entry != null)
            {
                result = true;
            }

            return result;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return DbSet<T>();
        }

        public async Task<T> GetByIdAsync<T>(string id) where T : class
        {
            return await DbSet<T>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
