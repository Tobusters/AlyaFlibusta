using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
		SQLConnection conn = SQLConnection.getInstance(@"Server=DESKTOP-UNTJG88\SQLEXPRESS;database=AlyaFlibusta;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");//под ето отдельный поток нужно кидать
		//SQLConnection conn = SQLConnection.getInstance();//под ето отдельный поток нужно кидать
		public MainWindow()
		{
			var result = MessageBox.Show("Загрузить с sql?", "SQL", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {
                try
                {
                    if (conn.Conn != null)
                    {
                        MessageBox.Show("Успешное подключение!", "Статус подключения", MessageBoxButton.OK);
                        conn.Conn.Open();
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message, e.ToString(), MessageBoxButton.OK);
                }
                finally
                {
                    conn.Conn.Close();
                }
				genres.SetGenres(conn.ConnectToDTBaseAndRead("exec ShowGenre"), ref conn);
            }

			InitializeComponent();

			//RegLog regLog = new RegLog();
			//regLog.ShowDialog();
			books.AddBook(ref testBook1);
			//Books2G.AddBook2genre(books[0].ID, genres[0]);
			ExpandGenresUpdate();
			CollectionBooksViewTable.ItemsSource = conn.ConnectToDTBaseAndFillDataGrid("exec ShowSimpleBooksForViewTable");
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
				GenreSelect.Content = null;
			}
                ////MessageBox.Show("sdsd");
                //string[] list = genres.GetGenresNames();
                //string[] listID = genres.GetGenresID();
                //ScrollViewer scrollViewer = new ScrollViewer();
                //StackPanel stackPanel = new StackPanel();
                //for (int i = 0; i < list.Length; i++)
                //{
                //	stackPanel.Children.Add(new CheckBox { Content = list[i], Name = listID[i]});
                //}
                //scrollViewer.Content = stackPanel;
                //scrollViewer.MaxHeight = 150;
                //GenreSelect.Content = scrollViewer;
                ////GenreSelect.Content;
                ///


                Dictionary<string, string> list = genres.GetGenresDict();
				ScrollViewer scrollViewer = new ScrollViewer();
				StackPanel stackPanel = new StackPanel();
				stackPanel.Children.Add(new TextBox { Name = "FilterGenre", HorizontalAlignment = HorizontalAlignment.Left, MinWidth = 50 });
				foreach (var Genres in list) { 
					stackPanel.Children.Add(new CheckBox { Content = Genres.Value, Name = 'G' + Genres.Key, Template = (ControlTemplate)this.FindResource("CustomCheckBoxes") });
				}
				scrollViewer.Content = stackPanel;
				scrollViewer.MaxHeight = 150;
				GenreSelect.Content = scrollViewer;
			
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
