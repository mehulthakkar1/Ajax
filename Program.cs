using Ajax.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Ajax
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please pass the web api project assembly path.");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Web api project assembly does not exist.");
                return;
            }
            
            ApiScriptsGenerator.GenerateScripts(args[0]);
        }

            
    }
}
