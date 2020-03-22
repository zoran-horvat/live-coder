﻿using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Interfaces
{
    interface IStateVerifier : IDemoCommand
    {
        bool IsStateAsExpected { get; }
        string PrintableReport { get; }
    }
}
