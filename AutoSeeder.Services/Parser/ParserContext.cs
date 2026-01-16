using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Data.Common.Datatypes;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.ServiceContracts.Parser;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoSeeder.Services.Parser
{
    //public interface IParserContext
    //{
    //    (ColumnNode Column, List<ConstraintNode> Constraints) ParseColumn();
    //    List<string> ParseIdentifierList();
    //    CreateTableNode ParseNode();
    //    string ParseTableName();
    //    IReadOnlyList<CreateTableNode> ParseTokens();
    //}

    public sealed class ParserContext : IParserContext
    {
        private readonly TokenStream _tokens;
        private readonly IReadOnlyList<IColumnConstraintParser> _constraintParsers;
        private readonly IDataTypeFactory _dataTypeFactory;

        public ParserContext(IEnumerable<Token> tokens, IEnumerable<IColumnConstraintParser> constraintParsers, IDataTypeFactory dataTypeFactory)
        {
            _tokens = new TokenStream(tokens);
            _constraintParsers = constraintParsers.ToList();
            _dataTypeFactory = dataTypeFactory;
        }

        public IReadOnlyList<CreateTableNode> ParseTokens()
        {
            var nodes = new List<CreateTableNode>();

            while (_tokens.Peek() != null)
            {
                nodes.Add(ParseNode());
            }

            return nodes;
        }

        public CreateTableNode ParseNode()
        {
            _tokens.Expect(TokenType.Keyword, "CREATE");
            _tokens.Expect(TokenType.Keyword, "TABLE");

            string tableName = ParseTableName();

            _tokens.Expect(TokenType.Symbol, "(");

            var table = new CreateTableNode
            {
                TableName = tableName
            };

            while (true)
            {
                if (_tokens.Peek()?.Type == TokenType.Keyword)
                {
                    table.Constraints.Add(ParseTableConstraint());
                }
                else
                {
                    var (column, constraint) = ParseColumn();
                    table.Columns.Add(column);

                    if (constraint != null && constraint.Count > 0)
                    {
                        table.Constraints.AddRange(constraint);
                    }
                }

                if (_tokens.Peek().Value == ",")
                {
                    _tokens.Consume();
                    continue;
                }

                break;
            }

            _tokens.Expect(TokenType.Symbol, ")");
            if (_tokens.Peek()?.Value == ";")
                _tokens.Consume();

            return table;
        }

        private ConstraintNode ParseTableConstraint()
        {
            var constraint = new ConstraintNode();

            if (_tokens.Peek().Value.Equals("CONSTRAINT", StringComparison.OrdinalIgnoreCase))
            {
                _tokens.Expect(TokenType.Keyword, "CONSTRAINT");
                constraint.Name = _tokens.Expect(TokenType.Identifier).Value;
            }

            if (_tokens.Peek().Value.Equals("PRIMARY", StringComparison.OrdinalIgnoreCase))
            {
                _tokens.Consume();
                _tokens.Expect(TokenType.Keyword, "KEY");
                constraint.Type = "PRIMARY KEY";
                constraint.Columns = ParseIdentifierList();
                return constraint;
            }

            if (_tokens.Peek().Value.Equals("FOREIGN", StringComparison.OrdinalIgnoreCase))
            {
                _tokens.Consume();
                _tokens.Expect(TokenType.Keyword, "KEY");
                constraint.Type = "FOREIGN KEY";
                constraint.Columns = ParseIdentifierList();

                _tokens.Expect(TokenType.Keyword, "REFERENCES");
                constraint.ReferenceTable = ParseTableName();
                constraint.ReferenceColumns = ParseIdentifierList();

                return constraint;
            }

            throw new Exception("Unknown table constraint");
        }

        public List<string> ParseIdentifierList()
        {
            var list = new List<string>();

            _tokens.Expect(TokenType.Symbol, "(");

            list.Add(_tokens.Expect(TokenType.Identifier).Value);

            while (_tokens.Peek().Value == ",")
            {
                _tokens.Consume();
                list.Add(_tokens.Expect(TokenType.Identifier).Value);
            }

            _tokens.Expect(TokenType.Symbol, ")");

            return list;
        }

        public string ParseTableName()
        {
            var name = _tokens.Expect(TokenType.Identifier).Value;

            if (_tokens.Peek()?.Value == ".")
            {
                _tokens.Consume(); // .
                name += "." + _tokens.Expect(TokenType.Identifier).Value;
            }

            return name;
        }

        public (ColumnNode Column, List<ConstraintNode> Constraints) ParseColumn()
        {
            var column = new ColumnNode
            {
                Name = _tokens.Expect(TokenType.Identifier).Value
            };

            column.DataType = _dataTypeFactory.Create(_tokens);

            var constraints = new List<ConstraintNode>();

            while (_tokens.Peek()?.Type == TokenType.Keyword)
            {
                constraints.Add(ParseColumnConstraint(column.Name));
            }

            return (column, constraints);
        }

        private ConstraintNode ParseColumnConstraint(string columnName)
        {
            var token = _tokens.Peek()!;

            var parser = _constraintParsers.FirstOrDefault(p => p.CanParse(token));

            if (parser is null)
            {
                throw new Exception($"Unknown column constraint: {token.Value}");
            }

            return parser.Parse(_tokens, this, columnName);
        }

    }
}
