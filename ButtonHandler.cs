using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public class ButtonHandler
    {
        private TextBox entryField;
        private RichTextBox historyField;
        private VariableHandler variableHandler;

        public ButtonHandler(TextBox entryField, RichTextBox historyField)
        {
            this.entryField = entryField;
            this.historyField = historyField;
            this.variableHandler = new VariableHandler(historyField);
        }
        public void NumberButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            if (button != null)
            {
                if (entryField.Text == "0" && entryField.Text != null)
                {
                    entryField.Text = button.Text;
                }
                else
                {
                    entryField.Text += button.Text;
                }
            }
        }

        public void CommaButton_Click(object sender, EventArgs e)
        {
            entryField.Text += ",";
        }
        public void Dot_Button_Click(object sender, EventArgs e)
        {
            if (!entryField.Text.Contains(".") && entryField.Text != "")
            {
                entryField.Text += ".";
            }
        }
        public void X_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "x";
        }

        public void Y_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "y";
        }

        public void Z_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "z";
        }

        public void FunctionX_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "F(x)";
        }

        public void FunctionXY_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "F(x,y)";
        }
        public void FunctionXYZ_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "F(x,y,z)";
        }
        public void LeftBrackets_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += "(";
        }

        public void RightBrackets_Button_Click(object sender, EventArgs e)
        {
            entryField.Text += ")";
        }

        public void Enter_Button_Click(object sender, EventArgs e)
        {
            string expression = entryField.Text.Trim();
            if (string.IsNullOrEmpty(expression))
            {
                return; 
            }
            try
            {
                variableHandler.HandleVariableAssignment(expression);
                variableHandler.HandleExpressionEvaluation(expression);

                entryField.Clear();
            }
            catch (Exception)
            {
                historyField.AppendText(expression + " = Ошибка" + Environment.NewLine);
            }
        }
        public void OperationButton_Click(string operation)
        {
            if (!string.IsNullOrEmpty(entryField.Text))
            {
                entryField.Text += $" {operation} ";
            }
        }
        public void Equals_Button_Click()
        {
            entryField.Text += " = ";
        }

        public void Delete_Button_Click()
        {
            if (entryField.Text.Length > 0)
            {
                entryField.Text = entryField.Text.Remove(entryField.Text.Length - 1, 1);
            }
        }
        public void Clear_Button_Click()
        {
            entryField.Clear();
        }
        public void HistoryField_MouseClick(object sender, MouseEventArgs e)
        {
            variableHandler.HandleHistoryFieldClick(e.Location);
        }
    }
}
