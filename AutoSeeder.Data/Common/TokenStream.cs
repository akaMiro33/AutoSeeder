using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Parser
{
    public sealed class TokenStream
    {
        private int _position;
        private readonly IReadOnlyList<Token> _tokens;

        public TokenStream(IEnumerable<Token> tokens) => _tokens = tokens.ToList();

        public Token? Peek() => _position < _tokens.Count ? _tokens[_position] : null;

        public Token Consume() => _tokens[_position++];

        public Token Expect(TokenType type, string? value = null)
        {
            var token = Peek();

            if (token is null || token.Type != type || value != null && !token.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Expected {type} {value}, got {token?.Value}");
            }

            return Consume();
        }
    }
}
