using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Download : ICmdlet
    {
        private Shell shellInstance;
        public Download(Shell s)
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
            string file = new Regex(@"download\s{0,1}(?<file>.*)", RegexOptions.IgnoreCase).Match(arguments).Groups["file"].Value;
            FileSystemItem current = Instance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { file, Instance.token });
            Instance.requests++;
            Console.Write("Downloading {0}...", current.DisplayName);
            Instance.lib.Download(current.Hash, Instance.token, current.DisplayName);
            Console.WriteLine("Done");
            System.Diagnostics.Process.Start(current.DisplayName);
            return 0;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Downloads a file and open it with the associated application");
        }
    }
}
