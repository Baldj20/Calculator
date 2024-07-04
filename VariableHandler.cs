using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public class VariableHandler
    {
        private RichTextBox historyField;
        private TextBox entryField;
        private Dictionary<string, double> variables;

        public VariableHandler(RichTextBox historyField)
        {
            this.historyField = historyField;

            this.variables = new Dictionary<string, double>();
        }

        public void HandleVariableAssignment(string expression)
        {
            if (expression.Contains("="))
            {
                string[] parts = expression.Split('=');
                string variableName = parts[0].Trim();

                double value;
                if (double.TryParse(parts[1].Trim(), out value))
                {
                    if (variables.ContainsKey(variableName))
                    {
                        variables[variableName] = value;
                    }
                    else
                    {
                        variables.Add(variableName, value);
                    }
                    historyField.AppendText($"{variableName} = {value}" + Environment.NewLine);
                }
                else
                {
                    historyField.AppendText(expression + " = Ошибка (невозможно присвоить значение)" + Environment.NewLine);
                }
            }
        }
        public void HandleExpressionEvaluation(string expression)
        {
            if (!expression.Contains("="))
            {
                string evaluatedExpression = ReplaceVariables(expression);

                var result = new DataTable().Compute(evaluatedExpression, null).ToString();
                historyField.AppendText(expression + " = " + result + Environment.NewLine);
            }
        }

        private string ReplaceVariables(string expression)
        {
            foreach (var variable in variables)
            {
                expression = expression.Replace(variable.Key, variable.Value.ToString());
            }
            return expression;
        }

        public void HandleHistoryFieldClick(Point location)
        {
            int charIndex = historyField.GetCharIndexFromPosition(location);
            if (charIndex >= 0)
            {
                int lineIndex = historyField.GetLineFromCharIndex(charIndex);
                if (lineIndex >= 0 && lineIndex < historyField.Lines.Length)
                {
                    string clickedText = historyField.Lines[lineIndex];
                    int equalsIndex = clickedText.IndexOf('=');
                    if (equalsIndex != -1)
                    {
                        string expression = clickedText.Substring(0, equalsIndex).Trim();
                        entryField.Text = expression;
                    }
                    else
                    {
                        entryField.Text = clickedText.Trim();
                    }
                }
            }
        }
    }
}
