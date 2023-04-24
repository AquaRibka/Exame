using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Net.Mime.MediaTypeNames;

namespace eTOM
{
    /// <summary>
    /// Логика взаимодействия для Serv_edit.xaml
    /// </summary>
    public partial class Serv_edit : Window
    {
        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connect;
        public Serv_edit()
        {
            InitializeComponent();
           
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           connect = new NpgsqlConnection(connectPostgre);
            Data_Upload();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить услугу?", "Услуга удалена", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    connect.Open();

                    string sql = @"DELETE FROM public." + '\u0022' + "Services" + '\u0022' + "WHERE id = " + test.Text + ";";
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {
                connect.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите внести изменения?", "Изменения внесены", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //  string channelsVar = "true";
                    string priceBack = price.Text.Remove(price.Text.LastIndexOf(@","));
                    string speedBack = speed.Text.Remove(speed.Text.LastIndexOf(@" "));
                    connect.Open();

                    
                  string sql = @"UPDATE public." + '\u0022' + "Services" + '\u0022' + "SET channels="+channels.Text+", cinema="+ cinema.Text+ ", mobile_connection=" + mobile.Text+ ", equipment=" + equipment.Text+ ", price=" +priceBack + ", serv_name='" + name.Text+ "', about='" + about.Text+ "', video="+video.Text+", speed=" + speedBack+" WHERE id = " + test.Text +";";
                    sql = sql.Replace("Нет", "false");
                    sql = sql.Replace("Да", "true");
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    Data_Upload();
                    MessageBox.Show("Изменения сохранены");
                } 

            }
            catch (Exception ex)
            {
                connect.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Data_Upload()
        {

            try
            {
                
                connect.Open();
                string sql = @"SELECT *
	FROM public." + '\u0022' + "Services" + '\u0022' + "WHERE id = "+test.Text+";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataTable iDataSet = new DataTable();
                iAdapter.Fill(iDataSet);
                DataRow[] data_row = iDataSet.Select();
                name.Text = data_row[0]["serv_name"].ToString();
                about.Text = data_row[0]["about"].ToString();
                switch (data_row[0]["speed"].ToString())
                {
                    case "100":
                        speed.SelectedIndex = 0;
                        break;

                    case "200":
                        speed.SelectedIndex = 1;
                        break;
                    case "300":
                        speed.SelectedIndex = 2;
                        break;
                    case "400":
                        speed.SelectedIndex = 3;
                        break;
                    case "500":
                        speed.SelectedIndex = 4;
                        break;
                    case "1":
                        speed.SelectedIndex = 5;
                        break;
                    case "2":
                        speed.SelectedIndex = 6;
                        break;
                }
                channels.Text = data_row[0]["channels"].ToString();

                if (data_row[0]["channels"].ToString() == "False")
                {
                    channels.SelectedIndex = 0; 
                } else { channels.SelectedIndex = 1; }

                if (data_row[0]["cinema"].ToString() == "False")
                {
                    cinema.SelectedIndex = 0;
                }
                else { cinema.SelectedIndex = 1; }

                if (data_row[0]["mobile_connection"].ToString() == "False")
                {
                    mobile.SelectedIndex = 0;
                }
                else { mobile.SelectedIndex = 1; }
                
                if (data_row[0]["equipment"].ToString() == "False")
                {
                    equipment.SelectedIndex = 0;
                }
                else { equipment.SelectedIndex = 1; }

                if (data_row[0]["video"].ToString() == "False")
                {
                    video.SelectedIndex = 0;
                }
                else { video.SelectedIndex = 1; }

                price.Text = data_row[0]["price"].ToString();


                connect.Close();

            }
            catch (Exception ex)
            {
                connect.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

       
    }
}
