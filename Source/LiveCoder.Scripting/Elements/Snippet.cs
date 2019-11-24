namespace LiveCoder.Scripting.Elements
{
    public class Snippet
    {
        public int Number { get; }
        public string Content { get; }

        public Snippet(int number, string content)
        {
            this.Number = number;
            this.Content = content;
        }
    }
}
