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

		public double GetResult(string expression)
		{
			return CalculateExample(expression);
		}

		public double CalculateExample(string example)
		{
			var operands = new Stack<double>();
			var pn = GetPolishNotation(example).Split(' ');

			for (int i = 0; i < pn.Length; i++)
			{
				if (Double.TryParse((pn[i]), out double number))
				{
					operands.Push(number);
				}
				else
				{
					// TODO: Написать обработчик.
					if (operands.Count == 1)
					{

					}
					var secondNumber = operands.Pop();
					var firstNumber = operands.Pop();

				}
			}
			var result = operands.Pop();
			return result;
		}
	}
}
