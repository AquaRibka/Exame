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

namespace eTOM
{
    /// <summary>
    /// Логика взаимодействия для Mark_edit.xaml
    /// </summary>
    public partial class Mark_edit : Window
    {
        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connect;
        public string idData { get; set; }
        public Mark_edit()
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

                    string sql = @"DELETE FROM public." + '\u0022' + "Marketing" + '\u0022' + "WHERE id = " + idData + ";";
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
                  
                    connect.Open();


                    string sql = @"UPDATE public." + '\u0022' + "Services" + '\u0022' + "SET channels=" + channels.Text + ", cinema=" + cinema.Text + ", mobile_connection=" + mobile.Text + ", equipment=" + equipment.Text + ", price=" + priceBack + ", serv_name='" + name.Text + "', about='" + about.Text + "', video=" + video.Text + ", speed=" + speedBack + " WHERE id = " + idData + ";";
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
            // MessageBox.Show(idData);
            try
            {

                connect.Open();
                string sql = @"SELECT *
	FROM public." + '\u0022' + "Services" + '\u0022' + "WHERE id = " + idData + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataTable iDataSet = new DataTable();
                iAdapter.Fill(iDataSet);
                DataRow[] data_row = iDataSet.Select();
               


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
