using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Parsing.Instructions;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseDotTests
    {
        [Theory]
        [InlineData(".something")]
        [InlineData(".something.again(andAgain)")]
        public void BeginsWithDot_FirstInstructionIsNotSelectGlobalScope(string text) =>
            this.OptionalFirstInstruction(text).Do(Assert.IsNotType<SelectGlobalScope>);

        [Theory]
        [InlineData("something")]
        [InlineData("something.again(andAgain)")]
        public void BeginsWithNonDot_FirstInstructionIsSelectGlobalScope(string text) =>
            Assert.IsType<SelectGlobalScope>(this.FirstInstruction(text));

        private Option<Instruction> OptionalFirstInstruction(string text) =>
            this.Parse(text).FirstOrNone();

        private Instruction FirstInstruction(string text) =>
            this.Parse(text).First();

        private IEnumerable<Instruction> Parse(string text) =>
            new Interpreter().Parse(this.Tokenize(text));

        private TokensArray Tokenize(string text) =>
            new TokensArray(this.GetTokens(text));

        private IEnumerable<Token> GetTokens(string text) =>
            new Lexer().Tokenize(text);
    }
}
