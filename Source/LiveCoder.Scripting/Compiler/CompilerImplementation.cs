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

        public IStatement SayStatement(string say, string openParen, string closeParen) =>
            new Say();
    }
}