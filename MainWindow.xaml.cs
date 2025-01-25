using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 
	delegate void Update();
	public partial class MainWindow : Window
	{
		Book testBook1 = new Book("0", "Принцесса Марса", "Джон Картер на Марсе", new DateTime(1911, 1, 1), "");
		BOOKS books = BOOKS.getInstance();
		GENRES genres = GENRES.getInstance();
		BOOKS2G Books2G = BOOKS2G.getInstance();
		public MainWindow()
		{
			InitializeComponent();
			//RegLog regLog = new RegLog();
			//regLog.ShowDialog();
			books.AddBook(ref testBook1);
			genres.AddGenre("0", "Science Fantasy");
			genres.AddGenre("1", "Fantasy");
			genres.AddGenre("2", "Science Fiction");
			genres.AddGenre("3", "Science Trust");
			genres.AddGenre("4", "Science Trust");
			genres.AddGenre("5", "Science Trust");
			genres.AddGenre("6", "Science Trust");
			genres.AddGenre("7", "Science Trust");
			genres.AddGenre("8", "Science Trust");
			genres.AddGenre("9", "Science Trust");
			genres.AddGenre("10", "Science Trust");
			genres.AddGenre("11", "Science Trust");
			genres.AddGenre("12", "Science Trust");
			genres.AddGenre("13", "Science Trust");
			genres.AddGenre("14", "Science Trust");
			genres.AddGenre("15", "Science Trust");
			genres.AddGenre("16", "Science Trust");
			genres.AddGenre("17", "Science Trust");
			genres.AddGenre("18", "Science Trust");
			genres.AddGenre("19", "Science Trust");
			Books2G.AddBook2genre(books[0].ID, genres[0]);
			ExpandGenresUpdate();
			//UpdateComboBox(GenreSelect);
		}
		public void UpdateComboBox(params ComboBox[] comboBoxes)
		{
			if (comboBoxes.Length == 0)
			{
				return;
			}
			else
			{
				foreach (ComboBox comboBox in comboBoxes)
				{

				}
				return;
			}
		}
		public void ExpandGenresUpdate()
		{
			if (GenreSelect != null)
			{
				//MessageBox.Show("sdsd");
				string[] list = genres.GetGenresNames();
				ScrollViewer scrollViewer = new ScrollViewer();
				StackPanel stackPanel = new StackPanel();
				for (int i = 0; i < list.Length; i++)
				{
					stackPanel.Children.Add(new CheckBox { Content = list[i], Name = list[i].Replace(" ", "") });
				}
				scrollViewer.Content = stackPanel;
				scrollViewer.MaxHeight = 150;
				GenreSelect.Content = scrollViewer;
				//GenreSelect.Content;
			}
		}

		private void ToUserAccount(object sender, RoutedEventArgs e)
		{
			//Проверка на логин

			SwitchViewGrid_ToUserAccount();
        }

		private void ToMainCollection(object sender, RoutedEventArgs e)
		{
			SwitchViewGrid_ToMainCollection();
        }

		private void ToUpload(object sender, RoutedEventArgs e)
		{
			SwitchViewGrid_ToUpload();
        }
		private void EnableGrids(bool CollectionGrid , bool AccountGrid,bool UploadGrid)
		{
			//if (CollectionGrid == AccountGrid == UploadGrid && CollectionGrid == true)return;	//Так не должно быть
			//if (CollectionGrid == AccountGrid == UploadGrid && CollectionGrid == false)return;//Так не должно быть
			if (CollectionGrid == AccountGrid && CollectionGrid == true) return;           //Так не должно быть
			if (CollectionGrid == UploadGrid && CollectionGrid == true) return;         //Так не должно быть
			if (AccountGrid == UploadGrid && AccountGrid == true) return;               //Так не должно быть
			if (CollectionGrid)
			{
                BookMain.Visibility = Visibility.Visible;
                Account.Visibility = Visibility.Hidden;
                Upload.Visibility = Visibility.Hidden;
            }
			if (AccountGrid)
			{
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Visible;
                Upload.Visibility = Visibility.Hidden;
            }
			if (UploadGrid)
			{
                Upload.Visibility = Visibility.Visible;
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Hidden;
            }
		}
		private void SwitchViewGrid_ToMainCollection() { EnableGrids(true, false, false); }
		private void SwitchViewGrid_ToUserAccount() { EnableGrids(false, true, false); }
		private void SwitchViewGrid_ToUpload() { EnableGrids(false, false, true); }

    }
}
