using AutoSeeder.Data.Common.Datatypes;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.ConstraintParsing;
using AutoSeeder.Services.ConstraintParsing.Interfaces;
using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services
{
    public class ParserContext
    {
        private int position = 0;
        private readonly List<IColumnConstraintParser> parsers = [
            new NotNullConstraintParser(),
            new PrimaryKeyConstraintParser(),
            new ForeignKeyConstraintParser(),
            new UniqueConstraintParser(),
            new DefaultConstraintParser(),
            new IdentityConstraintParser()
        ];

    private IReadOnlyList<Token> tokens { get; set; }
        public ParserContext(IReadOnlyList<Token>  tokens)
        {
            this.tokens = tokens;
        }


        public Token Peek() => position < tokens.Count ? tokens[position] : null;
        public Token Consume() => tokens[position++];

        public Token Expect(TokenType type, string value = null)
        {
            var token = Peek();

            if (token == null ||
                token.Type != type ||
                (value != null && !token.Value.Equals(value, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Expected {type} {value}, got {token?.Value}");
            }

            return Consume();
        }

        public List<CreateTableNode> Parse()
        {
            var nodes = new List<CreateTableNode>();

            while (Peek() != null)
            {
                nodes.Add(ParseNode());
            }

            return nodes;
        }


        public List<CreateTableNode> ParseTokens()
        {
            var nodes = new List<CreateTableNode>();

            while (Peek() != null)
            {
                nodes.Add(ParseNode());
            }

            return nodes;
        }


        public CreateTableNode ParseNode()
        {
            Expect(TokenType.Keyword, "CREATE");
            Expect(TokenType.Keyword, "TABLE");

            string tableName = ParseTableName();

            Expect(TokenType.Symbol, "(");

            var table = new CreateTableNode
            {
                TableName = tableName
            };

            while (true)
            {
                if (Peek().Type == TokenType.Keyword &&
                    Peek().Value.Equals("CONSTRAINT", StringComparison.OrdinalIgnoreCase))
                {
                    table.Constraints.Add(ParseTableConstraint());
                }
                else
                {
                    table.Columns.Add(ParseColumn());
                }

                if (Peek().Value == ",")
                {
                    Consume();
                    continue;
                }

                break;
            }

            Expect(TokenType.Symbol, ")");
            if (Peek()?.Value == ";")
                Consume();

            return table;
        }

        public string ParseTableName()
        {
            var name = Expect(TokenType.Identifier).Value;

            if (Peek()?.Value == ".")
            {
                Consume(); // .
                name += "." + Expect(TokenType.Identifier).Value;
            }

            return name;
        }

        public ColumnNode ParseColumn()
        {
            var column = new ColumnNode
            {
                Name = Expect(TokenType.Identifier).Value
            };

            // Data type (e.g. INT, VARCHAR(50))
            column.DataType = ParseDataType();

            while (Peek() != null &&
                   Peek().Type == TokenType.Keyword)
            {
                column.Constraints.Add(ParseColumnConstraint());
            }

            return column;
        }

        private IDataType ParseDataType()
        {

            var type = Expect(TokenType.Identifier).Value;
            var dataTypeDescriptor = new DataTypeDescriptor()
            {
                Name = type,
            };

            if (Peek()?.Value == "(")
            {
                Consume(); // (
                //type += "(";

                var firstNumber = int.Parse(Expect(TokenType.Number).Value);

                if (Peek()?.Value == ",")
                {
                    Consume();
                    var secondNumber = int.Parse(Expect(TokenType.Number).Value);
                    dataTypeDescriptor.Precision = firstNumber;
                    dataTypeDescriptor.Scale = secondNumber;

                }
                else
                {
                    dataTypeDescriptor.Length = firstNumber;
                }

                Expect(TokenType.Symbol, ")");
                //type += ")";
            }

            var dataType = GenerateType(dataTypeDescriptor);


            return dataType;
        }

        private IDataType GenerateType(DataTypeDescriptor type)
        {
            //var type = column.DataType.Trim().ToUpperInvariant();

            return type.Name switch
            {
                // ===== Integers =====
                "TINYINT" => new IntDataType(byte.MinValue, byte.MaxValue),
                "SMALLINT" => new IntDataType(short.MinValue, short.MaxValue),
                "INT" => new IntDataType(int.MinValue, int.MaxValue),
                "BIGINT" => new IntDataType(long.MinValue, long.MaxValue),

                // ===== Exact numerics =====
                "DECIMAL" or "NUMERIC" => new DecimalDataType(decimal.MaxValue, long.MinValue),
                var t when t.StartsWith("DECIMAL(") => new DecimalDataType(decimal.MaxValue, long.MinValue),
                //var t when t.StartsWith("NUMERIC(") => "1.0",
                //"MONEY" => "1.0",
                //"SMALLMONEY" => "1.0",

                //// ===== Approx numerics =====
                //"FLOAT" => "1.0",
                //"REAL" => "1.0",

                //// ===== Bit =====
                //"BIT" => "1",

                //// ===== Date & time =====
                //"DATE" => "CAST(GETDATE() AS DATE)",
                //"DATETIME" => "GETDATE()",
                "DATETIME2" => new DateDataType(),
                //"SMALLDATETIME" => "GETDATE()",
                //"TIME" => "CAST(SYSDATETIME() AS TIME)",
                //"DATETIMEOFFSET" => "SYSDATETIMEOFFSET()",

                //// ===== Character =====
                //"CHAR" => "'A'",
                //var t when t.StartsWith("CHAR(") => "'A'",
                //"VARCHAR" => "'Test'",
                //var t when t.StartsWith("VARCHAR(") => "'Test'",
                //"NCHAR" => "N'A'",
                //var t when t.StartsWith("NCHAR(") => "N'A'",
                "NVARCHAR" => new CharDataType(),
                var t when t.StartsWith("NVARCHAR(") => new CharDataType(),
                //"TEXT" => "'Test'",
                //"NTEXT" => "N'Test'",

                //// ===== Binary =====
                //"BINARY" => "0x00",
                //var t when t.StartsWith("BINARY(") => "0x00",
                //"VARBINARY" => "0x00",
                //var t when t.StartsWith("VARBINARY(") => "0x00",
                //"IMAGE" => "0x00",

                //// ===== Unique identifiers =====
                //"UNIQUEIDENTIFIER" => "NEWID()",

                //// ===== XML =====
                //"XML" => "'<root />'",

                //// ===== SQL Server special =====
                //"ROWVERSION" => "DEFAULT",
                //"TIMESTAMP" => "DEFAULT",

                // ===== Fallback =====
                _ => throw new NotSupportedException(
                    $"Unsupported SQL data type: '{type}'")
            };
        }



        private ColumnConstraintNode ParseColumnConstraint()
        {
            var token = Peek();

            foreach (var parser in parsers)
            {
                if (parser.CanParse(token))
                {
                    return parser.Parse(this);
                }
            }

            throw new Exception($"Unknown column constraint: {token.Value}");
        }

        private TableConstraintNode ParseTableConstraint()
        {
            Expect(TokenType.Keyword, "CONSTRAINT");

            var constraint = new TableConstraintNode
            {
                Name = Expect(TokenType.Identifier).Value
            };

            if (Peek().Value.Equals("PRIMARY", StringComparison.OrdinalIgnoreCase))
            {
                Consume();
                Expect(TokenType.Keyword, "KEY");
                constraint.Type = "PRIMARY KEY";
                constraint.Columns = ParseIdentifierList();
                return constraint;
            }

            if (Peek().Value.Equals("FOREIGN", StringComparison.OrdinalIgnoreCase))
            {
                Consume();
                Expect(TokenType.Keyword, "KEY");
                constraint.Type = "FOREIGN KEY";
                constraint.Columns = ParseIdentifierList();

                Expect(TokenType.Keyword, "REFERENCES");
                constraint.ReferenceTable = ParseTableName();
                ParseIdentifierList();

                return constraint;
            }

            throw new Exception("Unknown table constraint");
        }

        public List<string> ParseIdentifierList()
        {
            var list = new List<string>();

            Expect(TokenType.Symbol, "(");

            list.Add(Expect(TokenType.Identifier).Value);

            while (Peek().Value == ",")
            {
                Consume();
                list.Add(Expect(TokenType.Identifier).Value);
            }

            Expect(TokenType.Symbol, ")");

            return list;
        }
    }
}
