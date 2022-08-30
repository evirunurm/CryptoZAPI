using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public interface IRepository<T> 
        where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);

        Task<List<T>> FindBy(Expression<Func<T, bool>> expression);

        Task<T> Create(T o);


        Task CreateRange(IEnumerable<T> range);
        Task<T> Update(T o, int id);

        Task SaveDB();

    }


    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ILogger _logger;
        private DbContext _context;
        private DbSet<T> dbSet;

        public Repository( DbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbSet = _context.Set<T>();
        }



        public async Task<T> Create(T o) // throws OperationCanceledException
        {
            await dbSet.AddAsync(o);
            return o;
        }

        public async Task CreateRange(IEnumerable<T> range) // throws OperationCanceledException
        {
            await dbSet.AddRangeAsync(range);
        }

        public async Task<List<T>> FindBy(Expression<Func<T, bool>> expression) // throws ArgumentNullException, OperationCanceledException
        {
           return await dbSet.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll() // throws ArgumentNullException, OperationCanceledException
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id) // throws KeyNotFoundException
        {
            var foundObj = await dbSet.FindAsync(id);
            return foundObj ?? throw new KeyNotFoundException();
        }

        public async Task SaveDB() // throws DbUpdateException, DbUpdateConcurrencyException, OperationCanceledException
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T o, int id) // throws KeyNotFoundException
        {
            var entity = await GetById(id);
            _context.Entry(entity).CurrentValues.SetValues(o);
            return o;
        }

    }
}
