using WpfApp1;

namespace WpfApp1
{
    public class User
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

    public Admin() : this("-1", "voidadm", false)
    {
    }

    void User2Moderator(int UserID) { }
    void Moderator2User(int UserID) { }
}

class USERS
{
    private static USERS instance;
    //Update updatebox;
    public User[] users;
    public Moderator[] moderators;
    public Admin[] admins;

    private USERS()
    {
        users = new User[10];
        moderators = new Moderator[0];
        admins = new Admin[3];
    }

    public static USERS getInstance()
    {
        if (instance == null)
            instance = new USERS();
        return instance;
    }
    public void AddUser(User user, short p)
    {
        if (p <= 0)
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].ID != "-1")
                {
                    users[i] = user;
                    return;
                }
            }
        }
        //else if(p == 1)
        //{
        //    for (int i = 0; i < moderators.Length; i++)
        //    {
        //        if (moderators[i].ID != "-1")
        //        {
        //            moderators[i] = user;
        //            return;
        //        }
        //    }
        //}
        //else {
        //    for (int i = 0; i < admins.Length; i++)
        //    {
        //        if (users[i].ID != "-1")
        //        {
        //            admins[i] = user;
        //            return;
        //        }
        //    }
        //}
    }

}