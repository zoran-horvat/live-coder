using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeStringTests : TokenizeTestsBase
    {
        [Fact]
        public void EmptyQuotedString_ReturnsSingleToken() =>
            Assert.Single(this.TokenizeContentOnly(this.EmptyString));

        [Fact]
        public void EmptyQuotedString_ReturnsStringLiteral() =>
            Assert.IsType<StringLiteral>(this.SingleToken(this.EmptyString));

        [Fact]
        public void EmptyQuotedString_ReturnsStringWithEmptyRawContent() =>
            Assert.Equal(string.Empty, this.SingleStringLiteral(this.EmptyString).RawValue);

        private string EmptyString => this.String(string.Empty);

        private string String(string content) =>
            $"\"{content}\"";

        private StringLiteral SingleStringLiteral(params string[] text) =>
            (StringLiteral)this.SingleToken(text);

        private Token SingleToken(params string[] text) =>
            this.TokenizeContentOnly(text).Single();

        private IEnumerable<Token> TokenizeContentOnly(params string[] text) =>
            base.Tokenize(text).Where(token => !(token is EndOfLine));
    }
}
