using Ajax.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ajax
{
    internal class ApiScriptsGenerator
    {
        public static string GetScriptLocation(string assemblyPath)
        {
            var binDir = Path.GetDirectoryName(assemblyPath);
            DirectoryInfo projDir = null;
            var pathForJs = "";
            if (binDir.EndsWith("bin"))
            {
                projDir = Directory.GetParent(binDir);
            }
            else
            {
                projDir = Directory.GetParent(binDir).Parent;
            }
            if (projDir == null)
            {
                pathForJs = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\js").FullName;
            }
            else
            {
                var jsDir = projDir.GetDirectories().SingleOrDefault(d => d.Name == "js");
                if (jsDir == null)
                {
                    pathForJs = projDir.CreateSubdirectory("js").FullName + "\\apis";
                }
                else
                {
                    pathForJs = jsDir.FullName + "\\apis";
                }

                Directory.CreateDirectory(pathForJs);
            }

            return pathForJs;
        }

        public static void GenerateScripts(string assemblyPath)
        {
            var apisAssem = Assembly.LoadFile(assemblyPath);

            Func<IEnumerable<Type>> getTypes = () => {
                try
                {
                    return apisAssem.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }

            };

            var types = getTypes().Where(p => p.IsSubclassOf(typeof(ApiController)));
            var pathForJs = GetScriptLocation(assemblyPath);
            foreach (var t in types)
            {
                var script = Scripts.GenerateForApi(t);
                var fileName = pathForJs + "\\" + t.Name + ".js";
                Console.WriteLine($"File has been generated '{fileName}'.");
                File.WriteAllText(fileName, script);
            }
        }
    }
}
