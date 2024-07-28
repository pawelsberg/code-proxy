using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SubC.AllegroDotNet;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace CodeProxy.Routines;

public static class R
{
    public static string CodeSplitter = "R.Run();";
    [Obsolete("This method is not intended to be called directly. 'R.Run();' is recognised as a special token dividing runnable chunks of code.")]
    public static void Run() { } 
    public static bool ExitRequested = false;
    public static void Exit()
    {
        ExitRequested = true;
    }

    public static StringBuilder CodeStringBuilder = new ();
    public static void RunCode(string code)
    {

        string src = @"
            #nullable enable
            namespace CodeProxy
            {
                using SubC.AllegroDotNet;
                using SubC.AllegroDotNet.Models;
                using SubC.AllegroDotNet.Enums;
                using System;
                using System.Linq;
                using CodeProxy.Routines;
                public class DynamicClass
                {
                    public void DynamicMethod()
                    {
                        " + code + @"
                    }
                }
            }
            ";

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(src);

        string assemblyName = Path.GetRandomFileName();

#pragma warning disable CS8604 // Possible null reference argument for System.Runtime.dll
        string[] refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(System.Linq.Enumerable).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                typeof(Al).GetTypeInfo().Assembly.Location,
                typeof(R).GetTypeInfo().Assembly.Location
            };
#pragma warning restore CS8604 // Possible null reference argument for System.Runtime.dll
        MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: [syntaxTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


        using (MemoryStream ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine($"CODE_ERROR: {string.Join(";\n", result.Diagnostics)}");
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);

                Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                var type = assembly.GetType("CodeProxy.DynamicClass");
                if (type == null)
                {
                    Console.WriteLine("CODE_ERROR: Type not found");
                    return;
                }
                var instance = assembly.CreateInstance("CodeProxy.DynamicClass");
                var meth = type.GetMember("DynamicMethod").First() as MethodInfo;
                if (meth == null)
                {
                    Console.WriteLine("CODE_ERROR: Method not found");
                    return;
                }

                try
                {
                    meth.Invoke(instance, []);
                    Console.WriteLine("CODE_OK.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"RUNTIME_ERROR: {e.Message}");
                }
            }
        }
    }
    public static void MainLoop()
    {
        Task task = Task.Run(() =>
        {
            while (!ExitRequested)
            {
                string? line = Console.ReadLine();
                lock (R.CodeStringBuilder)
                {
                    R.CodeStringBuilder.AppendLine(line);
                }
            }
        });

        while (!ExitRequested)
        {
            string? code = null;
            lock (R.CodeStringBuilder)
            {
                string codeBuffered = R.CodeStringBuilder.ToString();
                string[] codes = codeBuffered.Split(CodeSplitter);
                if (codes.Length > 1)
                {
                    code = codes[0];
                    R.CodeStringBuilder.Clear();
                    R.CodeStringBuilder.Append(String.Join(CodeSplitter, codes.Skip(1)));
                }
            }
            if (code!= null)
            {
                R.RunCode(code);
            }
        }
    }
}