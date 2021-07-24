using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Data;
using StandardCalculator.Model;
using StandardCalculator.Views;
using System.Collections.Generic;
using System;

namespace StandardCalculator.ViewModels
{
	class MainViewModel : INotifyPropertyChanged
	{        
		private string _expression = "";

		/// <summary>
		/// true - если была ошибка при вычислении, иначе false.
		/// </summary>
		public bool IsError { get; private set; }
		public string Expression
		{
			get { return _expression; }
			set
			{
				_expression = value;
				OnPropertyChanged();
			}
		}
		private List<string> _history = new List<string>();

		public List<string> History
		{
			get { return _history; }
		}


		public ICommand AddCommand
		{
			get
			{
				return new RelayCommand(obj =>
				{
					if (obj is string token)
					{
						if (IsError)
						{
							Expression = "";
							IsError = false;
						}

						if (int.TryParse(token, out int _))
						{
							Expression += token;
						}
						else if (Expression.Length > 0)
						{
							var lastSimbol = Expression[Expression.Length - 1].ToString();
							if (SortFacility.Operators.Contains(lastSimbol) && token != ",")
							{
								if ((lastSimbol == "-") && token == "-")
								{
									Expression += "(" + token;
								}
								else
								{
									if (token != "(" && token != ")")
										Expression = Expression.Remove(Expression.Length - 1);
									Expression += token;
								}
							}
							else
							{
								if (token == ",")
								{
									TryAddComma();
								}
								else
								{
									if (lastSimbol != ",")
										Expression += token;
								}								
							}
						}
					}
				},
				obj => Expression.Length < 50);
			}
		}

		/// <summary>
		/// Пытается добавить в пример запятую.
		/// </summary>
		/// <returns> true, если установить удалось, иначе false. </returns>
		private bool TryAddComma()
		{
			var lastSimbol = Expression.Last().ToString();
			if (SortFacility.Operators.Contains(lastSimbol) || lastSimbol == "(" || lastSimbol == ")")
			{
				return false;
			}
			var operators = SortFacility.Operators.ToArray();
			var numbers = Expression.Split(operators, StringSplitOptions.RemoveEmptyEntries).ToList();
			var lastNumber = numbers.Last();
			if (lastNumber.Contains(','))
			{
				return false;
			}
			Expression += ",";
			return true;
		}

		public ICommand DeleteCommand
		{
			get
			{
				return new RelayCommand(obj =>
				{
					Expression = Expression.Remove(Expression.Length - 1);
				}, 
				obj => Expression.Length > 0);
			}
		}

		public ICommand ClearCommand
		{
			get
			{
				return new RelayCommand(obj =>
				{
					Expression = "";
				});
			}
		}

		public ICommand GetResultCommand
		{
			get
			{
				return new RelayCommand(obj =>
				{
					ICalculator calculator = new SortFacility();
					string result = "";
					try
					{
						result = Expression;
						Expression = calculator.GetResult(Expression).ToString();
						result += $" = {Expression}";
						_history.Add(result);
						IsError = false;
					}
					catch
					{
						Expression = "Ошибка!";
						IsError = true;
					}
				});
			}
		}

		public ICommand ShowAuthor
		{
			get
			{
				return new RelayCommand(obj =>
				{
					var authorWindow = new AuthorWindow();
					authorWindow.ShowDialog();
				});
			}
		}

		public ICommand ShowHistory
		{
			get
			{
				return new RelayCommand(obj =>
				{
					var historyWindow = new HistoryWindow(_history);
					historyWindow.ShowDialog();
				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
