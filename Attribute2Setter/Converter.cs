using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Attribute2Setter
{
    class Converter
    {
        enum MatchType { Attribute, Setter, Invalid };
        static Regex oneAttribute = new Regex(@"([^=\s]+)=(?:""([^""]+)""|''([^'']+)'')");
        static Regex allAttributes = new Regex(@"(?:[^\s]+?=([""''])[^\r\n\1]+?\1[\s]*)+");

        static Regex oneSetter = new Regex(@"<Setter[\s]*Property=(?:""([^""]+)""|''([^'']+)'')[\s]*Value=(?:""([^""]+)""|''([^'']+)'')[\s]*/>");
        static Regex allSetters = new Regex(@"(?:<Setter[\s]*Property=(?:""([^""]+)""|''([^'']+)'')[\s]*Value=(?:""([^""]+)""|''([^'']+)'')[\s]*/>[\s]*)+");

        public static string Convert(string input)
        {
            MatchType type = CheckType(input);
            switch (type)
            {
                case MatchType.Attribute:
                    return ConvertToSetters(input);
                case MatchType.Setter:
                    return ConvertToAttributes(input);
                default:
                    return null;
            }
        }
        static MatchType CheckType(string input)
        {
            if (allSetters.IsMatch(input))
                return MatchType.Setter;
            else if (allAttributes.IsMatch(input))
                return MatchType.Attribute;
            else
                return MatchType.Invalid;
        }
        static string ConvertToSetters(string attributes)
        {
            string setters = "";
            MatchCollection matches = oneAttribute.Matches(attributes);
            for (int i = 0; i < matches.Count; i++)
            {
                setters += "<Setter Property=\"" + matches[i].Groups[1].Value + "\" Value=\"" + matches[i].Groups[2].Value + "\"/>";
                int[] k = allSetters.GetGroupNumbers();
                if (i != matches.Count - 1)
                    setters += "\n";
            }
            return setters;
        }
        static string ConvertToAttributes(string setters)
        {
            string attributes = "";
            MatchCollection matches = oneSetter.Matches(setters);
            for (int i = 0; i < matches.Count; i++)
            {
                attributes += matches[i].Groups[1] + "=\"" + matches[i].Groups[3] + "\"";
                if (i != matches.Count - 1)
                    attributes += " ";
            }
            return attributes;
        }
    }
}
