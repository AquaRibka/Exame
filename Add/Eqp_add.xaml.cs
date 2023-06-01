using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using static Model_eTOM.Add.Service_add;

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Eqp_add.xaml
    /// </summary>
    public partial class Eqp_add : Window
    {
        List<string> cabinetNames = new List<string>();
        
        string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public Eqp_add()
        {
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();
            LoadCab();
            LoadCategory();
        }

        private void LoadCab()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT name FROM public.""Cabinet"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    cabinetNames.Add(name);
                }
                connecting.Close();
                Cabinet.ItemsSource = cabinetNames;
            }

            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        public class Cat
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }
        List<Cat> category = new List<Cat>();

        private void LoadCategory()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, cat_name FROM public.""Eqp_category"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    category.Add(new Cat { Value = id, Display = name });
                }
                connecting.Close();
                Category.ItemsSource = category;
            }

            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
}
}
