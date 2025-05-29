using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    internal interface ICyborgSessionRepository
    {
        Task AddAsync(CyborgSession session);
        Task<CyborgSession?> GetByIdAsync(Guid id);
        Task<List<CyborgSession>> ListAsync();
        void Update(CyborgSession session);
        void Delete(CyborgSession session);

    }
}
