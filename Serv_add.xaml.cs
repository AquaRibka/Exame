using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace eTOM
{
    /// <summary>
    /// Логика взаимодействия для Serv_add.xaml
    /// </summary>
    public partial class Serv_add : Window
    {
        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connect;

        public Serv_add()
        {
            InitializeComponent();
            connect = new NpgsqlConnection(connectPostgre);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите добавить услугу?", "Услуга добавлена", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //  string channelsVar = "true";
                   // string priceBack = price.Text.Remove(price.Text.LastIndexOf(@","));
                    string speedBack = speed.Text.Remove(speed.Text.LastIndexOf(@" "));

                    connect.Open();


                    string sql = @"INSERT INTO public." + '\u0022' + "Services" + '\u0022' + "(channels, cinema, mobile_connection, equipment, price, serv_name, about, video, speed) VALUES ("+channels.Text+", "+ cinema.Text + ", " + mobile.Text + ", " + equipment.Text + ", " + price.Text + ", '" + name.Text + "', '" + about.Text + "', " + video.Text + ", " + speedBack + ");";
                    sql = sql.Replace("Нет", "false");
                    sql = sql.Replace("Да", "true");
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    MessageBox.Show("Данные добавлены");
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                connect.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            speed.Text = string.Empty;
            price.Text = string.Empty;
            name.Text = string.Empty;
            about.Text = string.Empty;
            video.Text = string.Empty;
            channels.Text = string.Empty;
            cinema.Text = string.Empty; 
            mobile.Text = string.Empty; 
            equipment.Text = string.Empty;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
