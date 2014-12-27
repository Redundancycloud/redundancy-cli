using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace r2Shell.Cmdlets
{
    class Authentification : ICmdlet
    {
        private Shell shellInstance;
        public Authentification(Shell s)
        {
            this.shellInstance = s;
        }
        public int run(string arguments)
        {
            Console.Write("Server: ");
            string server = Console.ReadLine();
            Console.Write("Username for {0}: ", server);
            string username = Console.ReadLine();
            Console.Write("Password for {0}@{1}: ", username, server);
            string password = ConsoleExtension.GetConsolePassword();
            shellInstance.lib = new libRedundancy.libRedundancy(new Uri(server));
            string got = shellInstance.lib.Request<string>("Kernel.UserKernel", "LogIn", new string[] { username, password, "true" });
            shellInstance.requests++;
            if (got != null)
            {
                Console.WriteLine("Authentification successfull :)!");
                Console.WriteLine("Got Token {0}", got);
                shellInstance.token = got;
                shellInstance.CurrentDir = "/";
                return 0;
            }
            else
            {
                return 1;
            }
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
        public void PrintHelp()
        {
            Console.WriteLine("Authentificates the user against the server.");
        }
    }
}
