using System;
using System.Linq;
using System.Reflection;
using ThirdPartyTool;

namespace CombineDlls
{
    class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static void Main(string[] args)
        {
            Util.SayHelloTo("Peter");
            Console.ReadLine();
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var requiredDllName = $"{(new AssemblyName(args.Name).Name)}.dll";
            var resource = currentAssembly.GetManifestResourceNames().Where(s => s.EndsWith(requiredDllName)).FirstOrDefault();

            if (resource != null)
            {
                using (var stream = currentAssembly.GetManifestResourceStream(resource))
                {
                    if (stream == null)
                        return null;

                    var block = new byte[stream.Length];
                    stream.Read(block, 0, block.Length);
                    return Assembly.Load(block);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
