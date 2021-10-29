using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MetricsGenerator
{
    public class ModelDefinitionReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassesToProcess { get; set; }

        public ModelDefinitionReceiver()
        {
            ClassesToProcess = new List<ClassDeclarationSyntax>();
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds && cds.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a => (a.Name as IdentifierNameSyntax)?.Identifier.ValueText == "AddInstrumentation"))
            {
                ClassesToProcess.Add(cds);
            }
        }
    }

    [Generator]
    public class MetricsGenerator: ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ModelDefinitionReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (ModelDefinitionReceiver)context.SyntaxReceiver;
            foreach (var classToProcess in syntaxReceiver.ClassesToProcess)
            {
                var namespaceDeclaration = classToProcess.Parent as NamespaceDeclarationSyntax;
                var fileDeclaration = namespaceDeclaration.Parent as CompilationUnitSyntax;
                var usings = namespaceDeclaration.Usings.Union(fileDeclaration.Usings).Select(u => u.ToString()); // collect all usings. dont bother filtering for now

                //Debugger.Launch();
                var text = usings.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s));
                text.AppendLine()
                    .AppendLine("using System; ")
                    .AppendLine("using System.Collections.Generic;")
                    .AppendLine("using Prometheus; ")
                    .AppendLine();

                text.AppendLine().AppendLine($"namespace {(classToProcess.Parent as NamespaceDeclarationSyntax).Name} {{ public partial class {classToProcess.Identifier} {{").AppendLine();

                
                var properties = classToProcess.Members.Where(m => m.IsKind(SyntaxKind.PropertyDeclaration)
                                                                   && m.Modifiers.Any(mm => mm.ValueText == "public")).ToList();

                var attr = classToProcess.GetAttr("AddInstrumentation");
                var attrPrefix = "";
                if (attr.ArgumentList != null && attr.ArgumentList.Arguments.Any())
                {
                    attrPrefix = (attr.ArgumentList.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText;
                }
                
                if (!string.IsNullOrWhiteSpace(attrPrefix)) attrPrefix = $"_{attrPrefix}";

                text.AppendLine(GetMetrics(properties, attrPrefix).ToString());

                text.AppendLine(UpdateMetrics(properties, classToProcess.Identifier, attrPrefix).ToString());

                text.AppendLine("}}");
                context.AddSource($"{classToProcess.Identifier}.Metrics.cs", SourceText.From(text.ToString(), Encoding.UTF8));

            }
        }

        private StringBuilder GetMetrics(List<MemberDeclarationSyntax> properties, string attrPrefix)
        {
            var commonLabels = "\"host\", \"slot\", \"algo\"";

            var text = new StringBuilder(@"public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {").AppendLine();
            foreach (PropertyDeclarationSyntax p in properties)
            {
                var jsonPropertyAttr = p.GetAttr("JsonProperty");
                var metricAttr = p.GetAttr("Metric");

                if (metricAttr == null) continue;

                var propName = jsonPropertyAttr.GetFirstParameterValue();
                var metricName = metricAttr.GetFirstParameterValue();
                if (metricAttr.ArgumentList.Arguments.Count > 1)
                {
                    var labels = metricAttr.GetTailParameterValues();
                    text.AppendLine(
                        $"{{$\"{{prefix}}{attrPrefix}_{propName}\", Metrics.Create{metricName}($\"{{prefix}}{attrPrefix}_{propName}\", \"{propName}\", {commonLabels}, {labels}) }},");

                }
                else
                {
                    text.AppendLine(
                        $"{{$\"{{prefix}}{attrPrefix}_{propName}\", Metrics.Create{metricName}($\"{{prefix}}{attrPrefix}_{propName}\", \"{propName}\", {commonLabels}) }},");
                }
            }
            text.AppendLine(@"};
                            return result;
                        }");

            return text;
        }

        private StringBuilder UpdateMetrics(List<MemberDeclarationSyntax> properties, SyntaxToken classToProcess, string attrPrefix)
        {
            var text = new StringBuilder($"public static void UpdateMetrics(string prefix, Dictionary<string, Collector> metrics, {classToProcess} data, string host, string slot, string algo, List<string> extraLabels = null) {{");
            text.AppendLine();
            text.AppendLine(@"if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo);
                                    extraLabels.Insert(0, slot);
                                    extraLabels.Insert(0, host);
                                }");

            foreach (PropertyDeclarationSyntax p in properties)
            {
                var jsonPropertyAttr = p.GetAttr("JsonProperty");
                var metricAttr = p.GetAttr("Metric");

                if (metricAttr == null) continue;

                var propName = jsonPropertyAttr.GetFirstParameterValue();
                var metricName = metricAttr.GetFirstParameterValue();
                var newValue = $"data.{p.Identifier.ValueText}";

                text.Append(
                    $"(metrics[$\"{{prefix}}{attrPrefix}_{propName}\"] as {metricName}).WithLabels(extraLabels.ToArray())");
                switch (metricName)
                {
                    case "Counter": text.AppendLine($".IncTo({newValue});"); break;
                    case "Gauge": text.AppendLine($".Set({newValue});"); break;
                }
            }

            text.AppendLine("}").AppendLine();
            return text;
        }
    }
}
