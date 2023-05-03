using Npgsql;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Equipment_edit.xaml
    /// </summary>
    public partial class Equipment_edit : Window
    {
        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connect;
        public string idData { get; set; }
        public Equipment_edit()
        {
            InitializeComponent();
        }
    }
}
