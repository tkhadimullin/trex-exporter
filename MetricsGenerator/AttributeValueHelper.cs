using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetricsGenerator
{
    public static class AttributeValueHelper
    {
        public static AttributeSyntax GetAttr(this PropertyDeclarationSyntax p, string name)
        {
            return
                p.AttributeLists.SelectMany(al => al.Attributes)
                    .FirstOrDefault(a => (a.Name as IdentifierNameSyntax)?.Identifier.ValueText == name);
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
