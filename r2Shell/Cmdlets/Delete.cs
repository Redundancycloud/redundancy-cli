using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Delete : ICmdlet  
    {
        private Shell shellInstance;
        public Delete(Shell s)
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
            string file = new Regex(@"rm\s{0,1}(?<file>.*)", RegexOptions.IgnoreCase).Match(arguments).Groups["file"].Value;
            FileSystemItem current = Instance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { file, Instance.token });
            Instance.requests++;
            bool result;
            if (current.FilePath == null)
            {
                result = Instance.lib.Request<bool>("Kernel.FileSystemKernel", "DeleteDirectory", new string[] { file, Instance.token });
            }
            else
            {
                result = Instance.lib.Request<bool>("Kernel.FileSystemKernel", "DeleteFile", new string[] { file, Instance.token });
            }
            return result ? 0 : 1;

        }
        public void PrintHelp()
        {
            Console.WriteLine("Deletes an filesystem entry.");
        }
    }
}
