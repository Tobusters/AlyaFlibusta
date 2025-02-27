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
using System.Xml.Linq;

namespace WpfApp1
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 
	delegate void Update();
	public partial class MainWindow : Window
	{
		#region Инициализация
		volatile static bool _Is_Logged = false;
		volatile static User Logged = new User();
		BOOKS books = BOOKS.getInstance();
		GENRES genres = GENRES.getInstance();
		USERS users = USERS.getInstance();
		List<string> SelectedGenreId = new List<string>();
        Book SelectedBook;
        //SQLConnection conn = SQLConnection.getInstance(@"Server=DESKTOP-UNTJG88\SQLEXPRESS;database=AlyaFlibusta;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");//под ето отдельный поток нужно кидать
        SQLConnection conn = SQLConnection.getInstance();//под ето отдельный поток нужно кидать
		//SQLConnection conn = SQLConnection.getInstance(@"Server=DESKTOP-CVTHJDK\SQLEXPRESS;database=AlyaFlibusta2;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");//под ето отдельный поток нужно кидать
		RegLog reglog = new RegLog(); 
		#endregion

		public MainWindow()
		{
			var result = MessageBox.Show("Загрузить с sql?", "SQL", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				try
				{
					if (conn.Conn != null)
					{
						MessageBox.Show("Успешное подключение!", "Статус подключения", MessageBoxButton.OK);
						conn.CheckConn();
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
			}
			else
			{
				
			}

			InitializeComponent();

			try { 
			genres.SetGenres(conn.ConnectToDTBaseAndReadDictionary("exec ShowGenre"));
			books.SetBooksBySql(conn.ConnectToDTBaseAndReadBooks("exec ShowSimpleBooksForViewTable"));
                //books2G.SetBySql(conn.ConnectToDTBaseAndReadG2B("exec ShowGenre2Book"));
                books.B2G.AddBook2genre("1053", "1075");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, e.ToString());
			}

			ExpandGenresUpdate();
			BooksGridUpdate();
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

		
		private void CollectionBooksViewTable_MouseUp(object sender, MouseButtonEventArgs e)
		{
			try
            {
                DataGridCell dt = (DataGridCell)sender;
                {
                    TextBlock dti = (TextBlock)dt.Content;
                    //MessageBox.Show(dti.Text);
                    SelectedBook = books.GetbookByName(dti.Text);
                    MessageBox.Show(SelectedBook.ID);
                    ToreadImage.Style = SelectedBook.style;
                    ToreadImage.UpdateLayout();

                }
            }

            catch { }
		}


		public void ExpandGenresUpdate()
		{
			if (GenreSelect != null)
			{ 
				GenreSelect.Content = null;
			}
			void Button_Click(object sender, RoutedEventArgs e)
			{
				CheckBox ch = (CheckBox)sender;
				//if(ch.IsChecked == false) return;
				//MessageBox.Show(ch.Name);

				if (ch.IsChecked == true)
				{
					SelectedGenreId.Add(ch.Name.Substring(1));
				}
				else
				{
					SelectedGenreId.Remove(ch.Name.Substring(1));
				}
				//string tO = "";
				//foreach(var g in SelectedGenreId)
				//{
				//	tO += g + "/n";
				//}
				//MessageBox.Show(tO);
				BooksGridUpdate();


			}
			Dictionary<string, string> list = genres.GetGenresDict();
				if (list == null) return;
				ScrollViewer scrollViewer = new ScrollViewer();
				StackPanel stackPanel = new StackPanel();
				Style buttonStyle = new Style {};
				EventSetter clickEventSetter = new EventSetter(CheckBox.ClickEvent, new RoutedEventHandler(Button_Click));
				buttonStyle.Setters.Add(clickEventSetter);
				stackPanel.Children.Add(new TextBox { Name = "FilterGenre", HorizontalAlignment = HorizontalAlignment.Left, MinWidth = 50 });
				foreach (var Genres in list) { 
					stackPanel.Children.Add(new CheckBox { Content = Genres.Value, Name = 'G' + Genres.Key, Template = (ControlTemplate)this.FindResource("CustomCheckBoxes"), Style = buttonStyle});
				}
				scrollViewer.Content = stackPanel;
				scrollViewer.MaxHeight = 150;
				GenreSelect.Content = null ;
				GenreSelect.Content = scrollViewer;
			
		}

		public void BooksGridUpdate()
		{
			CollectionBooksViewTable.ItemsSource = null;
            CollectionBooksViewTable.ItemsSource = books.SimpleFillDataGridByGenre(SelectedGenreId);
        }
  
      #region Вкладки
        void ToUserAccount(object sender, RoutedEventArgs e)
		{
			if (_Is_Logged)
			{
				SwitchViewGrid_ToUserAccount();
			}
			else
			{
				reglog.SetRef(ref conn.GetRef());
				reglog.Owner = this;
				reglog.ShowDialog();
				Logged = reglog.GetLog();
				_Is_Logged = reglog.Is_Logged;
				reglog.Close();
				reglog = null;
				SwitchViewGrid_ToUserAccount();
			}
		}
		void ToMainCollection(object sender, RoutedEventArgs e)
		{
			SwitchViewGrid_ToMainCollection();
		}
		void ToUpload(object sender, RoutedEventArgs e)
		{
			SwitchViewGrid_ToUpload();
		}

		void EnableGrids(bool CollectionGrid, bool AccountGrid, bool UploadGrid)
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
		void SwitchViewGrid_ToMainCollection() { EnableGrids(true, false, false); }
		void SwitchViewGrid_ToUserAccount() {
			UpdateUserInformatin();
			EnableGrids(false, true, false);
		}
		void UpdateUserInformatin()
		{
			User_Login.Text = Logged.Login;

			//ПОПРОБОВАТЬ сначала сделать фильтры книг с помощью checkboxов, тк всё может пойти по откосу



			//ScrollViewer scrollViewer = new ScrollViewer();
			//StackPanel stackPanel = new StackPanel();
			//foreach (var Genres in list)
			//{
			//    stackPanel.Children.Add(new CheckBox { Content = Genres.Value, Name = 'G' + Genres.Key, Template = (ControlTemplate)this.FindResource("CustomCheckBoxes") });
			//}
			//scrollViewer.Content = stackPanel;
			//scrollViewer.MaxHeight = 150;
			//User_UploadedBooksCollection.Content = null;
		}
		void SwitchViewGrid_ToUpload() { EnableGrids(false, false, true); }
		#endregion

		#region Closing
		void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			reglog.Close();
			conn.Close();
		}

		void Window_Closing(object sender, EventArgs e)
		{
			reglog.Close();
			conn.Close();
		}
	}
}

#endregion