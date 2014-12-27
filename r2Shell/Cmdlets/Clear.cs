using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Clear : ICmdlet
    {
        private Shell shellInstance;
        public Clear(Shell s)
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
            Console.Clear();
            return 0;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Clear screen");
        }
    }
}
