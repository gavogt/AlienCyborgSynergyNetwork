using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienCyborgSynergyNetwork.Shared;

namespace FirmwareDistributionService
{
    public class PushCommand : ICommand
    {
        public async Task ExecuteAsync(FirmwareJob job, IProgress<int> progress)
        {
            await Task.Delay(500);
            progress.Report(75);
            await Task.Delay(500);
            progress.Report(100);
        }
    }
}
