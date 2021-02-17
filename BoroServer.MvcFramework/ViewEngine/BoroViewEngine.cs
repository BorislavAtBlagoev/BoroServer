namespace BoroServer.MvcFramework.ViewEngine
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.CSharp;

    public class BoroViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel, string user)
        {
            string csharpCode = GenerateCSharpFromTemplate(templateCode, viewModel);
            IView executableObject = GenerateExecutableCode(csharpCode, viewModel);
            string html = executableObject.ExecuteTemplate(viewModel, user);
            html = html.Remove(html.Length - 2);
            return html;
        }

        private IView GenerateExecutableCode(string csharpCode, object viewModel)
        {
            var compileResult = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));

            if (viewModel != null)
            {
                if (viewModel.GetType().IsGenericType)
                {
                    var genericArguments = viewModel.GetType()
                        .GenericTypeArguments;

                    foreach (var genericArgument in genericArguments)
                    {
                        compileResult = compileResult
                            .AddReferences(MetadataReference.CreateFromFile(genericArgument.Assembly.Location));
                    }
                }

                compileResult = compileResult
                    .AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compileResult = compileResult
                    .AddReferences(MetadataReference.CreateFromFile(
                        Assembly.Load(library).Location));
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult result = compileResult.Emit(stream);

                if (!result.Success)
                {
                    return new ErrorView(result.Diagnostics
                        .Where(x => x.Severity == DiagnosticSeverity.Error)
                        .Select(x => x.GetMessage()), csharpCode);
                }

                try
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var byteAssembly = stream.ToArray();
                    var assembly = Assembly.Load(byteAssembly);
                    var viewType = assembly.GetType("ViewNamespace.ViewClass");
                    var instance = Activator.CreateInstance(viewType);

                    return instance as IView;
                }
                catch (Exception ex)
                {

                    return new ErrorView(new List<string> { ex.ToString() }, csharpCode);
                }


            }
        }

        private string GenerateCSharpFromTemplate(string templateCode, object viewModel)
        {
            var typeOfModel = "object";
            if (viewModel != null)
            {
                if (viewModel.GetType().IsGenericType)
                {
                    var modelName = viewModel.GetType().FullName;
                    var genericArguments = viewModel.GetType().GenericTypeArguments;
                    typeOfModel = modelName.Substring(0, modelName.IndexOf('`')) +
                        "<" + string.Join(",", genericArguments.Select(x => x.FullName)) + ">";
                }
                else
                {
                    typeOfModel = viewModel.GetType().FullName;
                }
            }
            string csharpCode = @"
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using BoroServer.MvcFramework.ViewEngine;

namespace ViewNamespace
{
public class ViewClass : IView
{
public string ExecuteTemplate(object viewModel, string user)
{
var User = user;
var Model = viewModel as " + typeOfModel + @" ;
var html = new StringBuilder();
" + GetMethodBody(templateCode) + @"
return html.ToString();
}
}
}";

            return csharpCode;
        }

        private string GetMethodBody(string templateCode)
        {
            StringBuilder sb = new StringBuilder();
            var templateCodeLines = templateCode.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Regex csharpCodeRegex = new Regex(@"[^\""\s&\'<]+");
            //StreamReader sr = new StreamReader(templateCode);
            //string line;

            for (int i = 0; i < templateCodeLines.Length; i++)
            {
                if (templateCodeLines[i].TrimStart().StartsWith("@"))
                {
                    var atSignIndex = templateCodeLines[i].IndexOf("@");
                    templateCodeLines[i] = templateCodeLines[i].Remove(atSignIndex, 1);
                    sb.AppendLine(templateCodeLines[i]);
                }
                else if (templateCodeLines[i].TrimStart().StartsWith("{") ||
                         templateCodeLines[i].TrimStart().StartsWith("}"))
                {
                    sb.AppendLine(templateCodeLines[i]);
                }
                else if (templateCodeLines[i].Contains("@"))
                {
                    sb.Append($"html.AppendLine(@\"");

                    while (templateCodeLines[i].Contains("@"))
                    {
                        var atSignLocation = templateCodeLines[i].IndexOf("@");
                        var lineBeforeAtSign = templateCodeLines[i].Substring(0, atSignLocation);
                        sb.Append(lineBeforeAtSign.Replace("\"", "\"\"") + "\" + ");
                        var lineAfterAtSign = templateCodeLines[i].Substring(atSignLocation + 1);
                        var code = csharpCodeRegex.Match(lineAfterAtSign).Value;
                        sb.Append(code + " + @\"");
                        templateCodeLines[i] = lineAfterAtSign.Substring(code.Length);
                    }

                    sb.AppendLine(templateCodeLines[i].Replace("\"", "\"\"") + "\");");
                }
                else
                {
                    if (templateCode != "")
                    {
                        sb.AppendLine($"html.AppendLine(@\"{templateCodeLines[i].Replace("\"", "\"\"")}\");");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
