using AuditingSystem.Database;
using AuditingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly AuditingSystemDbContext context;

    public BaseRepository(AuditingSystemDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private async Task ExecuteInContextAsync(Func<AuditingSystemDbContext, Task> action)
    {
        await action(context);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await ExecuteInContextAsync(async dbContext => await Task.CompletedTask);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await ExecuteInContextAsync(async dbContext => await dbContext.Set<TEntity>().AddAsync(entity));
    }

    public async Task DeleteAsync(int id)
    {
        var entityToDelete = await context.Set<TEntity>().FindAsync(id);
        if (entityToDelete != null)
        {
            var softDeleteProperty = entityToDelete.GetType().GetProperty("IsDeleted");
            if (softDeleteProperty != null && softDeleteProperty.PropertyType == typeof(bool))
            {
                softDeleteProperty.SetValue(entityToDelete, true);
                context.Entry(entityToDelete).State = EntityState.Modified;
            }
            else
            {
                context.Set<TEntity>().Remove(entityToDelete);
            }
        }
        await ExecuteInContextAsync(async dbContext => await Task.CompletedTask);
    }

    public async Task<TEntity?> FindByAsync(int? id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>?[] includes)
    {
        var query = context.Set<TEntity>().AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.ToListAsync();
    }

    public string GetEntityType()
    {
        return typeof(TEntity).Name;
    }
}
