using System.Collections.Generic;
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

                //if(!Debugger.IsAttached) Debugger.Launch();
                var text = usings.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s));
                text.AppendLine()
                    .AppendLine("using System; ")
                    .AppendLine("using System.Collections.Generic;")
                    .AppendLine("using Prometheus; ")
                    .AppendLine();

                var ns = (classToProcess.Parent as NamespaceDeclarationSyntax).Name;
                text.AppendLine().AppendLine($"namespace {ns} {{ public partial class {classToProcess.Identifier} {{").AppendLine();

                
                var properties = classToProcess.Members.Where(m => m.IsKind(SyntaxKind.PropertyDeclaration)
                                                                   && m.Modifiers.Any(mm => mm.ValueText == "public")).ToList();

                var attr = classToProcess.GetAttr("AddInstrumentation");
                var attrPrefix = "";
                if (attr.ArgumentList != null && attr.ArgumentList.Arguments.Any())
                {
                    attrPrefix = (attr.ArgumentList.Arguments.FirstOrDefault()?.Expression as LiteralExpressionSyntax)?.Token.ValueText;
                }
                
                if (!string.IsNullOrWhiteSpace(attrPrefix)) attrPrefix = $"_{attrPrefix}";

                text.AppendLine(GetMetrics(properties, attrPrefix, context.Compilation).ToString());

                text.AppendLine(UpdateMetrics(properties, classToProcess.Identifier, attrPrefix, context.Compilation).ToString());

                text.AppendLine("}}");
                context.AddSource($"{ns}.{classToProcess.Identifier}.Metrics.cs", SourceText.From(text.ToString(), Encoding.UTF8));

            }
        }

        private StringBuilder GetMetrics(List<MemberDeclarationSyntax> properties, string attrPrefix, Compilation compilation)
        {
            var commonLabels = "\"host\", \"slot\", \"algo\"";

            var text = new StringBuilder(@"public static Dictionary<string, Collector> GetMetrics(string prefix)
                {
                    var result = new Dictionary<string, Collector>
                    {").AppendLine();
            foreach (PropertyDeclarationSyntax p in properties)
            {
                var semanticModel = compilation.GetSemanticModel(p.SyntaxTree);
                var jsonPropertyAttr = p.GetAttr("JsonProperty", semanticModel);
                var metricAttr = p.GetAttr("Metric", semanticModel);

                if (metricAttr == null) continue;

                var metricType = metricAttr.GetParameterValue("type");
                var propName = metricAttr.GetParameterValue("metricName") ??
                    jsonPropertyAttr.GetParameterValue("propertyName");

                if (metricAttr.HasParameterValues("labels") > 0)
                {
                    var labels = metricAttr.GetParameterValue("labels");
                    text.AppendLine(
                        $"{{$\"{{prefix}}{attrPrefix}_{propName}\", Metrics.Create{metricType}($\"{{prefix}}{attrPrefix}_{propName}\", \"{propName}\", {commonLabels}, {labels}) }},");

                }
                else
                {
                    text.AppendLine(
                        $"{{$\"{{prefix}}{attrPrefix}_{propName}\", Metrics.Create{metricType}($\"{{prefix}}{attrPrefix}_{propName}\", \"{propName}\", {commonLabels}) }},");
                }
            }
            text.AppendLine(@"};
                            return result;
                        }");

            return text;
        }

        private StringBuilder UpdateMetrics(List<MemberDeclarationSyntax> properties, SyntaxToken classToProcess, string attrPrefix, Compilation compilation)
        {
            var text = new StringBuilder($"public static void UpdateMetrics(string prefix, MetricCollection metrics, {classToProcess} data, string host, string slot, string algo, List<string> extraLabels = null) {{");
            text.AppendLine();
            text.AppendLine(@"if(extraLabels == null) { 
                                    extraLabels = new List<string> {host, slot, algo};
                                }
                                else {
                                    extraLabels.Insert(0, algo.ToLowerInvariant());
                                    extraLabels.Insert(0, slot.ToLowerInvariant());
                                    extraLabels.Insert(0, host.ToLowerInvariant());
                                }");

            foreach (PropertyDeclarationSyntax p in properties)
            {
                var semanticModel = compilation.GetSemanticModel(p.SyntaxTree);

                var jsonPropertyAttr = p.GetAttr("JsonProperty", semanticModel);
                var metricAttr = p.GetAttr("Metric", semanticModel);

                if (metricAttr == null) continue;

                var metricType = metricAttr.GetParameterValue("type");
                var propName = metricAttr.GetParameterValue("metricName") ?? jsonPropertyAttr.GetParameterValue("propertyName");
                var newValue = metricAttr.GetParameterValue("valuePath") != null ? 
                    $"data.{metricAttr.GetParameterValue("valuePath")}" 
                    : $"data.{p.Identifier.ValueText}";

                text.Append(
                    $"(metrics[$\"{{prefix}}{attrPrefix}_{propName}\"] as {metricType}).WithLabels(extraLabels.ToArray())");
                switch (metricType)
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
