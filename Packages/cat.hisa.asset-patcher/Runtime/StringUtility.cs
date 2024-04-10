using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace HisaCat.AssetPatcher.Extensions
{
    public static class StringExtension
    {
        public static string FormatNamed(this string input, object p)
        {
            var properties = System.ComponentModel.TypeDescriptor.GetProperties(p);
            int count = properties.Count;
            for (int i = 0; i < count; i++)
            {
                var prop = properties[i];
                var pattern = @"({" + System.Text.RegularExpressions.Regex.Escape(prop.Name) + @")+(:+.*?}|})";
                var matches = System.Text.RegularExpressions.Regex.Matches(input, pattern);
                var matchesCount = matches.Count;
                for (int j = 0; j < matchesCount; j++)
                {
                    var match = matches[j];
                    var capture = match.Captures.Count <= 0 ? null : match.Captures[0];
                    if (capture == null) continue;

                    var format = capture.Value.Replace(prop.Name, "0");
                    string result;

                    result = string.Format(format, prop.GetValue(p));

                    input = input.Replace(capture.Value, result);
                }
            }

            return input;
        }
    }
}
