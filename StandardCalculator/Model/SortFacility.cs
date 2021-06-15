using System;
using System.Collections.Generic;
using System.Linq;

namespace StandardCalculator.Model
{
	public class SortFacility : ICalculator
	{
		private List<string> Operators;

		public SortFacility()
		{
			Operators = new List<string>()
			{
				"+",
				"-",
				"x",
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
				case "x":
				case "/":
					return 2;
				case "um": // Унарный минус
				case "up": // Унарный плюс
 					return 3;
				default:
					throw new ArgumentException(op);
			}
		}

		private int GetAssociativity(string expression, int counter)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException(nameof(expression));
			}
			if (counter < 0)
			{
				throw new ArgumentException(nameof(counter));
			}

			//TODO: Сделать проверку на входящий символ.

			if (counter == 0)
			{
				return 0; // Правоасоциативный
			}
			if (int.TryParse(expression[counter - 1].ToString(), out int _))
			{
				return 1; // Левоасоциативный
			}
			return 0;
		}

		public string GetPolishNotation(string expression)
		{
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
						{
							result += expression[j];
						}
						else
						{
							break;
						}
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
					if (stack.Count > 0 && stack.Peek() != "(" && stack.Peek() != ")")
					{
						while (GetPriority(stack.Peek()) > GetPriority(expression[i].ToString())
						|| (GetPriority(stack.Peek()) == GetPriority(expression[i].ToString())) && (GetAssociativity(expression, i) == 1))
						{
							result += stack.Pop() + " ";
							if (stack.Count == 0)
							{
								break;
							}
						}
					}
					stack.Push(expression[i].ToString());
					continue;
				}
				if (expression[i] == '(')
				{
					stack.Push("(");
				}
				if (expression[i] == ')')
				{
					while (stack.Peek() != "(")
					{
						if (stack.Count == 1)
						{
							throw new ArgumentException("Пропущена скобка!");
						}
						result += stack.Pop() + " ";
					}
					stack.Pop(); // Выкидываем закрывающую скобку.
				}
			}
			while (stack.Count > 0)
			{
				if (stack.Peek() == "(" || stack.Peek() == ")")
				{
					throw new ArgumentException("Пропущена скобка!");
				}
				result += stack.Pop() + " ";
			}
			return result;
		}


		public double GetResult(string expression)
		{
			GetPolishNotation(expression);
			return 0;
		}

		//public static double CalculateExample(string example)
		//{
		//	var operands = new Stack<double>();
		//	var pn = GetPolishNatation(example).Split(' ');

		//	for (int i = 0; i < pn.Length; i++)
		//	{
		//		if (Double.TryParse((pn[i]), out double number))
		//			operands.Push(number);
		//		else if (_binaryOperators.Contains(pn[i]))
		//		{
		//			var firstNumber = operands.Pop();
		//			var secondNumber = operands.Pop();
		//			operands.Push(Calculate(firstNumber, secondNumber, pn[i]));
		//		}
		//		else if (_unaryOperators.Contains(pn[i]))
		//		{
		//			var firstNumber = operands.Pop();
		//			operands.Push(Calculate(firstNumber, pn[i]));
		//		}
		//	}
		//	var result = operands.Pop();
		//	return result;
		//}
	}
}
