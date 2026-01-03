using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common
{
    public static class SqlTokenizer
    {
        private static readonly HashSet<char> Symbols = ['(', ')', ',', ';', '.'];
        private static readonly HashSet<string> Keywords = ["CREATE", "TABLE", "PRIMARY", "KEY", "FOREIGN", "REFERENCES", "NOT", "NULL", "DEFAULT", "CHECK", "UNIQUE", "CONSTRAINT", "IDENTITY"];
        private static readonly HashSet<string> IngoredKeywords = ["ON", "NO", "DELETE", "CASCADE", "UPDATE", "ACTION", "CLUSTERED", "NONCLUSTERED", "SET", "CHECK"];

        public static List<Token> GetTokens(string input)
        {
            var tokens = new List<Token>();
            int i = 0;

            while (i < input.Length)
            {
                char c = input[i];

                // Skip whitespace
                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                // Symbols
                if (Symbols.Contains(c))
                {
                    tokens.Add(new Token(TokenType.Symbol, c.ToString()));
                    i++;
                    continue;
                }

                // String literal
                if (c == '\'')
                {
                    int start = ++i;
                    while (i < input.Length && input[i] != '\'')
                        i++;

                    tokens.Add(new Token(
                        TokenType.String,
                        input[start..i]
                    ));

                    i++; // skip closing quote
                    continue;
                }

                // Identifier or keyword (dbo.Table, [Table], "Table")
                if (char.IsLetter(c) || c == '_' || c == '[' || c == '"')
                {
                    int start = i;

                    if (c == '[')
                    {
                        i++;
                        while (i < input.Length && input[i] != ']')
                            i++;
                        i++;
                    }
                    else if (c == '"')
                    {
                        i++;
                        while (i < input.Length && input[i] != '"')
                            i++;
                        i++;
                    }
                    else
                    {
                        while (i < input.Length &&
                               (char.IsLetterOrDigit(input[i]) || input[i] == '_'))
                        {
                            i++;
                        }
                    }

                    string value = input[start..i];

                    if(IsIgnoredKeyword(value))
                    {
                        continue;
                    }

                    tokens.Add(new Token(
                        IsKeyword(value) ? TokenType.Keyword : TokenType.Identifier,
                        value
                    ));

                    continue;
                }

                // Number
                if (char.IsDigit(c))
                {
                    int start = i;
                    while (i < input.Length && char.IsDigit(input[i]))
                        i++;

                    tokens.Add(new Token(
                        TokenType.Number,
                        input[start..i]
                    ));

                    continue;
                }

                throw new Exception($"Unexpected character: {c}");
            }

            return tokens;
        }

        private static bool IsKeyword(string value)
        {
            return Keywords.Contains(value.Trim('[', ']', '"').ToUpper());
        }

        private static bool IsIgnoredKeyword(string value)
        {
            return IngoredKeywords.Contains(value.Trim('[', ']', '"').ToUpper());
        }

    }
}
