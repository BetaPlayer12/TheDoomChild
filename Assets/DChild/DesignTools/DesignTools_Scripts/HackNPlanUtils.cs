using System.Linq;
using System.Text;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    public static class HackNPlanUtils
    {
        public static string BoldenString(string text) => $"**{text}**";
        public static void BoldenString(StringBuilder builder)
        {
            builder.Insert(0, "**");
            builder.Append("**");
        }

        public static string EmphasizeGuideIDs(string element) => $"[**{element}**]";

        public static void EmphasizeGuideIDs(StringBuilder builder)
        {
            builder.Insert(0, "[**");
            builder.Append("**]");
        }

        public static void EmphasizeGuideIDs(StringBuilder builder, LevelMapElement[] elements)
        {
            var elementNames = elements.Select(x => x.elementName).ToArray();
            for (int i = 0; i < elementNames.Length; i++)
            {
                builder.Append(elementNames[i]);
                if (i + 1 < elements.Length)
                {
                    builder.Append(",");
                }
            }
        }

        public static string EmphasizeGuideIDs(params LevelMapElement[] elements)
        {
            StringBuilder builder = new StringBuilder();
            var elementNames = elements.Select(x => x.elementName).ToArray();
            for (int i = 0; i < elementNames.Length; i++)
            {
                builder.Append(elementNames[i]);
                if (i + 1 < elements.Length)
                {
                    builder.Append(",");
                }
            }
            EmphasizeGuideIDs(builder);
            return builder.ToString();
        }

        public static string ConvertToHeader(string text)
        {
            return $"### {BoldenString(text)}:\n";
        }

        public static string GetDesignLink(string designElement) => "NULL";
    }
}