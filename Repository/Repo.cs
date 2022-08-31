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

        // Must be IQueryable to do ToListAsync();
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression); // throws ArgumentNullException, OperationCanceledException

        Task<T> Create(T o); // throws OperationCanceledException


        Task CreateRange(IEnumerable<T> range); // throws OperationCanceledException
        Task<T> Update(T o);

        Task SaveDB();

    }


    public class Repository<T> : IRepository<T>, IDisposable
        where T : class
    {
        private readonly ILogger _logger;
        private DbContext _context;
        private DbSet<T> dbSet;

        public Repository(ILogger logger = null)
        {
            _context = new CryptoZContext();
            _logger = logger;
            dbSet = _context.Set<T>();
        }



        public async Task<T> Create(T o) 
        {
            await dbSet.AddAsync(o);
            return o;
        }

        public async Task CreateRange(IEnumerable<T> range) 
        {
            await dbSet.AddRangeAsync(range);
        }



        public IQueryable<T> FindBy(Expression<Func<T, bool>> expression) 
        {
            return dbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> GetAll() // throws ArgumentNullException, OperationCanceledException
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id) // throws KeyNotFoundException
        {
            var foundObj = await dbSet.FindAsync(id);
            // var foundObj = await dbSet.FirstOrDefaultAsync(t => t.Id == id);

            return foundObj ?? throw new KeyNotFoundException();
        }

        public async Task SaveDB() // throws DbUpdateException, DbUpdateConcurrencyException, OperationCanceledException
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T o) // throws KeyNotFoundException
        {
            // var entity = await GetById(id);
            //_context.Attach<T>(entity);


            //_context.Entry(entity).CurrentValues.SetValues(o);

            dbSet.Attach(o);
            _context.Entry(o).State = EntityState.Modified;
            return o;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null; // TODO: Check this
                }
            }
        }

    }
}
