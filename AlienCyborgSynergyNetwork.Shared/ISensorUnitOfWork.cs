﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public interface ISensorUnitOfWork : IDisposable
    {
       ISensorRepository Sensors { get; }
        Task<int> SaveChangesAsync();
    }
}
