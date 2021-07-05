using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StandardCalculator.Views
{
	/// <summary>
	/// Логика взаимодействия для HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
	{
		public readonly List<string> History;

		public HistoryWindow()
		{
			InitializeComponent();
		}

		public HistoryWindow(List<string> history) : this()
		{
			History = history;
		}

		private void HistoryList_Loaded(object sender, RoutedEventArgs e)
		{
			if (sender is ListBox historyList)
			{
				historyList.ItemsSource = History;
			}
		}
	}
}
