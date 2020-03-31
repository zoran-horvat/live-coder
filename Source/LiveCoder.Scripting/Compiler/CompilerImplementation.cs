using System;
using System.Collections.Immutable;
using EasyParse.Parsing;
using LiveCoder.Scripting.Commands;

namespace LiveCoder.Scripting.Compiler
{
    internal class CompilerImplementation : MethodMapCompiler
    {
        public Script Script(ImmutableList<IStatement> statements) =>
            new Script(statements);

        public ImmutableList<IStatement> StatementList(IStatement statement, string semicolon) =>
            ImmutableList<IStatement>.Empty.Add(statement);

        public ImmutableList<IStatement> StatementList(ImmutableList<IStatement> list, IStatement statement, string semicolon) =>
            list.Add(statement);

        public IStatement Statement(IStatement statement) => 
            statement;

        public string String(string value) => 
            value;

        public string PlainString(string quoted) =>
            quoted.Substring(1, quoted.Length - 2);

        public IStatement SayStatement(string say, string openParen, string message, string closeParen) =>
            new Say(message);
    }
}