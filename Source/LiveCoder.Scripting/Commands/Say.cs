using System;

namespace LiveCoder.Scripting.Commands
{
    public class Say : IStatement
    {
        public string Message { get; }
     
        public Say(string message)
        {
            this.Message = message;
        }
    }
}
