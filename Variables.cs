using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                expression = expression.Replace(variable.Key, variable.Value.ToString());
            }
            return expression;
        }
        public static void AssignVariable(string name, string expression)
        {
            SetVariableValue(name,expression);
        }

    }
}