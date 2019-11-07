using System.IO;
using System.Text;
using Common.Optional;

namespace LiveCoderExtension.Scripting
{
    class Text
    {
        private string[] Content { get; }
        public int LineIndex { get; }
        public string CurrentLine => this.Content[this.LineIndex];
        public bool Consumed => this.LineIndex >= this.Content.Length;

        private Text(string[] content)
        {
            this.Content = content;
            this.LineIndex = 0;
        }

        public static Option<Text> TryLoad(FileInfo source) =>
            File.ReadAllLines(source.FullName, Encoding.UTF8) is string[] array && array.Length > 0
                ? (Option<Text>)new Some<Text>(new Text(array))
                : None.Value;
    }
}
