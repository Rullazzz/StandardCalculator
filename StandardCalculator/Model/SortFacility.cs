using System;
using System.Collections.Generic;
using System.Linq;

namespace StandardCalculator.Model
{
	public class SortFacility : ICalculator
	{
		private readonly List<string> Operators;

		public SortFacility()
		{
			Operators = new List<string>()
			{
				"+",
				"-",
				"*",
				"/"
			};
		}

		private int GetPriority(string op)
		{
			if (string.IsNullOrEmpty(op))
			{
				throw new ArgumentException(nameof(op));
			}

			switch (op)
			{
				case "+":
				case "-":
					return 1;
				case "*":
				case "/":
					return 2;
				case "~": // Унарный минус
				case "#": // Унарный плюс
					return 3;
				default:
					throw new ArgumentException(op);
			}
		}

		private int GetAssociativity(string expression, int counter)
		{
			#region Проверка
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException(nameof(expression));
			}
			if (counter < 0)
			{
				throw new ArgumentException(nameof(counter));
			}
			#endregion

			if (counter == 0)
			{
				return 0; // Правоассоциативный
			}
			if (int.TryParse(expression[counter - 1].ToString(), out int _) || expression[counter - 1] == ')')
			{
				return 1; // Левоассоциативный
			}		
			return 0;
		}

		private string GetUnaryOperation(string oper)
		{
			if (oper == "-" || oper == "+")
			{
				if (oper == "-")
					oper = "~";
				else
					oper = "#";
			}
			return oper;
		}

		private bool IsBinary(string oper)
		{
			if (oper is null)
			{
				throw new ArgumentNullException(nameof(oper));
			}

			return Operators.Contains(oper);
		}

		/// <summary>
		/// Возвращает обратную польскую запись. 
		/// </summary>
		/// <param name="expression"> Выражение, которое требуется преобразовать. </param>
		/// <returns> Обратная польская запись. </returns>
		public string GetPolishNotation(string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw new ArgumentException(nameof(expression));
			}

			expression = expression.Replace(" ", ""); // Убираем все пробелы
			var stack = new Stack<string>();
			var result = "";
			var lenght = expression.Length;

			for (var i = 0; i < lenght; i++)
			{
				if (int.TryParse(expression[i].ToString(), out int _))
				{
					var j = i;
					for (; j < lenght; j++)
					{
						if (int.TryParse(expression[j].ToString(), out int _) || expression[j] == ',')
							result += expression[j];
						else
							break;
					}
					i = --j;

					result += " ";
					continue;
				}
				if (expression[i] == ',')
				{
					result += expression[i];
					continue;
				}
				if (Operators.Contains(expression[i].ToString()))
				{
					var oper = expression[i].ToString();

					if (stack.Count > 0 && stack.Peek() != "(" && stack.Peek() != ")")
					{
						while (GetPriority(stack.Peek()) > GetPriority(oper)
						|| (GetPriority(stack.Peek()) == GetPriority(oper)) && (GetAssociativity(expression, i) == 1))
						{
							if (GetAssociativity(expression, i) == 1)
								result += stack.Pop() + " ";
							else
								result += GetUnaryOperation(stack.Pop()) + " ";
							if (stack.Count == 0)
								break;
						}
					}
					if (GetAssociativity(expression, i) == 1)
						stack.Push(oper);
					else
						stack.Push(GetUnaryOperation(oper));
					continue;
				}
				if (expression[i] == '(')
					stack.Push("(");
				if (expression[i] == ')')
				{
					while (stack.Peek() != "(")
					{
						if (stack.Count == 1)
							throw new ArgumentException("Пропущена скобка!");
						
						result += stack.Pop() + " ";
					}
					stack.Pop(); // Выкидываем закрывающую скобку.
				}
			}
			while (stack.Count > 0)
			{
				if (stack.Peek() == "(" || stack.Peek() == ")")
					throw new ArgumentException("Пропущена скобка!");

				result += stack.Pop() + " ";
			}
			return result;
		}

		private double Calculate(double leftNumber, double rightNumber, string binaryOperator)
		{
			switch (binaryOperator)
			{
				case "+":
					return leftNumber + rightNumber;
				case "-":
					return leftNumber - rightNumber;
				case "*":
					return leftNumber * rightNumber;
				case "/":
					return leftNumber / rightNumber;
				//case "^":
				//	return Math.Pow(leftNumber, rightNumber);
				default:
					return 0;
			}
		}

		public double Calculate(string example)
		{
			var operands = new Stack<double>();
			var pn = new List<string>();
			// Where() - нужен, чтобы убрать ненужные пробелы.
			pn.AddRange(GetPolishNotation(example).Split(' ').Where(p => p != ""));
			string token;

			for (int i = 0; i < pn.Count; i++)
			{
				token = pn[i];
				if (Double.TryParse(token, out double number))
				{
					operands.Push(number);
				}
				else
				{
					if (!IsBinary(token))
					{
						ChangeSign(operands, token);
					}
					else
					{
						var secondNumber = operands.Pop();
						var firstNumber = operands.Pop();

						operands.Push(Calculate(firstNumber, secondNumber, token));
					}
				}
			}
			var result = operands.Pop();
			return result;
		}

		private void ChangeSign(Stack<double> operands, string token)
		{
			if (operands is null)
			{
				throw new ArgumentNullException(nameof(operands));
			}
			if (string.IsNullOrWhiteSpace(token))
			{
				throw new ArgumentException(nameof(token));
			}

			var number = operands.Pop();
			switch (token)
			{
				case "~":
					operands.Push(-number);
					break;
				case "#":
					operands.Push(number);
					break;
				default:
					throw new Exception("Error");
			}
		}

		public double GetResult(string expression)
		{
			return Calculate(expression);
		}
	}
}
