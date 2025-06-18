using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class SignCommand : ICommand
    {
        public Task ExecuteAsync(FirmwareJob job, IProgress<int> progress)
        {
            progress.Report(25);
            return Task.CompletedTask;
        }
    }
}
