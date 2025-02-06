using WpfApp1;

namespace WpfApp1
{
    class User
    {
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        private bool status;
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        private string[] messanges;
        public string[] Messanges
        {
            get { return messanges; }
            set { messanges = value; }
        }
        string[] PersonallyLoadedBooks;
        string[] PersonalLibrary;
        public User(string iD, string login, bool status)
        {
            ID = iD;
            Login = login;
            Status = status;
            Messanges = new string[0];
            PersonalLibrary = new string[0];
            PersonallyLoadedBooks = new string[0];
        }

        public User() : this("-1", "VoidUser", false) { }

        public void ShowMessages() { }
        public void DeleteAllMessages() { }
        public void DeleteMessage(int id) { }
        public void PersonallyBookLoad() { }
        public void PersonallyBookDelete(int BookID) { }
        public void AddBook2PersonalLibrary(int BookID) { }
        public void DeleteBookFromPersonalLibrary(int BookID) { }
        public void ShareBook(int BookID) { }
        public void WriteComment(int BookID) { }
    }
}
class Moderator : User
{
    public Moderator(string iD, string login, bool status) : base(iD, login, status)
    {
    }

    public Moderator() : this("-1", "voidmod", false)
    {
    }

    public void BanUser(int UserID) { }
    public void UnBanUser(int UserID) { }
    public void WarnUser(int UserID, string WarnText) { }
    public void DeleteBook(int BookID) { }

}
class Admin : Moderator
{
    public Admin(string iD, string login, bool status) : base(iD, login, status)
    {
    }

    public Admin() : this("-1", "voidmod", false)
    {
    }

    void User2Moderator(int UserID) { }
    void Moderator2User(int UserID) { }
}