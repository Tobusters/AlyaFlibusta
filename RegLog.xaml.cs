using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RegLog.xaml
    /// </summary>
    /// 

    public partial class RegLog : Window
    {
        User user;
        public SqlConnection Conn;
        SqlDataReader rdr;
        public bool Is_Logged = false;


        public RegLog()
        {
            InitializeComponent();
        }

        public void SetRef(ref SqlConnection conn)
        {
            Conn = conn;
        }

        private void ToLogIn(object sender, RoutedEventArgs e)
        {
            Reg.Visibility = Visibility.Hidden;
            Log.Visibility = Visibility.Visible;
        }

        public User GetLog()
        {
            return user;
        }

        public void LogIn(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand($"exec GetUserByLogin {LogIn_Login.Text}", Conn);//Запрос
                Conn.Open();
                rdr = sqlCommand.ExecuteReader();
                while (rdr.Read()) {
                    if (rdr[4].ToString() == LogIn_Pass.Text) {
                        if (rdr[1].ToString() == "1")
                        {
                            MessageBox.Show("Ваш аккаунт заблокирован", "БАН");
                            Owner.Close();
                            Close();
                            Is_Logged = false;
                            return;

                        }
                        user = new User(rdr[0].ToString(), rdr[3].ToString(), (rdr[2].ToString()) == "1" ? true : false);
                        Hide();
                        Is_Logged = true;
                        return;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.ToString());
            }
            finally {
                Conn.Close();
                rdr.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Conn.Close();
                rdr.Close();
            }
            catch { }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Conn.Close();
                rdr.Close();
            }
            catch { }
        }
    }
}