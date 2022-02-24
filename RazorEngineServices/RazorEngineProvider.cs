using RazorEngine.Compilation;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RazorEngineServices
{
    public class RazorEngineProvider
    {
        private readonly static string _conventionFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private readonly IRazorEngineService _razorEngineService;

        public RazorEngineProvider()
        {
            var templateConfig = new TemplateServiceConfiguration
            {
                TemplateManager = new DelegateTemplateManager(controllerAndAction =>
                File.ReadAllText(Path.Combine(_conventionFolderPath, controllerAndAction))),
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
                ReferenceResolver = new MyIReferenceResolver(),

            };
            _razorEngineService = RazorEngineService.Create(templateConfig);
        }

        //public string GetHTMLFromControllerAction(string controller, string action, object model)
        //{
        //    //C# 6
        //    var actionController = $"{controller}/{action}.cshtml";
        //    //var actionController = string.Format("{0}/{1}.cshtml",controller,action);
        //    return _razorEngineService.RunCompile(actionController, model.GetType(), model); //
        //}

        public static string RenderViewToString(string controller, string action, object model = null)
        {
            if (model == null)
                model = new object();

            RazorEngineProvider p = new RazorEngineProvider();
            //C# 6
            var actionController = $"{controller}/{action}.cshtml";
            //var actionController = string.Format("{0}/{1}.cshtml",controller,action);
            return p._razorEngineService.RunCompile(actionController, model.GetType(), model); //
        }


        class MyIReferenceResolver : IReferenceResolver
        {
            public string FindLoaded(IEnumerable<string> refs, string find)
            {
                try
                {
                    return refs.FirstOrDefault(r => r.EndsWith(System.IO.Path.DirectorySeparatorChar + find));
                }
                catch
                {
                    return default(string);
                }
            }
            public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies)
            {
                // TypeContext gives you some context for the compilation (which templates, which namespaces and types)

                // You must make sure to include all libraries that are required!
                // Mono compiler does add more standard references than csc! 
                // If you want mono compatibility include ALL references here, including mscorlib!
                // If you include mscorlib here the compiler is called with /nostdlib.
                IEnumerable<string> loadedAssemblies = (new UseCurrentAssembliesReferenceResolver())
                    .GetReferences(context, includeAssemblies)
                    .Select(r => r.GetFile())
                    .ToArray();

                List<CompilerReference> re= new List<CompilerReference>();
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "mscorlib.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "System.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "System.Core.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "RazorEngine.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "Microsoft.CSharp.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.Optimization.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.WebPages.dll")));
                re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.Mvc.dll")));


                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "mscorlib.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Core.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "RazorEngine.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "Microsoft.CSharp.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.Optimization.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.WebPages.dll"));
                //yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.Mvc.dll"));

                foreach (var item in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), "*.dll"))
                {
                    try
                    {
                            re.Add(CompilerReference.From(FindLoaded(loadedAssemblies, Path.GetFileName(item))));
                    }
                    catch
                    {
                    }
                }

                re.Add(CompilerReference.From(typeof(MyIReferenceResolver).Assembly));

                return re;

              //  yield return CompilerReference.From(typeof(MyIReferenceResolver).Assembly); // Assembly

                // There are several ways to load an assembly:
                //yield return CompilerReference.From("Path-to-my-custom-assembly"); // file path (string)
                //byte[] assemblyInByteArray = --- Load your assembly ---;
                //yield return CompilerReference.From(assemblyInByteArray); // byte array (roslyn only)
                //string assemblyFile = --- Get the path to the assembly ---;
                //yield return CompilerReference.From(File.OpenRead(assemblyFile)); // stream (roslyn only)
            }
        }
    }
}
