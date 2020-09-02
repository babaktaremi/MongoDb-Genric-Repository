using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbTest.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerFirstChar(this string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
        }
    }
}
