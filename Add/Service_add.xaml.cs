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

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Service_add.xaml
    /// </summary>
    public partial class Service_add : Window
    {
        string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public Service_add()
        {
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();
            SpeedBox.ItemsSource = speed;
            Channels.ItemsSource = boolchouse;
            Cinema.ItemsSource = boolchouse;
            MobileConnection.ItemsSource = boolchouse;
            Video.ItemsSource = boolchouse;
            Equipment.ItemsSource = boolchouse;
        }
        public class Speed
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }
        List<Speed> speed = new List<Speed>
        {
            new Speed { Value = 100, Display = "100 mb/s" },
            new Speed { Value = 200, Display = "200 mb/s" },
            new Speed { Value = 300, Display = "300 mb/s" },
            new Speed { Value = 400, Display = "400 mb/s" },
            new Speed { Value = 500, Display = "500 mb/s" },
            new Speed { Value = 600, Display = "600 mb/s" },
            new Speed { Value = 700, Display = "700 mb/s" },
            new Speed { Value = 800, Display = "800 mb/s" },
            new Speed { Value = 900, Display = "900 mb/s" },
            new Speed { Value = 1, Display = "1 Gb/s" },
            new Speed { Value = 2, Display = "2 Gb/s" },
        };
        public class BoolFind
        {
            public bool Value { get; set; }
            public string Display { get; set; }
        }
        List<BoolFind> boolchouse = new List<BoolFind>()
        {
         new BoolFind { Value = true, Display = "Да"},
         new BoolFind { Value = false, Display = "Нет"}
        };

        
    }
}
