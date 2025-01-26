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

        public SqlConnection Conn
        {
            get { return connection; }
            private set { connection = value; }
        }

        //SqlDataReader rdr { get; set; }

        private SQLConnection()
        {
        }
        private SQLConnection(ref string coon)
        {
            Conn = new SqlConnection(@"Server=DESKTOP-QVUI8Q3;database=AlyaFlibusta2;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true");
        }

        public static SQLConnection getInstance()
        {
            if (instance == null)
                instance = new SQLConnection();
            return instance;
        }
        public static SQLConnection getInstance(ref string coon)
        {
            if (instance == null)
                instance = new SQLConnection(ref coon);
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
    }
}

