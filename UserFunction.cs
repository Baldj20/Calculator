using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    internal class UserFunction
    {
        private static readonly List<char> invalidCharacters = new List<char> 
        {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/', ' ', '\t', '\n', '\r'
        };
        private string _name; 
        private string[] _functionArgs;
        private string _body;
        public UserFunction(string name, string[] functionArgs, string body)
        {
            Name = name;
            FunctionArgs = functionArgs;
            Body = body;
        }

        public UserFunction() { }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string[] FunctionArgs
        {
            get { return _functionArgs; }
            set { _functionArgs = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }


        static bool isFunctionCreating(string exeption)
        {
            if (exeption.Contains("="))
            {
                var parts = exeption.Split('=');
                if (parts.Length != 2)
                    return false; 

                var left = parts[0].Trim();

              
                if (left.Contains("(") && left.Contains(")"))
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
            else
            {
                return false; 
            }

        }

        private void isValidArgument(string argument) 
        {
            foreach (char character in invalidCharacters)
            {
                if (argument.Contains(character))
                {
                    throw new Exception("Недопустимый символ в аргументах функции");
                }
            }

        }

        private void doesBodyContainsArgument(string body, string[] argumnets) 
        {
            // Счётчик аргуметов в теле функции
            int counter = 0;
            for (int i = 0; i < argumnets.Length; i++)
            {
                if (body.Contains(argumnets[i]))
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                throw new Exception("Тело функции не содержит ни одного из аргументов функции");
            }
        }

        static string AddMultiplicationSigns(string expression)
        {
           
            string pattern = @"(\d)([a-zA-Z])";

            string replacement = "$1*$2";

            return Regex.Replace(expression, pattern, replacement);
        }

        public UserFunction CreateUserFunction(string expression) 
        {
            UserFunction userFunction = new UserFunction(null, null, null);

            var parts = expression.Split('=');
            var definition = parts[0].Trim(); 
            var body = AddMultiplicationSigns(parts[1].Trim()); 

     
            var openParen = definition.IndexOf('(');
           
            var closeParen = definition.IndexOf(')');

            
            string name = definition.Substring(0, openParen).Trim();
          
            var args = definition.Substring(openParen + 1, closeParen - openParen - 1).Split(',');

         
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim();
                isValidArgument(args[i]);
            }
            doesBodyContainsArgument(body, args);
            userFunction.Name = name;
            userFunction.FunctionArgs = args;
            userFunction.Body = "(" + body + ")";
            return userFunction;
        }



        public bool isFunctionContains(string expression, List<UserFunction> usersFunctions) 
        {
            bool isFunctionUsed = false;
            foreach (var userFunction in usersFunctions)
            {
                if (expression.Contains(userFunction.Name + "("))
                {
                    isFunctionUsed = true;
                }
            }
            return isFunctionUsed;

        }

        public string ConvertFunctionsToExpression(string expression, List<UserFunction> usersFunctions) 
        {
            while (isFunctionContains(expression, usersFunctions))
            {
                foreach (var userFunction in usersFunctions)
                {
                    if (expression.Contains(userFunction.Name + "("))
                    {
                        
                        var startIndex = expression.IndexOf(userFunction.Name + "(");
                     
                        var endIndex = FindMatchingParenthesis(expression, startIndex + userFunction.Name.Length + 1);
                        
                        var argsString = expression.Substring(startIndex + userFunction.Name.Length + 1, endIndex - startIndex - userFunction.Name.Length - 1);

                        var args = argsString.Split(',');

                        var localVars = new Dictionary<string, double>();
                        for (int i = 0; i < userFunction.FunctionArgs.Length; i++)
                        {
                            localVars[userFunction.FunctionArgs[i]] = Convert.ToDouble(args[i]);
                        }
                        
                        string userFunctionBody = userFunction.Body;
                       
                        for (int i = 0; i < userFunction.FunctionArgs.Length; i++)
                        {
                            
                            string pattern = $@"\b{userFunction.FunctionArgs[i]}\b";
                            userFunctionBody = Regex.Replace(userFunctionBody, pattern, args[i]);
                        }

                      
                        string functionInitText = userFunction.Name + "(" + argsString + ")";
                      
                        expression = expression.Replace(functionInitText, userFunctionBody);
                        break;
                    }
                }
            }
            return expression;
        }

        static int FindMatchingParenthesis(string expression, int startIndex)
        {
            int depth = 0;

            for (int i = startIndex; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                    depth++;
                else if (expression[i] == ')')
                {
                    if (depth == 0)
                        return i;
                    depth--;
                }
            }
         
            throw new Exception("Не найдено соответствующей закрывающей скобки.");
        }

    }
}
