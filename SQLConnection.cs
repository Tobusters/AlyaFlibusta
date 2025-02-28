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
        static string serverName;
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
                    SqlCommand command = new SqlCommand($@"USE master; GO IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{databaseName}')begin;CREATE DATABASE '{databaseName}'", Conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show($"База данных '{databaseName}' успешно создана на сервере.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Ошибка при создании базы данных");
                }
            }
        }
        private void CreateDataBase()
        {
            string cmd = @"USE master; GO IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Warehouse')begin;CREATE DATABASE Warehouse ON(NAME = Warehouse, FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Warehouse.mdf',SIZE = 8,MAXSIZE = 64,FILEGROWTH = 5)end;";
            ConnectToDTBaseAndQuery(cmd);

        }

        private void ReconnectSql()
        {
            Conn.Close();
            Conn = null;
            Conn = new SqlConnection($@"Server={serverName};database={databaseName};Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");
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
            IsConnected = true;
        }
        private SQLConnection(string SvName, string coon)
        {
            serverName = SvName;
            Conn = new SqlConnection(coon);
            CreateDataBase();
            ReconnectSql();
            rdr = null;
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
        public static SQLConnection getInstanceCreate(string SvName, string coon)
        {
            if (instance == null)
                instance = new SQLConnection(SvName, coon);
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

