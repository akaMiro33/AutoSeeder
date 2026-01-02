using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.Datatypes
{
    public class DecimalDataType : IDataType
    {
        private readonly decimal upperBound;
        private readonly decimal lowerBound;
        private static readonly Random _random = new();


        public DecimalDataType(decimal upperBound, decimal lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        public List<string> GenerateValue(bool unique, int count)
        {
            var data = new List<string>();

            for (int i = 0; i < count; i++)
            {
                data.Add(_random.NextDouble().ToString());
            }

            return data;
        }
    }
}
