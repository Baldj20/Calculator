using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class Validation
    {
        static List<string> validCharacters = new List<string>()
        {
            "+","-","*","/","1","2","3","4","5","6","7","8","9","0",")","("
        };
        static bool IsValidSymbol(char symbol)
        {
            foreach (string character in validCharacters)
            {
                if (character == symbol.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsValidString(string input)
        {
            bool result = true;
            for (int i = 0; i < input.Length; i++)
            {
                result = result && IsValidSymbol(input[i]);
            }
            return result;
        }
    }
}
