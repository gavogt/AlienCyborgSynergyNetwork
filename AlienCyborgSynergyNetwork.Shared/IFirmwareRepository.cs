using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public interface IFirmwareRepository
    {
        Task<FirmwareImage?> GetLastestAsync();
        Task AddAsync(FirmwareImage image);
    }
}
