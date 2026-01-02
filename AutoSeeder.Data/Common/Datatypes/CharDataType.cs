using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.Datatypes
{
    public class CharDataType : IDataType
    {
        private static readonly char[] AllowedChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        private static readonly Random _random = new();
        private static readonly int length = 5;

        //unique yet not implemented
        public List<string> GenerateValue(bool unique, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var result = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                var chars = new char[length];

                for (int j = 0; j < length; j++)
                {
                    chars[j] = AllowedChars[_random.Next(AllowedChars.Length)];
                }

                string help = new string(chars);
                //string help2 = "\'" + help + "\'";
                string help2 = $"'{help}'";

                result.Add(help2);
                //result.Add(new string(chars));
            }

            return result;
        }
    }
}
