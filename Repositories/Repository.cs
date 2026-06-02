using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext context;
    protected readonly DbSet<T> dbSet;

    public Repository(AppDbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public virtual Task<List<T>> GetAllAsync() => dbSet.ToListAsync();

    public virtual Task<T?> GetByIdAsync(int id) => dbSet.FindAsync(id).AsTask();

    public virtual async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public virtual void Update(T entity)
    {
        dbSet.Update(entity);
        context.SaveChanges();
    }

    public virtual void Delete(T entity)
    {
        dbSet.Remove(entity);
        context.SaveChanges();
    }
}
