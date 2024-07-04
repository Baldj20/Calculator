using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Parsing
    {
        public static string Parse(in string input)
        {
            string output = input;
            if (UserFunction.isFunctionCreating(output))
            {
                return output;
            }
            output = ConvertFunctionsToExpression(output, UserFunction.functions);
            output = ParseInput(output);
            switch (output)
            {
                case "Incorrect input":
                    ErrorMessage.ThrowErrorMessage();
                    return "Incorrect input";
                case "Variable":
                    return output;
                default:
                    if (Validation.IsValidString(output))
                    {
                        return ConvertToReversePolandNotation(output);
                    }
                    else
                    {
                        ErrorMessage.ThrowErrorMessage();
                        return "Incorrect input";
                    }
            }
        }

        public static string ConvertToReversePolandNotation(string input)
        {
            var output = new StringBuilder();
            Stack<char> stack = new Stack<char>();
            char previous_token = '\0';

            foreach (char token in input)
            {
                if (char.IsDigit(token) && char.IsDigit(previous_token))
                {
                    output.Remove(output.Length - 1, 1).Append(token).Append(' ');
                }
                else if ((token == 46 || token == 44) && char.IsDigit(previous_token))
                {
                    output.Remove(output.Length - 1, 1).Append(',');
                }
                else if (char.IsDigit(token))
                {
                    output.Append(token).Append(' ');
                }
                else if (IsOperator(token))
                {
                    while (stack.Count > 0 && IsOperator(stack.Peek()) && GetOperatorPriority(stack.Peek()) >= GetOperatorPriority(token))
                    {
                        output.Append(stack.Pop()).Append(' ');
                    }
                    stack.Push(token);
                }
                else if (token == '(')
                {
                    stack.Push(token);
                }
                else if (token == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output.Append(stack.Pop()).Append(' ');
                    }
                    if (stack.Count > 0 && stack.Peek() == '(')
                    {
                        stack.Pop();
                    }
                }
                previous_token = token;
            }

            while (stack.Count > 0)
            {
                output.Append(stack.Pop()).Append(' ');
            }

            return output.ToString();
        }

        public static bool IsOperator(char symbol)
        {
            return symbol == 42 || symbol == 43 || symbol == 45 || symbol == 47;
        }
      
        public static int GetOperatorPriority(char operator_symbol)
        {
            if (operator_symbol == '+' || operator_symbol == '-')
            {
                return 1;
            }
            else if (operator_symbol == '*' || operator_symbol == '/')
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }      
    }
}
