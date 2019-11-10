﻿using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Scripting.Parsing.Events
{
    class ErrorParsingLine : IEvent
    {
        public string Label =>
            $"Error parsing line #{this.CurrentText.LineIndex + 1}: {this.CurrentText.CurrentLine}";

        private NonEmptyText CurrentText { get; }

        public ErrorParsingLine(NonEmptyText currentText)
        {
            this.CurrentText = currentText;
        }
    }
}