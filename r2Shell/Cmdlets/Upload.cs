using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Upload : ICmdlet
    {
        private Shell shellInstance;
        public Upload(Shell s)
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
            string file = new Regex(@"upload\s{0,1}(?<file>.*)", RegexOptions.IgnoreCase).Match(arguments).Groups["file"].Value;
            if (File.Exists(file))
            {

                if (Instance.CurrentDir == "/")
                {
                    bool upload = Instance.lib.UploadFile(-1, Instance.token, file);
                    return upload ? 0 : 1;
                }
                else
                {
                    FileSystemItem current = Instance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { Instance.CurrentDir, Instance.token });
                    Instance.requests++;
                    bool upload = Instance.lib.UploadFile(current.ID, Instance.token, file);
                    return upload ? 0 : 1;
                }
            }
            return 0;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Uploads files.");
        }
    }
}
