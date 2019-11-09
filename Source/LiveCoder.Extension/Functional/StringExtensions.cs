using System;

namespace LiveCoder.Extension.Functional
{
    static class StringExtensions
    {
        public static string WithNormalizedNewLines(this string value) =>
            string.Join(Environment.NewLine, value.Split(new string[] {"\r\n", "\r", "\n"}, StringSplitOptions.None));

        public static string WithPrintableNewLines(this string value) =>
            value.Replace("\r", "\\r").Replace("\n", "\\n");
    }
}
