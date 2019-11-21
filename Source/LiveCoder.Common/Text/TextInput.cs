using System;
using System.Collections.Generic;
using System.IO;

namespace LiveCoder.Common.Text
{
    public static class TextInput
    {
        public static IEnumerable<string> GetLines(this TextReader reader, Action prompt)
        {
            while (true)
            {
                prompt();
                yield return reader.ReadLine();
            }
        }
    }
}
