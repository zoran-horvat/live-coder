using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiveCoderExtension.Implementation.Readers
{
    class FileReader : SourceReader
    {
        private FileInfo File { get; }

        public FileReader(FileInfo file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public override IEnumerable<(string line, int index)> ReadAllLines() =>
            System.IO.File.ReadAllLines(this.File.FullName).Select((line, index) => (line, index));
    }
}
