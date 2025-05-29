using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public interface IUnitOfWork : IDisposable
    {
        ICyborgSessionRepository Sessions { get; }

        /// <summary>
        /// Commits all ADD/UPDATE/DELETE in a single transaction.
        /// </summary>
        /// <returns>An int</returns>
        Task<int> SaveChangesAsync();
    }
}
