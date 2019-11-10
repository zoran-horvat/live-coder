﻿using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;

namespace LiveCoder.Extension.Scripting.Parsing
{
    interface IPattern
    {
        Regex StartsWith { get; }
        Option<(IText remaining, DemoScript script)> Apply(NonEmptyText current, DemoScript script);
    }
}