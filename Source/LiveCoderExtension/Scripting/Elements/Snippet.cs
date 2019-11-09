namespace LiveCoderExtension.Scripting.Elements
{
    class Snippet
    {
        private int Number { get; }
        private string Content { get; }

        public Snippet(int number, string content)
        {
            this.Number = number;
            this.Content = content;
        }
    }
}
