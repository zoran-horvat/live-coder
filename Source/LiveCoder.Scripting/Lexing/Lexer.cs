﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;

namespace LiveCoder.Scripting.Lexing
{
    public class Lexer
    {
        private Regex WhiteSpacePattern { get; }
        private Regex IdentifierPattern { get; }

        private IEnumerable<(Regex pattern, Func<string, Token> tokenFactory)> Patterns { get; }

        public Lexer()
        {
            this.WhiteSpacePattern = new Regex(@"\s+");
            this.IdentifierPattern = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*");

            this.Patterns = new (Regex, Func<string, Token>)[]
            {
                (this.WhiteSpacePattern, WhiteSpace.Of),
                (this.IdentifierPattern, Identifier.Of)
            };
        }

        public IEnumerable<Token> Tokenize(IText text)
        {
            while (text is NonEmptyText remaining)
            {
                foreach (Token partial in this.Tokenize(remaining.CurrentLine))
                    yield return partial;
                yield return new EndOfLine();
                text = remaining.ConsumeLine();
            }
        }

        private IEnumerable<Token> Tokenize(string line)
        {
            int pos = 0;
            while (pos < line.Length && this.Match(line, pos) is Some<(string value, Func<string, Token> tokenFactory)> match)
            {
                yield return match.Content.tokenFactory(match.Content.value);
                pos += match.Content.value.Length;
            }
        }

        private Option<(string value, Func<string, Token> tokenFactory)> Match(string target, int pos) =>
            this.Matches(target, pos)
                .FirstOrNone(tuple => tuple.match.Index == pos)
                .Map(tuple => (tuple.match.Value, tuple.tokenFactory));

        private IEnumerable<(Match match, Func<string, Token> tokenFactory)> Matches(string target, int pos) =>
            this.Patterns
                .Select(tuple => (match: tuple.pattern.Match(target, pos), tuple.tokenFactory))
                .Where(tuple => tuple.match.Success);
    }
}
