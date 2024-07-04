using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class CalcResult
    {
        public static double Evaluate(string expression)
        {
            Stack<double> stack = new Stack<double>();

            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (Variables.IsVariableDefined(token))
                {
                    double.TryParse(Variables.GetVariableValue(token), out double num);
                    stack.Push(num);
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;

                        case "-":
                            stack.Push(a - b);
                            break;

                        case "*":
                            stack.Push(a * b);
                            break;

                        case "/":
                            stack.Push(a / b);
                            break;

                        default:
                            throw new ArgumentException($"Invalid operator: {token}");
                    }
                }
            }

            return stack.Pop();
        }
    }
}
