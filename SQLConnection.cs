using System.Data.SqlClient;
using System.Windows;

namespace WpfApp1
{
    class SQLConnection
    {
        private static SQLConnection instance;

        //static readonly string connectionString = @"Server=.\SQLEXPRESS;database=Warehouse;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true";
        //static readonly string connectionString = @"Server=DESKTOP-QVUI8Q3;database=Warehouse;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true";
        private SqlConnection connection;

        public static bool IsConnected { get; private set; } = false;


        private void CheckConn()
        {
            try
            {
                Conn.Open();
                IsConnected = true;
            }
            catch (SqlException e)
            {
                IsConnected = false;
                MessageBox.Show(e.Message);
            }
            finally { Conn.Close(); }
        }


        public SqlConnection Conn
        {
            get { return connection; }
            private set { connection = value; }
        }


        private SQLConnection()
        {
            Conn = new SqlConnection(@"Server=DESKTOP-QVUI8Q3;database=AlyaFlibusta2;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");//дом
            //Conn = new SqlConnection(@"Server=DESKTOP-CVTHJDK;database=AlyaFlibusta2;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");
        }
        private SQLConnection(string coon)
        {
            Conn = new SqlConnection(coon);
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



        private void TryConnectionAndQueryBody(string select)
        {
            SqlCommand sqlCommand = new SqlCommand(select, Conn);
            Conn.Open();
            sqlCommand.ExecuteNonQuery();
        }
        private void FinallyBody()
        {
            //if (rdr != null)
            //{
            //    rdr.Close();
            //}
            if (Conn != null)
            {
                Conn.Close();
            }
        }

        public void ConnectToDTBaseAndQuery(string select)//without Select
        {
            try
            {
                TryConnectionAndQueryBody(select);
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
        public SqlDataReader ConnectToDTBaseAndRead(string select)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(select, Conn);//Запрос
                Conn.Open();//Открываем подключение
                SqlDataReader rdr = sqlCommand.ExecuteReader();//Включаем Читалку, для считывания ответа на запрос sql
                return rdr;//Не самая удобная реализация, но работает
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message.ToString(), e.ToString());
            }
            finally
            {
               // FinallyBody();
            }
            return null;
        }


    }
}

