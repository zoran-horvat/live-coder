using System;
using System.Reflection;
using EasyParse.Parsing;
using LiveCoder.Common.Text.Documents;

namespace LiveCoder.Scripting.Compiler
{
    public static class ScriptCompiler
    {
        public static Script Compile(IText text) => 
            text is NonEmptyText nonEmpty ? Compile(nonEmpty)
            : Script.Empty;

        private static Script Compile(NonEmptyText text) =>
            (Script)Parse(text).Compile(new CompilerImplementation());

        private static ParsingResult Parse(NonEmptyText text) =>
            Parser.Parse(text.Lines);

        private static Parser Parser =>
            Parser.FromXmlResource(Assembly.GetExecutingAssembly(), "LiveCoder.Scripting.Compiler.Parser.ParserDefinition.xml");
    }
}
