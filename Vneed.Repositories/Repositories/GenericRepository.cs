using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Repositories.Interfaces;
using Vneed.Common.Exceptions.Interfaces;
using Vneed.Data.Models;


namespace Vneed.Repositories.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
        
    public async Task<T> GetById(int id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        // ISoftDeletable interface'i uygulanmışsa soft delete uygularız.
        if (entity is ISoftDeletable deletableEntity)
        {
            deletableEntity.DeletedAt = DateTimeOffset.UtcNow;
            deletableEntity.IsActive = false;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        
        //  ISoftDeletable değilse (örneğin UserToken, UserMailToken gibi) hard delete yapılabilir
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
    
    // Paging için kullanıyoruz.
    public IQueryable<T> GetAllQueryable() 
    {
        return _dbSet.AsQueryable();
    }
}