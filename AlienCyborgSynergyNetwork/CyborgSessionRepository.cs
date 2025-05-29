using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    internal class CyborgSessionRepository : ICyborgSessionRepository
    {
        private readonly CyborgSessionDBContext _context;

        public CyborgSessionRepository(CyborgSessionDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CyborgSession session)
        {
            await _context.CyborgSessions.AddAsync(session).AsTask();
        }

        public void Delete(CyborgSession session)
        {
            _context.CyborgSessions.Remove(session);
        }

        public Task<CyborgSession?> GetByIdAsync(Guid id)
        {
           return _context.CyborgSessions.FindAsync(id).AsTask();
        }

        public Task<List<CyborgSession>> ListAsync()
        {
            return _context.CyborgSessions.ToListAsync();
        }

        public void Update(CyborgSession session)
        {
            _context.CyborgSessions.Update(session);
        }
    }
}
