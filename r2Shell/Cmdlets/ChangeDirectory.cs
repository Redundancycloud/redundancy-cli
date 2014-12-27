using libRedundancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class ChangeDirectory : ICmdlet
    {
        private Shell shellInstance;
        public ChangeDirectory(Shell s)
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
           string to = new Regex(@"cd\s{0,1}(?<directory>.*)", RegexOptions.IgnoreCase).Match(arguments).Groups["directory"].Value;
            bool isExisting = false;
            if (to != "..")
            {
                if (to.StartsWith("/") == false)
                    to = "/" + to;
                if (to.EndsWith("/") == false)
                    to = to + "/";
            }
            else
            {
                if (shellInstance.CurrentDir != "/")
                {
                    FileSystemItem current = shellInstance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { shellInstance.CurrentDir, shellInstance.token });
                    shellInstance.requests++;
                    if (current.ParentID == -1)
                        to = "/";
                    else
                    {
                        FileSystemItem parent = shellInstance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryById", new string[] { current.ParentID.ToString(), shellInstance.token });
                        shellInstance.requests++;
                        to = shellInstance.lib.Request<string>("Kernel.FileSystemKernel", "GetAbsolutePathById", new string[] { current.ParentID.ToString(), shellInstance.token });
                    }                  
                }
            }
            
            if (to == "/" || to == "~")
                isExisting = true;
            else
            {
                try
                {
                    var request = shellInstance.lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { to,shellInstance.token });
                    shellInstance.requests++;
                    if (request ==  null)
                       isExisting = false;
                    else
                       isExisting = true;
                }
                catch
                {
                    isExisting = false;
                }
            }
            if (!isExisting)
                Console.WriteLine("{0}: Directory not found",to);
            else
            {
                shellInstance.CurrentDir = to;
            }
            return isExisting ? 0 : 1;
        }
        public void PrintHelp()
        {
            Console.WriteLine("Changes the current directory.");
        }
    }
}
