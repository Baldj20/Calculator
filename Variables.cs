using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class Variables
    {
        private static Dictionary<string, string> variables = new Dictionary<string, string>();

        public static string GetVariableValue(string variableName)
        {
            if (IsVariableDefined(variableName))
            {
                return variables[variableName];
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not defined.");
            }
        }

        public static void SetVariableValue(string variableName, string variableValue)
        {
            variables[variableName] = variableValue;
        }

        public static bool IsVariableDefined(string variableName)
        {
            return variables.ContainsKey(variableName);
        }

        public static string ReplaceVariables(string expression)
        {
            foreach (var variable in variables)
            {
                string variableKey = variable.Key;
                string variableValue = variable.Value.ToString();

                expression = Regex.Replace(expression, $@"(?<=\d|\))({variableKey})(?=\d|\()", $"*{variableValue}*");
                expression = Regex.Replace(expression, $@"(\d)({variableKey})", $"$1*{variableValue}");
                expression = Regex.Replace(expression, $@"({variableKey})(\d)", $"{variableValue}*$2");
                expression = Regex.Replace(expression, $@"({variableKey})(?=\()", $"{variableValue}*");
                expression = Regex.Replace(expression, $@"(?<=\))({variableKey})", $"*{variableValue}");
                expression = Regex.Replace(expression, $@"\b{variableKey}\b", variableValue);
            }
            return expression;
        }
        public static void AssignVariable(string name, string expression)
        {
            SetVariableValue(name, expression);
        }

        public static string ParseInput(string input)
        {
            input = input.Trim();

            if (input.Contains("="))
            {
                var parts = input.Split(new char[] { '=' }, 2);
                if (parts.Length != 2 || parts[1] == "" || parts[0] == "")
                    return "Incorrect input";
                var left = parts[0].Trim();
                var right = parts[1].Trim();

                if (!left.Contains("(") && !left.Contains(")"))
                {
                    Variables.AssignVariable(left, right);
                    return "Variable";
                }
                else
                {
                   return"Incorrect input";
                }
            }
            else
            {
                return Variables.ReplaceVariables(input);
            }
        }
    }
}