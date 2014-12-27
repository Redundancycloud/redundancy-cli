using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using libRedundancy;
using libRedundancy.Models;
using System.IO;
using System.Net;
using r2Shell.Cmdlets;

namespace r2Shell
{
    class Shell
    {
        public string CurrentDir { get; set; }
        public libRedundancy.libRedundancy lib;
        public string token;
        public int requests;
        private Dictionary<string, string> commands;
        public Shell()
        {
            CurrentDir = "~";
            token = string.Empty;
            commands = new Dictionary<string, string>();
            commands.Add("auth", "r2Shell.Cmdlets.Authentification");
            commands.Add("cd", "r2Shell.Cmdlets.ChangeDirectory");
            commands.Add("rm", "r2Shell.Cmdlets.Delete");
            commands.Add("download", "r2Shell.Cmdlets.Download");
            commands.Add("ls", "r2Shell.Cmdlets.ListFiles");
            commands.Add("upload", "r2Shell.Cmdlets.Upload");
            commands.Add("info", "r2Shell.Cmdlets.Info");
            commands.Add("clear", "r2Shell.Cmdlets.Clear");
        }
        public string GetLineBegin()
        {
            string hostname = System.Environment.MachineName;
            string user = System.Environment.UserName;
            return string.Format("{0}@{1}:{2} ", user, hostname, (CurrentDir == "/") ? "~" : CurrentDir);
        }
        private bool Processing(string arguments)
        {          
            foreach (KeyValuePair<string, string> kvp in commands)
            {
                if (new Regex(kvp.Key, RegexOptions.IgnoreCase).IsMatch(arguments))
                {
                    try
                    {
                        Type t = Type.GetType(kvp.Value);
                        ICmdlet x = (ICmdlet)Activator.CreateInstance(t, new object[] { this });
                        return x.run(arguments) == 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Got an Error: {0}", ex.Message);
                        if (ex.GetType() == typeof(NullReferenceException))
                        {
                            Console.WriteLine("Did you authentificate?");
                        }
                        return false;
                    }
                }
            }
            Console.WriteLine("{0}: Command not found", arguments);
            return false;
        }
        public void Help()
        {
            foreach (KeyValuePair<string, string> kvp in commands)
            {
               
                Console.Write("{0} - ", kvp.Key);
                Type t = Type.GetType(kvp.Value);
                ICmdlet x = (ICmdlet)Activator.CreateInstance(t, new object[] { this });
                x.PrintHelp();                                              
            }
            Console.WriteLine("help - Print out this text");
        }
        public bool Loop()
        {
            Console.Write(this.GetLineBegin());
            string cmd = Console.ReadLine();
            if (cmd == "help")
                this.Help();
            else
                this.Processing(cmd);
            
            if (cmd != "exit")
                return true;
            else
                return false;
        }
    }
}
