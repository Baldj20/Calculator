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
        private static readonly List<char> invalidCharacters = new List<char> //Недопустимые символы
        {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/', ' ', '\t', '\n', '\r'
        };
        private string _name; // Название функции
        private string[] _functionArgs; // Переменные функции
        private string _body; // Тело функции
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


        public bool isFunctionCreating(string exeption) // Метод будет вызываться для проверки создается ли функция 
        {
            if (exeption.Contains("="))
            {
                var parts = exeption.Split('=');
                if (parts.Length != 2)
                    return false; // Возникает из-за того что есть только левая или только правая часть функции (Ошибка)

                var left = parts[0].Trim(); //определение названия и аргрументов функции

                // Проверка на наличие определения функции
                if (left.Contains("(") && left.Contains(")"))
                {
                    return true;
                }
                else
                {
                    return false; // Нет , так как возможно создается переменная 
                }
            }
            else
            {
                return false; // Нет , так как это выражение для подсчета
            }

        }

        private void isValidArgument(string argument) // Метод проверки валидности символов в названии аргументов
        {
            foreach (char character in invalidCharacters)
            {
                if (argument.Contains(character))
                {
                    throw new Exception("Недопустимый символ в аргументах функции");
                }
            }

        }

        private void doesBodyContainsArgument(string body, string[] argumnets) // Метод проверка содрежит ли тело функции один из аргументов функции
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

        static string AddMultiplicationSigns(string expression)// Метод добавляет знак * между числом и переменной (3x = 3*x)
        {
            // Регулярное выражение для поиска числа, за которым следует переменная
            string pattern = @"(\d)([a-zA-Z])";

            // Замена найденных совпадений на число * переменная
            string replacement = "$1*$2";

            // Применение замены к исходному выражению

            return Regex.Replace(expression, pattern, replacement);
        }

        public UserFunction CreateUserFunction(string expression) // Метод создания пользовательской функции
        {
            UserFunction userFunction = new UserFunction(null, null, null);

            var parts = expression.Split('=');// разделение выражения на части (определение функции и тело функции)
            var definition = parts[0].Trim(); //определения названия и аргрументов функции
            var body = AddMultiplicationSigns(parts[1].Trim()); // тело функции

            // Найти позицию открывающей круглой скобки в определении функции
            var openParen = definition.IndexOf('(');
            // Найти позицию закрывающей круглой скобки в определении функции
            var closeParen = definition.IndexOf(')');

            // Извлечь имя функции из строки определения
            string name = definition.Substring(0, openParen).Trim();
            // Извлечь аргументы функции из строки определения и разбить их по запятым
            var args = definition.Substring(openParen + 1, closeParen - openParen - 1).Split(',');

            // Убрать лишние пробелы у каждого аргумента
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim();
                isValidArgument(args[i]);// Проверка на название аргументов
            }
            doesBodyContainsArgument(body, args);//Проверка на наличие аргумента в теле функции
            userFunction.Name = name;
            userFunction.FunctionArgs = args;
            userFunction.Body = "(" + body + ")";
            return userFunction;
        }



        public bool isFunctionContains(string expression, List<UserFunction> usersFunctions) //Метод будет проверять содержаться ли в выражении функция 
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

        public string ConvertFunctionsToExpression(string expression, List<UserFunction> usersFunctions) // Метод который конвертирует введеную строку с пользовательскими функциями в обычную строку для польской итерации
        {
            while (isFunctionContains(expression, usersFunctions))
            {
                foreach (var userFunction in usersFunctions)
                {
                    if (expression.Contains(userFunction.Name + "("))
                    {
                        // Найти начало вызова функции
                        var startIndex = expression.IndexOf(userFunction.Name + "(");
                        // Найти конец вызова функции (соответствующую закрывающую скобку)
                        var endIndex = FindMatchingParenthesis(expression, startIndex + userFunction.Name.Length + 1);
                        // Извлечь строку аргументов функции
                        var argsString = expression.Substring(startIndex + userFunction.Name.Length + 1, endIndex - startIndex - userFunction.Name.Length - 1);

                        // Разбить аргументы по запятым и вычислить их значения
                        var args = argsString.Split(',');

                        var localVars = new Dictionary<string, double>();
                        for (int i = 0; i < userFunction.FunctionArgs.Length; i++)
                        {
                            localVars[userFunction.FunctionArgs[i]] = Convert.ToDouble(args[i]);
                        }
                        // Тело функции
                        string userFunctionBody = userFunction.Body;
                        // Заменить имена переменных на их значения в выражении
                        for (int i = 0; i < userFunction.FunctionArgs.Length; i++)
                        {
                            //Позволяет найти подстроку, которая является отдельным словом ,а не частью другого слова 
                            string pattern = $@"\b{userFunction.FunctionArgs[i]}\b";// \b - указывает на начало и конец подстроки
                            userFunctionBody = Regex.Replace(userFunctionBody, pattern, args[i]);
                        }

                        //Получение текста вызова функции
                        string functionInitText = userFunction.Name + "(" + argsString + ")";
                        //Изменение входной строки на формулу функции с числами
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

            // Пройтись по символам строки, начиная с заданного индекса
            for (int i = startIndex; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                    depth++;
                else if (expression[i] == ')')
                {
                    if (depth == 0)
                        return i;//Соответствующая закрывающая скобка для начальной открывающей скобки
                    depth--;//Найдена одна из внутренних закрывающих скобок
                }
            }

            // Если не найдено соответствующей закрывающей скобки, выбросить исключение
            throw new Exception("Не найдено соответствующей закрывающей скобки.");
        }

    }
}
