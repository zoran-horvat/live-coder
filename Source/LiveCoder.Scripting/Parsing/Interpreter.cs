using System.Collections.Generic;
using System.Linq;
using System.Xml;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Parsing.Instructions;

namespace LiveCoder.Scripting.Parsing
{
    public class Interpreter
    {
        public IEnumerable<Instruction> Parse(TokensArray tokens)
        {
            bool produced = false;
            do
            {
                produced = false;
                foreach ((Instruction instruction, TokensArray rest) tuple in this.ParseNext(tokens))
                {
                    yield return tuple.instruction;
                    tokens = tuple.rest;
                    produced = true;
                }
            } while (produced);
        }

        private IEnumerable<(Instruction instruction, TokensArray rest)> ParseNext(TokensArray tokens)
        {
            if (tokens.FirstOrNone() is Some<Token> some &&
                some.Content is Token firstToken &&
                firstToken != Operator.Of("."))
            {
                yield return (new SelectGlobalScope(), tokens);
            }
        }
    }
}
