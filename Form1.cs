using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private TextBox entryField;
        private RichTextBox historyField;
        private ButtonHandler buttonHandler;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            entryField = Entry_Field;
            historyField = richTextBox1;

            buttonHandler = new ButtonHandler(entryField, historyField);

            X_Button.Click += buttonHandler.X_Button_Click;
            Y_Button.Click += buttonHandler.Y_Button_Click;
            Z_Button.Click += buttonHandler.Z_Button_Click;
            FunctionX_Button.Click += buttonHandler.FunctionX_Button_Click;
            FunctionXY_Button.Click += buttonHandler.FunctionXY_Button_Click;
            FunctionXYZ_Button.Click += buttonHandler.FunctionXYZ_Button_Click;
            LeftBrackets_Button.Click += buttonHandler.LeftBrackets_Button_Click;
            RightBrackets_Button.Click += buttonHandler.RightBrackets_Button_Click;

            // Привязка кнопок к методам в ButtonHandler
            Plus_Button.Click += (sender, e) => buttonHandler.OperationButton_Click("+");
            Minus_Button.Click += (sender, e) => buttonHandler.OperationButton_Click("-");
            Multiply_Button.Click += (sender, e) => buttonHandler.OperationButton_Click("*");
            Divide_Button.Click += (sender, e) => buttonHandler.OperationButton_Click("/");
            Equals_Button.Click += (sender, e) => buttonHandler.Equals_Button_Click();
            Delete_Button.Click += (sender, e) => buttonHandler.Delete_Button_Click();
            Clear_Button.Click += (sender, e) => buttonHandler.Clear_Button_Click();
            Enter_Button.Click += buttonHandler.Enter_Button_Click;
            historyField.MouseClick += buttonHandler.HistoryField_MouseClick;
            Dot_Button.Click += buttonHandler.Dot_Button_Click;

            n0.Click += buttonHandler.NumberButton_Click;
            n1.Click += buttonHandler.NumberButton_Click;
            n2.Click += buttonHandler.NumberButton_Click;
            n3.Click += buttonHandler.NumberButton_Click;
            n4.Click += buttonHandler.NumberButton_Click;
            n5.Click += buttonHandler.NumberButton_Click;
            n6.Click += buttonHandler.NumberButton_Click;
            n7.Click += buttonHandler.NumberButton_Click;
            n8.Click += buttonHandler.NumberButton_Click;
            n9.Click += buttonHandler.NumberButton_Click;
            comma.Click += buttonHandler.CommaButton_Click;

        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть форму?", "Подтверждение закрытия", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
