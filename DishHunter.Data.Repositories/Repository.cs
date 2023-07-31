namespace DishHunter.Data.Repositories
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Interfaces;

    public class Repository : IRepository
    {
        public Repository(ApplicationDbContext context)
        {
            this.Context = context;
        }
        protected DbContext Context { get; set; }
        protected DbSet<T> DbSet<T>() where T : class
        {
            return this.Context.Set<T>();
        }
        public async Task<IEnumerable<T>> ExecuteProc<T>(string procedureName, params object[] args) where T : class
        {
            return await this.Context.Set<T>().FromSqlRaw("/*NO LOAD BALANCE*/ select * from " + procedureName, args).ToListAsync();
        }
        public async Task<IEnumerable<T>> ExecuteSQL<T>(string query, params object[] args) where T : class
        {
            return await this.Context.Set<T>().FromSqlRaw("/*NO LOAD BALANCE*/ " + query, args).ToListAsync();
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await DbSet<T>().AddAsync(entity);
        }
        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            await DbSet<T>().AddRangeAsync(entities);
        }
        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>().AsQueryable();
        }

        public IQueryable<T> All<T>(Expression<Func<T, bool>> search) where T : class
        {
            return this.DbSet<T>().Where(search).AsQueryable();
        }
        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return this.DbSet<T>()
                .AsQueryable()
                .AsNoTracking();
        }
        public IQueryable<T> AllReadonly<T>(Expression<Func<T, bool>> search) where T : class
        {
            return this.DbSet<T>()
                .Where(search)
                .AsQueryable()
                .AsNoTracking();
        }
        public async Task DeleteAsync<T>(object id) where T : class
        {
            T entity = await GetByIdAsync<T>(id);

            Delete<T>(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            EntityEntry entry = this.Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.DbSet<T>().Attach(entity);
            }

            entry.State = EntityState.Deleted;
        }
        public void Detach<T>(T entity) where T : class
        {
            EntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }
        public void Dispose()
        {
            this.Context.Dispose();
        }
        public async Task<T> GetByIdAsync<T>(object id) where T : class
        {
            return await DbSet<T>().FindAsync(id);
        }
        public async Task<T> GetByIdsAsync<T>(object[] id) where T : class
        {
            return await DbSet<T>().FindAsync(id);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await this.Context.SaveChangesAsync();
        }
        public void Update<T>(T entity) where T : class
        {
            this.DbSet<T>().Update(entity);
        }
        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            this.DbSet<T>().UpdateRange(entities);
        }
        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            this.DbSet<T>().RemoveRange(entities);
        }
        public void DeleteRange<T>(Expression<Func<T, bool>> deleteWhereClause) where T : class
        {
            var entities = All<T>(deleteWhereClause);
            DeleteRange(entities);
        }
        public async Task Truncate(string table)
        {
            await Context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {table} RESTART IDENTITY");
        }
        public void ChangeTrackerClear()
        {
            this.Context.ChangeTracker.Clear();
        }
    }
}