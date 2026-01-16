using AutoSeeder.Data.Common.Datatypes;
using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.Services.Datatypes;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.DataTypeFactory
{
    public sealed class SqlTypeFactory : DataTypeFactoryBase,  IDataTypeFactory 
    {
        //public IDataType Create(DataTypeDescriptor d)
        public IDataType Create(TokenStream tokens)
        {
            var dataTypeDescriptor = base.ParseDataType(tokens);
            return dataTypeDescriptor.Name switch
            {
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
                    $"Unsupported SQL data type: '{dataTypeDescriptor.Name}'")
            };

        }
    }
}
