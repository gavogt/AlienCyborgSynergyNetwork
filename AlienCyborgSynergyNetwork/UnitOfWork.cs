using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CyborgSessionDBContext _context;

        public ICyborgSessionRepository Sessions { get; }

        public UnitOfWork(CyborgSessionDBContext context)
        {
            _context = context;
            Sessions = new CyborgSessionRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
