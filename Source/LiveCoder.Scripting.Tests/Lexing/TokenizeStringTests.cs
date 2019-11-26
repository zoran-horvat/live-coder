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
            Assert.Equal(
                string.Empty, 
                this.SingleStringRawValue(this.EmptyString));

        [Theory]
        [InlineData("something")]
        [InlineData("Something")]
        [InlineData("Something again, and again")]
        public void PlainString_ReturnsStringLiteralWithThatRawContent(string expectedContent) =>
            Assert.Equal(
                expectedContent, 
                this.SingleStringRawValue(this.String(expectedContent)));

        [Theory]
        [InlineData(@"Something\\again", @"Something\again")]
        [InlineData(@"Something\\\nagain", @"Something\\nagain")]
        [InlineData(@"Something\""\na\""gain", @"Something""\na""gain")]
        public void EscapedString_ReturnsStringLiteralWithUnescapedRawContent(string escapedContent, string expectedContent) =>
            Assert.Equal(
                expectedContent,
                this.SingleStringRawValue(this.String(escapedContent)));

        private string EmptyString => this.String(string.Empty);

        private string String(string content) =>
            $"\"{content}\"";

        private string SingleStringRawValue(params string[] text) =>
            this.SingleStringLiteral(text).RawValue;

        private StringLiteral SingleStringLiteral(params string[] text) =>
            (StringLiteral)this.SingleToken(text);

        private Token SingleToken(params string[] text) =>
            this.TokenizeContentOnly(text).Single();

        private IEnumerable<Token> TokenizeContentOnly(params string[] text) => 
            base.Tokenize(text).Where(token => !(token is EndOfLine));
    }
}
