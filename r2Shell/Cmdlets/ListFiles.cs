using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class ListFiles : ICmdlet
    {
        private Shell shellInstance;
        public ListFiles(Shell s)
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
            List<FileSystemItem> entries = Instance.lib.Request<List<FileSystemItem>>("Kernel.FileSystemKernel", "GetContent", new string[] { Instance.CurrentDir, Instance.token });
            Instance.requests++;
            foreach (FileSystemItem entry in entries)
            {
                if (entry.FilePath == null)
                {
                    ConsoleColor old = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(entry.DisplayName);
                    Console.ForegroundColor = old;
                }
                else
                {
                    Console.WriteLine(entry.DisplayName);
                }

            }
            return 0;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Lists the files of the ucrrent directory");
        }
    }
}
