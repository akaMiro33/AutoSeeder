using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.Services.Common.ConstraintParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.ServiceContracts.Parser
{
    public interface IParserService
    {
        IReadOnlyList<CreateTableNode> ParseTokens(IReadOnlyList<Token> tokens, IEnumerable<IColumnConstraintParser> constraintParsers, IDataTypeFactory dataTypeFactory);
    }
}
