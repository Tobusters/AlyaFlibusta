using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;

namespace WpfApp1
{
    class SQLConnection
    {
        private static SQLConnection instance;
        static string serverName = $@".\SQLEXPRESS";
        string databaseName = "AlyaFlibusta";
        private SqlConnection connection;
        SqlDataReader rdr;
        public static bool IsConnected { get; private set; } = false;
        public bool CheckConn()
        {
            try
            {
                Conn.Open();
                IsConnected = true;
                return true;
            }
            catch (SqlException e)
            {
                IsConnected = false;
                MessageBox.Show(e.Message);
                return false;
            }
            finally { Conn.Close(); }
        }
        public SqlConnection Conn
        {
            get { return connection; }
            private set { connection = value; }
        }

        public void CreatenFillDT()
        {
            if (CheckConn())
            {
                try
                {
                    Conn.Open();
                    string cmd = $@"USE master; GO IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{databaseName}')begin;CREATE DATABASE '{databaseName}'";
                    ConnectToDTBaseAndQuery(cmd);
                    MessageBox.Show($"База данных '{databaseName}' успешно создана на сервере.");
                    cmd = "USE AlyaFlibusta; GO IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Genre') AND type in (N'U'))BEGINcreate table Genre(Id int identity(1, 1) primary key,GenreName varchar(50) not null,);END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Author') AND type in (N'U'))BEGINcreate table Author(Id int identity(1, 1) primary key,FirstName varchar(50) not null,LastName varchar(50) not null,);END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'BOOKS') AND type in (N'U'))BEGINCREATE TABLE BOOKS(   Id int identity(1, 1) primary key,  BookName nvarchar(100) not null,  AuthorId int references Author(Id) not null,  BookDescription varchar(1000) not null,  DateOfUpload datetime not null,  Filepath varchar(200) not null,  BookIMG varchar(200)  );END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'G2B') AND type in (N'U'))BEGIN  create table G2B(  BookID int references BOOKS(Id)not null,  GenreID int references Genre(Id)not null,  );END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'USERS') AND type in (N'U'))BEGIN  CREATE TABLE USERS  (  Id int identity(1, 1) primary key,  IsBanned bit not null default 0 check(IsBanned >=0 and IsBanned <= 1),  UserStatus bit not null default 0 check(UserStatus >=0 and UserStatus <= 2),  Login nvarchar(100) not null unique,  Email varchar(100) not null,  Password varchar(256) not null,  NickName nvarchar(50),  AvatarIMG varchar(200)  );END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'COMMENTS') AND type in (N'U'))BEGINCREATE TABLE COMMENTS (  Id int identity(1, 1) primary key,  UserID int references USERS(Id)not null,  BookID int references BOOKS(Id)not null,  WrittenDate date not null,   TextOf nvarchar(1000)not null   );END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Estimation') AND type in (N'U'))BEGIN  CREATE TABLE Estimation (  UserId int references USERS(id) not null,  BookID int references BOOKS(id) not null,   Est float not null,  );END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'PersonalLibrary') AND type in (N'U'))BEGIN  CREATE TABLE PersonalLibrary (  UserID int references USERS(Id) not null,  BookID int references BOOKS(id) not null,);END;IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'PersonallyLoadedBooks') AND type in (N'U'))BEGIN  CREATE TABLE PersonallyLoadedBooks(   UserID int references USERS(Id)not null,  BookID int references BOOKS(id) not null,);END;\r\n";
                    ConnectToDTBaseAndQuery(cmd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Ошибка при создании базы данных");
                }
            }
        }
        private void ReconnectSql()
        {
            Conn.Close();
            Conn.ChangeDatabase($@"Server={serverName};database={databaseName};Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");
        }

        public ref SqlConnection GetRef()
        {
            return ref connection;
        }
        private SQLConnection()
        {
            Conn = new SqlConnection(@"Server=DESKTOP-QVUI8Q3;database=AlyaFlubusta;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");//дом
            rdr = null;
            IsConnected = true;
        }
        private SQLConnection(string coon)
        {
            Conn = new SqlConnection(coon);
            rdr = null;
            string cmd = Conn.ConnectionString;
            //CreatenFillDT();
            //ReconnectSql();
            IsConnected = true;
        }
        public static SQLConnection getInstance()
        {
            if (instance == null)
                instance = new SQLConnection();
            return instance;
        }
        public static SQLConnection getInstance(string coon)
        {
            if (instance == null)
                instance = new SQLConnection(coon);
            return instance;
        }
        private SqlCommand TryConnectionAndQueryBody(string select)
        {
            if (!IsConnected) return null;
            SqlCommand sqlCommand = new SqlCommand(select, Conn);
            Conn.Open();
            sqlCommand.ExecuteNonQuery();
            return sqlCommand;
        }
        private void FinallyBody()
        {
            if (rdr != null)
            {
                rdr.Close();
            }
            if (Conn != null)
            {
                Conn.Close();
            }
        }
        public void Close()
        {
            if (rdr != null)
            {
                rdr.Close();
            }
            if (Conn != null)
            {
                Conn.Close();
            }
        }
        public void ConnectToDTBaseAndQuery(string select)//without Select
        {
            try
            {
                _ = TryConnectionAndQueryBody(select);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message.ToString(), e.ToString());
            }
            finally
            {
                FinallyBody();
            }
        }
        private void ConnectToDTBaseAndRead(string select)
        {
            if (!IsConnected) return;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(select, Conn);//Запрос
                Conn.Open();//Открываем подключение
                rdr = sqlCommand.ExecuteReader();
            }
            catch (SqlException e)
            {
                MessageBox.Show(select, e.ToString());
            }
        }
        public Dictionary<string, string> ConnectToDTBaseAndReadDictionary(string select)
        {
            ConnectToDTBaseAndRead(select);
            if (rdr != null)
            {
                try
                {
                    Dictionary<string, string> Dict = new Dictionary<string, string>();
                    while (rdr.Read())
                    {
                        Dict.Add(rdr[0].ToString(), rdr[1].ToString());
                    }
                    return Dict;
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message.ToString(), e.ToString());
                }
                finally
                {
                    FinallyBody();
                }
            }
            return null;
        }
        public Book[] ConnectToDTBaseAndReadBooks(string select)
        {
            ConnectToDTBaseAndRead(select);
            if (rdr != null)
            {
                try
                {
                    List<Book> list = new List<Book>();
                    while (rdr.Read())
                    {
                        list.Add(new Book(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString()));
                    }
                    return list.ToArray();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message.ToString(), e.ToString());
                }
                finally
                {
                    FinallyBody();
                }
            }
            return null;
        }

        public string[][] ConnectToDTBaseAndReadG2B(string select)
        {
            ConnectToDTBaseAndRead("exec ShowGenre2BookCount");
            int lnth = 0;
            if (rdr != null)
            {
                try
                {
                    while (rdr.Read())
                    {
                        lnth = Convert.ToInt32(rdr[0].ToString());
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message.ToString(), e.ToString());
                }
                finally
                {
                    FinallyBody();
                }
            }
            ConnectToDTBaseAndRead(select);
            if (rdr != null)
            {
                try
                {
                    string[][] list = new string[lnth][];
                    int i = 0;
                    while (rdr.Read())
                    {
                        list[i] = new string[] { "", "" };
                        list[i][0] = rdr[0].ToString();
                        list[i][1] = rdr[1].ToString();
                        i++;
                    }
                    return list;
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message.ToString(), e.ToString());
                }
                finally
                {
                    FinallyBody();
                }
            }
            return null;
        }

    }
}

