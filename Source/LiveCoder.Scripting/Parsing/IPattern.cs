﻿using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, DemoScript script)> Apply(NonEmptyText current, DemoScript script);
    }
}