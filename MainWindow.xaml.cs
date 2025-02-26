using Microsoft.Win32;
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

        private string _filePath; // Путь к открытому файлу
        private string[] _pages; // Страницы из файла
        private int _currentPageIndex = 0; // Текущая страница


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
            //books.AddBook(ref testBook1);
            //Books2G.AddBook2genre(books[0].ID, genres[0]);
            //ExpandGenresUpdate();
            //CollectionBooksViewTable.ItemsSource = conn.ConnectToDTBaseAndFillDataGrid("exec ShowSimpleBooksForViewTable");
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
}
