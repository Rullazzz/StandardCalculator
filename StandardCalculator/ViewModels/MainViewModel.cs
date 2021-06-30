﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Data;
using StandardCalculator.Model;

namespace StandardCalculator.ViewModels
{
	class MainViewModel : INotifyPropertyChanged
    {        
        private string _expression = "";
		public string Expression
		{
			get { return _expression; }
			set
			{
                _expression = value;
                OnPropertyChanged();
            }
		}

		public ICommand AddCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (obj is string token)
					{
						// TODO: Написать проверку добавления запятой.
						if (int.TryParse(token, out int _))
						{
							Expression += token;
						}
						else if (Expression.Length > 0)
						{
                            if (SortFacility.Operators.Contains(Expression[Expression.Length - 1].ToString()))
							{
                                if ((Expression[Expression.Length - 1] == '-') && token == "-")
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
                                Expression += token;
                            }
						}
					}
                },
                obj => Expression.Length < 50);
            }
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
                    try
					{
                        Expression = calculator.GetResult(Expression).ToString();
                    }
                    catch
					{
                        Expression = "Ошибка!";
					}
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
