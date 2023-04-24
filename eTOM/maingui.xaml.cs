using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace eTOM
{
    /// <summary>
    /// Логика взаимодействия для maingui.xaml
    /// </summary>
    public partial class maingui : Page
    {

        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connecting;

      

        public maingui()
        {
            InitializeComponent();
           
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            connecting = new NpgsqlConnection(connectPostgre);
            Services_table();
            

            
        }

        private void Services_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, price, serv_name, about, date
	FROM public." + '\u0022' + "Services" + '\u0022' + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Services");
                services.IsReadOnly = true;
                services.DataContext = iDataSet;
           //     iDataSet.Tables["Services"].Columns.Add("activity", typeof(string));
               iDataSet.Tables["Services"].Columns.Add("activity", typeof(string));
                int i = 0;
               while( iDataSet.Tables["Services"].Rows.Count >  i)
                {
                    /*   string activity = iDataSet.Tables["Services"].Rows[i].Field<string>("activity");
                       activity = i.ToString(); */

                    iDataSet.Tables["Services"].Rows[i]["activity"] = i;
                


                    i++;
                }





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            
            DataRowView rowView = services.SelectedValue as DataRowView;
            Serv_edit serv_edit = new Serv_edit();
            serv_edit.test.Text += rowView[0].ToString();
            serv_edit.Show();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Services_table();
        }
    }
    }

