using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetricsGenerator
{
    public static class AttributeValueHelper
    {
        public static AttributeData GetAttr(this PropertyDeclarationSyntax p, string name, SemanticModel semanticModel)
        {
            var attrData = semanticModel.GetDeclaredSymbol(p).GetAttributes().ToList();
            var result = attrData.FirstOrDefault(a => a.AttributeClass.Name == name || a.AttributeClass.Name == $"{name}Attribute");

            return result;
        }

        public static string GetParameterValue(this AttributeData attr, string paramName)
        {
            var param = attr.AttributeConstructor.Parameters.FirstOrDefault(p => p.Name == paramName);
            var index = attr.AttributeConstructor.Parameters.IndexOf(param);
            if (index == -1) return null;
            TypedConstant argValue = attr.ConstructorArguments[index];
            if(argValue.Kind == TypedConstantKind.Primitive)
            { 
                return argValue.Value.ToString();
            }
            if (argValue.Kind == TypedConstantKind.Array) { 
                var values = argValue.Values.Select(v => v.Value.ToString());
                var result = values.Aggregate(new StringBuilder(), 
                                            (sb, s) => sb.Append('"').Append(s).Append('"').Append(", "), 
                                            (sb)=> { 
                                                sb.Length = sb.Length > 0 ? sb.Length -= 2 : sb.Length; // chop off last comma
                                                return sb.ToString();
                                            });
                return result;
            }
            return null;
        }

        public static int HasParameterValues(this AttributeData attr, string paramName)
        {
            var param = attr.AttributeConstructor.Parameters.FirstOrDefault(p => p.Name == paramName);
            var index = attr.AttributeConstructor.Parameters.IndexOf(param);
            if (index == -1) return 0;
            TypedConstant argValue = attr.ConstructorArguments[index];
            if(argValue.Kind == TypedConstantKind.Array)
                return argValue.Values.Count();
            return 0;
        }

        public static AttributeSyntax GetAttr(this ClassDeclarationSyntax p, string name)
        {
            return
                p.AttributeLists.SelectMany(al => al.Attributes)
                    .FirstOrDefault(a => (a.Name as IdentifierNameSyntax)?.Identifier.ValueText == name);
        }

        public static string GetFirstParameterValue(this AttributeSyntax attr)
        {
            return (attr.ArgumentList.Arguments.First().Expression as LiteralExpressionSyntax).Token
                .ValueText;
        }

        public static string GetParameterValue(this AttributeSyntax attr, string parameterName)
        {
            var arguments = attr.ArgumentList.Arguments.ToList();
            return (arguments.First().Expression as LiteralExpressionSyntax).Token
                .ValueText;
        }

        public static string GetTailParameterValues(this AttributeSyntax attr)
        {
            var result = attr.ArgumentList.Arguments.Skip(1)
                .Select(e => e.Expression)
                .Cast<LiteralExpressionSyntax>()
                .Aggregate(new StringBuilder(), (sb, e) => sb.Append("\"").Append(e.Token.ValueText).Append("\", "));

            if (result.Length > 1) result.Length = result.Length - 2; // cut trailing comma
            return result.ToString();
        }
    }
}
