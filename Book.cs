using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    class BOOKS
    {
        private static BOOKS instance;
        //Update updatebox;
        private Book[] Books;
        public BOOKS2G B2G = BOOKS2G.getInstance();
        static Book empty = new Book();
        public void AddBook(ref Book book)
        {
            var b = Books.Append(book);
            Books = b.ToArray();
        }
        public void DeleteBook(ref int BookID) { }
        private BOOKS()
        {
            Books = new Book[0];
        }
        public static BOOKS getInstance()
        {
            if (instance == null)
                instance = new BOOKS();
            return instance;
        }
        public void ShowBooks(int Maxcount = 10)
        {
            string toShow = "";
            toShow += $"{Books.Length}\n";
            int i = 0;
            foreach (Book book in Books)
            {
                if (i == Maxcount) break;
                toShow += "\t" + book.ToString() + "\n";
                i++;
            }
            MessageBox.Show(toShow);
        }

        public void SetBooksBySql(Book[] booksNew)
        {
            Books = booksNew;
        }

        public DataView SimpleFillDataGrid()
        {
            if (Books == null) return null;
            try
            {
                DataTable dt = new DataTable("Books");
                dt.Columns.Add(new DataColumn("Название", typeof(string)));
                dt.Columns.Add(new DataColumn("Автор", typeof(string)));
                for (var i = 0; i < Books.Length; i++)
                {
                    DataRow r = dt.NewRow();
                    //DataGridCell BookName = new DataGridCell() { Content = new TextBlock { Text = Books[i].Name }, Name = $"B{Books[i].ID}" };
                    //DataGridCell AuthorName = new DataGridCell() { Content = new TextBlock { Text = Books[i].AuthorName } };// добавить таблицу с Авторами
                    r[0] = Books[i].Name;
                    r[1] = Books[i].AuthorName;
                    dt.Rows.Add(r);
                }
                return dt.DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), e.ToString());
            }
            MessageBox.Show("Ошибка заполнения Datagrid");
            return null;
        }
        public DataView SimpleFillDataGridByGenre(List<string> genres)
        {
            if (Books == null) return null;
            if (genres.Count == 0) return SimpleFillDataGrid();
            List<string> BooksId = new List<string>(0);
            foreach (var g in genres)
            {
                BooksId = BooksId.Union(B2G.GetBooksIdByGenreId(g)).ToList();
            }
            try
            {
                DataTable dt = new DataTable("Books");
                dt.Columns.Add(new DataColumn("Название", typeof(string)));
                dt.Columns.Add(new DataColumn("Автор", typeof(string)));
                for (var i = 0; i < Books.Length; i++)
                {
                    if (!BooksId.Contains(Books[i].ID)) continue;
                    DataRow r = dt.NewRow();
                    //DataGridCell BookName = new DataGridCell() { Content = new TextBlock { Text = Books[i].Name }, Name = $"B{Books[i].ID}" };
                    //DataGridCell AuthorName = new DataGridCell() { Content = new TextBlock { Text = Books[i].AuthorName } };// добавить таблицу с Авторами
                    r[0] = Books[i].Name;
                    r[1] = Books[i].AuthorName;
                    dt.Rows.Add(r);
                }
                return dt.DefaultView;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), e.ToString());
            }
            MessageBox.Show("Ошибка заполнения Datagrid");
            return null;
        }

        public Book GetbookByName(string name)
        {
            foreach (Book item in Books)
            {
                if (item.Name == name || item.AuthorName == name) return item;
            }
            return null;
        }

        private ref Book GetBookByID(string ID)
        {
            //if (Books?[Convert.ToInt64(ID)] != null)
            //{
            //	if (Books?[Convert.ToInt64(ID)].ID == ID)
            //	{
            //		return ref Books[Convert.ToInt64(ID)];
            //	}
            //}
            for (int i = 0; i < Books.Length; i++)
            {
                if (Books[i].ID == ID) { return ref Books[i]; }
            }
            return ref empty;
        }

        public ref Book this[string ID]
        {
            get => ref GetBookByID(ID);
        }
        public ref Book this[int ID]
        {
            get => ref GetBookByID(ID.ToString());
        }
    }

    class BOOKS2G
    {
        private static BOOKS2G instance;
        //public List<string> G2B;//<BookID, GenreID>
        public string[][] G2B;

        public void SetBySql(string[][] newG2B)
        {
            G2B = newG2B;
        }

        private BOOKS2G()
        {
            G2B = new string[0][];
        }

        public static BOOKS2G getInstance()
        {
            if (instance == null)
                instance = new BOOKS2G();
            return instance;
        }

        public List<string> GetBooksIdByGenreId(string Id)
        {
            List<string> BooksId = new List<string>(0);

            foreach (var we in G2B)
            {
                if (we[1] == Id)
                {
                    BooksId.Add(we[0]);
                }
            }

            return BooksId;
        }

        public void shout()
        {
            string tO = "";
            foreach (string[] we in G2B)
            {
                tO += we[0] + "_" +  we[1] + "\n";
            }
            MessageBox.Show(tO);
        }

        public void AddBook2genre(string BookId, string GenreId)
        {
            G2B = new string[1][];
            G2B[0] = new string[] { BookId, GenreId };

        }

    }

    class Book
    {
        private string id;

        public BitmapImage image = new BitmapImage() { UriSource = new Uri(@"/llm-ops-language-model23.png", UriKind.Relative) };
        public Style style;

        public string ID
        {
            get { return id; }
            private set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string authorname;

        public string AuthorName
        {
            get { return authorname; }
            set { authorname = value; }
        }


        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private DateTime dateofupload;

        public DateTime DateOfUpload
        {
            get { return dateofupload; }
            set { dateofupload = value; }
        }

        private string filepath;

        public Book(string iD, string name, string Author, string description, string dateOfUpload, string filePath)
        {
            ID = iD;
            AuthorName = Author;
            Name = name;
            Description = description;
            DateTime ret;
            DateTime.TryParse(dateOfUpload, out ret);
            DateOfUpload = ret;
            FilePath = filePath;
            style = new Style();
            style.Setters.Add(new Setter { Property = System.Windows.Controls.Image.StretchProperty, Value = Stretch.Uniform });
            style.Setters.Add(new Setter { Property = System.Windows.Controls.Image.FocusableProperty, Value = false });
            style.Setters.Add(new Setter { Property = System.Windows.Controls.Image.SourceProperty, Value = new BitmapImage() { UriSource = new Uri(@"/llm-ops-language-model23.png", UriKind.Relative) } });

        }
        public Book() : this("-1", "Void", "Noname", "Empty class", "2010-4-11", "/") { }

        public Book(string iD, string name, string Author) : this(iD, name, Author, "", "", "") { }

        public string FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }
        public float GetAVGEstimation() { return 0; }
        public int GetCountEstimation() { return 0; }

        public override string ToString()
        {
            string Tostr = "";
            Tostr += ID + ". ";
            Tostr += Name + ". ";
            return base.ToString();
        }
    }

}
