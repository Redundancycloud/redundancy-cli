using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Info : ICmdlet
    {
        private Shell shellInstance;
        public Info(Shell s)
        {
            this.shellInstance = s;
        }
        public Shell Instance
        {
            get
            {
                return shellInstance;
            }
            set
            {
                shellInstance = value;
            }
        }
        public int run(string arguments)
        {
            Console.WriteLine("Server: {0}", Instance.lib.Target);
            string version = Instance.lib.Request<string>("Kernel", "GetVersion", new string[] { });
            Instance.requests++;
            Console.WriteLine("Server version: {0}", version);
            Console.WriteLine("Last error code: {0}", Instance.lib.LastErrorCode);
            Console.WriteLine("Request count until now: {0}", Instance.requests);
            Console.WriteLine("Ping: {0}", new Ping().Send(Instance.lib.Target.Host).RoundtripTime);
            
            return 0;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Print an info about the currently connected server.");
        }
    }
}
