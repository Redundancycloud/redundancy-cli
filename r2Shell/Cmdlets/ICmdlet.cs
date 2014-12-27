using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    interface ICmdlet
    {
        Shell Instance { get; set; }
        int run(string arguments);
        void PrintHelp();
    }
}
