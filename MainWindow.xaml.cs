using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    
    private string _filePath; // Путь к открытому файлу
        private string[] _pages; // Страницы из файла
        private int _currentPageIndex = 0; // Текущая страница
    
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

        }
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

        private void ToMessage(object sender, RoutedEventArgs e)
        {
			SwitchViewGrid_ToMessage();
        }
        private void ToBackMessage(object sender, RoutedEventArgs e)
        {
            Messager.Visibility = Visibility.Hidden;
        }
        private void ToBook(object sender, RoutedEventArgs e)
        {
            SwitchViewGrid_ToBook();
        }
        private void ToBackBook(object sender, RoutedEventArgs e)
        {
            PreviewBook.Visibility = Visibility.Hidden;
        }
        private void ToComment(object sender, RoutedEventArgs e)
        {
            SwitchViewGrid_ToComment();
        }
        private void ToBackComments(object sender, RoutedEventArgs e)
        {
            Comments.Visibility = Visibility.Hidden;
        }
        private void ToBackRead(object sender, RoutedEventArgs e)
        {
            Reader.Visibility = Visibility.Hidden;
        }
        private void Read(object sender, RoutedEventArgs e)
        {
            Reader.Visibility = Visibility.Visible;
        }

        private void EnableGrids(bool CollectionGrid , bool AccountGrid,bool UploadGrid, bool MessagerGrid, bool PrewBookGrid, bool CommentGrid)
		}
                BookMain.Visibility = Visibility.Visible;
                Account.Visibility = Visibility.Hidden;
                Upload.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Hidden;
                PreviewBook.Visibility = Visibility.Hidden;
                Comments.Visibility = Visibility.Hidden;

            }
			if (AccountGrid)
			{
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Visible;
                Upload.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Hidden;
                PreviewBook.Visibility = Visibility.Hidden;
                Comments.Visibility = Visibility.Hidden;
            }
			if (UploadGrid)
			{
                Upload.Visibility = Visibility.Visible;
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Hidden;
                PreviewBook.Visibility = Visibility.Hidden;
                Comments.Visibility = Visibility.Hidden;
            }
            if (MessagerGrid)
            {
                Upload.Visibility = Visibility.Hidden;
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Visible;
                PreviewBook.Visibility = Visibility.Hidden;
                Comments.Visibility = Visibility.Hidden;
            }
            if (PrewBookGrid)
            {
                Upload.Visibility = Visibility.Hidden;
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Hidden;
                PreviewBook.Visibility = Visibility.Visible;
                Comments.Visibility = Visibility.Hidden;
            }
            if (CommentGrid)
            {
                Upload.Visibility = Visibility.Hidden;
                BookMain.Visibility = Visibility.Hidden;
                Account.Visibility = Visibility.Hidden;
                Messager.Visibility = Visibility.Hidden;
                PreviewBook.Visibility = Visibility.Visible;
                Comments.Visibility = Visibility.Visible;
            }
        }
		private void SwitchViewGrid_ToMainCollection() { EnableGrids(true, false, false, false, false, false); }
		private void SwitchViewGrid_ToUserAccount() { EnableGrids(false, true, false, false, false, false); }
		private void SwitchViewGrid_ToUpload() { EnableGrids(false, false, true, false, false, false); }
        private void SwitchViewGrid_ToMessage() { EnableGrids(false, false, false, true, false, false); }
        private void SwitchViewGrid_ToBook() { EnableGrids(false, false, false, false, true, false); }
        private void SwitchViewGrid_ToComment() { EnableGrids(false, false, false, false, true, true); }
		void SwitchViewGrid_ToUserAccount() {
			UpdateUserInformatin();
			EnableGrids(false, true, false);
		}
		void UpdateUserInformatin()
		{
			User_Login.Text = Logged.Login;

			//ПОПРОБОВАТЬ сначала сделать фильтры книг с помощью checkboxов, тк всё может пойти по откосу

        // Открытие файла
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите книгу"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                LoadBook(_filePath);
            }
        }

        // Загрузка книги
        private void LoadBook(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                _pages = SplitIntoPages(content); // Разбиваем текст на страницы
                _currentPageIndex = 0; // Начинаем с первой страницы
                DisplayPage(_currentPageIndex);

                btnPreviousPage.IsEnabled = false; // Кнопка "Назад" недоступна на первой странице
                btnNextPage.IsEnabled = _pages.Length > 1; // Кнопка "Вперед" доступна, если есть следующая страница
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Разбиение текста на страницы
        private string[] SplitIntoPages(string content)
        {
            const int pageSize = 2000; // Размер страницы (в символах)
            return content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                          .Aggregate(new System.Text.StringBuilder(), (sb, line) => sb.AppendLine(line))
                          .ToString()
                          .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                          .Chunk(pageSize)
                          .Select(chunk => string.Join(Environment.NewLine, chunk))
                          .ToArray();
        }

        // Отображение страницы
        private void DisplayPage(int pageIndex)
        {
            txtContent.Text = _pages[pageIndex];
        }

        // Кнопка "Предыдущая страница"
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                DisplayPage(_currentPageIndex);

                btnNextPage.IsEnabled = true;
                btnPreviousPage.IsEnabled = _currentPageIndex > 0;
            }
        }

        // Кнопка "Следующая страница"
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPageIndex < _pages.Length - 1)
            {
                _currentPageIndex++;
                DisplayPage(_currentPageIndex);

                btnPreviousPage.IsEnabled = true;
                btnNextPage.IsEnabled = _currentPageIndex < _pages.Length - 1;
            }
        }

    }

    // Расширение для разбиения массива на части
    public static class ArrayExtensions
    {
        public static T[][] Chunk<T>(this T[] array, int size)
        {
            var chunks = new T[(int)Math.Ceiling((double)array.Length / size)][];
            for (int i = 0, j = 0; i < array.Length; i += size, j++)
            {
                chunks[j] = array.Skip(i).Take(size).ToArray();
            }
            return chunks;
        }
    }

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