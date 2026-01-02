using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Datatypes
{
    public class IntDataType : IDataType
    {
        private readonly long upperBound;
        private readonly long lowerBound;
        private static readonly Random _random = new();

        public IntDataType(long upperBound, long lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        public List<string> GenerateValue(bool unique, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            //if (min >= max)
            //    throw new ArgumentException("min must be less than max");

            //if (unique && count > (max - min))
            //    throw new InvalidOperationException("Cannot generate that many unique integers in the given range.");

            dynamic result = unique ? new HashSet<string>() : new List<string>(count);

            while ((unique && ((HashSet<string>)result).Count < count) ||
                   (!unique && ((List<string>)result).Count < count))
            {
                var value = _random.Next(0, 10000000).ToString();

                if (unique)
                    ((HashSet<string>)result).Add(value);
                else
                    ((List<string>)result).Add(value);
            }

            return unique ? ((HashSet<string>)result).ToList() : (List<string>)result;
        }
    }
}
