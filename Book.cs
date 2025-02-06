using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp1
{

    class BOOKS
{
    private static BOOKS instance;
    //Update updatebox;
    public Book[] Books;
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
    //Update updatebox;
    public Dictionary<string, string> G2B;//<BookID, GenreID>
    public void AddBook2genre(string BookID, string GenreID)
    {
        G2B.Add(BookID, GenreID);
    }

    public void DeleteGenre(int GenreID) { }
    private BOOKS2G()
    {
        G2B = new Dictionary<string, string>(0);
    }

    public static BOOKS2G getInstance()
    {
        if (instance == null)
            instance = new BOOKS2G();
        return instance;
    }
}

class Book
{
    private string id;


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

    public Book(string iD, string name, string description, DateTime dateOfUpload, string filePath)
    {
        ID = iD;
        Name = name;
        Description = description;
        DateOfUpload = dateOfUpload;
        FilePath = filePath;
    }
    public Book() : this("-1", "Void", "Empty class", new DateTime(2010, 4, 11), "/") { }

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
