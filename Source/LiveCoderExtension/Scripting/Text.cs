using System.IO;
using System.Text;

namespace LiveCoderExtension.Scripting
{
    class Text
    {
        private string[] Content { get; }

        private Text(string[] content)
        {
            this.Content = content;
        }

        public static Text Load(FileInfo source) =>
            new Text(File.ReadAllLines(source.FullName, Encoding.UTF8));
    }
}
