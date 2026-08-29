using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBToolbox.Utils;
using MVVMEssentials.Commands;

namespace EBToolbox.Commands.ConfigurationButtonsCommand
{
    public class InstallProcessExplorer : AsyncCommandBase
    {
        protected override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() => { ProcessHelper.StartShellExecute(@$"{Environment.GetEnvironmentVariable("windir")}\EBDesktop\6. Advanced Configuration\Process Explorer\Install Process Explorer.cmd"); });
        }
    }
}
