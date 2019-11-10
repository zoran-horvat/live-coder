﻿using System;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation.Commands
{
    class ActivateDocument : IDemoCommand
    {
        private ISource Document { get; }

        public ActivateDocument(ISource document)
        {
            this.Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public void Execute() => this.Document.Activate();

        public override string ToString() => $"activate {this.Document}";
    }
}