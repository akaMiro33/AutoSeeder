using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.DataTypeFactory
{
    public class DataTypeFactoryBase 
    {
        public DataTypeDescriptor ParseDataType(TokenStream tokens)
        {
            var type = tokens.Expect(TokenType.Identifier).Value;
            var dataTypeDescriptor = new DataTypeDescriptor()
            {
                Name = type,
            };

            if (tokens.Peek()?.Value == "(")
            {
                tokens.Consume(); // (
                                   //type += "(";

                var firstNumber = int.Parse(tokens.Expect(TokenType.Number).Value);

                if (tokens.Peek()?.Value == ",")
                {
                    tokens.Consume();
                    var secondNumber = int.Parse(tokens.Expect(TokenType.Number).Value);
                    dataTypeDescriptor.Precision = firstNumber;
                    dataTypeDescriptor.Scale = secondNumber;

                }
                else
                {
                    dataTypeDescriptor.Length = firstNumber;
                }

                tokens.Expect(TokenType.Symbol, ")");
                //type += ")";

            }

            return dataTypeDescriptor;
        }
    }
}
