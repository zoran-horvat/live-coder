using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Parsing.Instructions;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseGlobalAttributeTests
    {
        private class GlobalScope : IContext
        {
            public ISolution Solution { get; }
         
            public GlobalScope(ISolution solution)
            {
                this.Solution = solution;
            }
        }

        private class LocalScope : ISolution
        {
            public Option<FileInfo> File { get; }
         
            public LocalScope(Option<FileInfo> file)
            {
                File = file;
            }

        }

        [Theory]
        [InlineData("solution", typeof(ISolution), Skip = "ResolveAttribute instruction cannot execute yet")]
        public void ReceivesGlobalAttribute_ReturnsScopeOfExpectedType(string script, Type expectedScopeType) =>
            Assert.IsType(expectedScopeType, this.Evaluate(script));

        [Theory]
        [InlineData("something", typeof(SelectGlobalScope), typeof(ResolveAttribute))]
        [InlineData("something.again.andAgain", typeof(SelectGlobalScope), typeof(ResolveAttribute), typeof(ResolveAttribute), typeof(ResolveAttribute))]
        public void ReceivesAttributeReferences_ReturnsInstructionsOfSpecificTypes(string script, params Type[] expectedInstructionType) =>
            Assert.All(
                expectedInstructionType.Zip(this.GetInstructions(script), (expectedType, instruction) => (expectedType, instruction)),
                (tuple) => Assert.IsType(tuple.expectedType, tuple.instruction));
            

        private object Evaluate(string script) =>
            this.Evaluate(script, new GlobalScope(new LocalScope(None.Value)));

        private object Evaluate(string script, IContext globalScope) =>
            this.GetInstructions(script).Aggregate(
                    (globalScope, localScope: Option.None<object>()),
                    (acc, instruction) => (acc.globalScope, instruction.Execute(acc.globalScope, acc.localScope)))
                .localScope;

        private IEnumerable<Instruction> GetInstructions(string script) =>
            this.GetInstructions(this.Tokenize(script));

        private IEnumerable<Instruction> GetInstructions(TokensArray tokens) =>
            new Interpreter().Parse(tokens);

        private TokensArray Tokenize(string script) =>
            new TokensArray(this.GetTokens(script));

        private IEnumerable<Token> GetTokens(string script) =>
            new Lexer().Tokenize(script);
    }
}
