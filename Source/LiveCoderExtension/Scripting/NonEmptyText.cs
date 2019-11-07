using System.IO;
using System.Text;
using Common.Optional;

namespace LiveCoderExtension.Scripting
{
    class NonEmptyText : IText
    {
        private string[] Content { get; }
        public int LineIndex { get; }
        public string CurrentLine => this.Content[this.LineIndex];

        private NonEmptyText(string[] content) : this(content, 0) { }

        private NonEmptyText(string[] content, int lineIndex)
        {
            this.Content = content;
            this.LineIndex = lineIndex;
        }

        public static Option<NonEmptyText> Load(FileInfo source) =>
            File.ReadAllLines(source.FullName, Encoding.UTF8) is string[] array && array.Length > 0
                ? Option.Of(new NonEmptyText(array))
                : None.Value;

        public IText ConsumeLine() =>
            this.LineIndex < this.Content.Length - 1
                ? (IText)new NonEmptyText(this.Content, this.LineIndex + 1)
                : new EmptyText();
    }
}