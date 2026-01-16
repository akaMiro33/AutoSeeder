using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.ServiceContracts.Parser
{
    public interface IParserContext
    {
        (ColumnNode Column, List<ConstraintNode> Constraints) ParseColumn();
        List<string> ParseIdentifierList();
        CreateTableNode ParseNode();
        string ParseTableName();
        IReadOnlyList<CreateTableNode> ParseTokens();
    }
}
