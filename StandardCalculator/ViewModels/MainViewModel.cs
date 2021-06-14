using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;

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
                    if (obj is string str)
					{
                        if (str != "." || str == "." && Expression.Count(c => c == '.') == 0)
						{
                            Expression += str;
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

        //public ICommand GetResultCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(obj =>
        //        {
        //            Expression = "";
        //        });
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
