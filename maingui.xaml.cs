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
using System.Text.RegularExpressions;
using Npgsql;
using Excel = Microsoft.Office.Interop.Excel;

namespace eTOM

{
    
    /// <summary>
    /// Логика взаимодействия для maingui.xaml
    /// </summary>
    /// 
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
            Marketing_table();
            Equipment_table();
            Contracts0_table();
            Supply_table();
            Contracts2_table();


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
                
           //     (services.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                services.IsReadOnly = true;
                services.DataContext = iDataSet;
           
               





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }
        private void Supply_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT public." + '\u0022' + "Supply" + '\u0022' + ".id, public." + '\u0022' + "Supply_status" + '\u0022' + ".sp_status_name, public." + '\u0022' + "Supply" + '\u0022' + ".about, public." + '\u0022' + "Supply" + '\u0022' + ".summ, public." + '\u0022' + "Supply" + '\u0022' + ".date, public." + '\u0022' + "Contracts" + '\u0022' + ".interior_number FROM public." + '\u0022' + "Supply" + '\u0022' +
                " JOIN public." + '\u0022' + "Contracts" + '\u0022' + " ON public." + '\u0022' + "Supply" + '\u0022' + ".contract_id = public." + '\u0022' + "Contracts" + '\u0022' + ".id" +
                " JOIN public." + '\u0022' + "Supply_status" + '\u0022' + " ON public." + '\u0022' + "Supply" + '\u0022' + ".status_id = public." + '\u0022' + "Supply_status" + '\u0022' + ".id;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Supply");
                
           //     (services.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                supply.IsReadOnly = true;
                supply.DataContext = iDataSet;
           
               





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }

         private void Contracts0_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, interior_number, organization, sum, date_end   
	FROM public." + '\u0022' + "Contracts" + '\u0022' + " WHERE type = 0;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Contracts_0");
                
           //     (services.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                contracts_0.IsReadOnly = true;
                contracts_0.DataContext = iDataSet;
           
               





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }
         private void Contracts2_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, interior_number, organization, sum, date_end   
	FROM public." + '\u0022' + "Contracts" + '\u0022' + " WHERE type = 2;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Contracts_2");
                
           //     (services.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                contracts_2.IsReadOnly = true;
                contracts_2.DataContext = iDataSet;
           
               





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }

        private void Equipment_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT public." + '\u0022' + "Equipment" + '\u0022' +  ".id ,public." + '\u0022' + "Eqp_category" + '\u0022' + ".cat_name, public." + '\u0022' + "Equipment" + '\u0022' + ".name, public." + '\u0022' + "Users" + '\u0022' + ".fio, public." + '\u0022' + "Equipment" + '\u0022' + ".suitability, public." + '\u0022' + "Equipment" + '\u0022' + ".doc_number FROM public." + '\u0022' + "Equipment" + '\u0022' +
                " JOIN public." + '\u0022' + "Eqp_category" + '\u0022' + " ON public." + '\u0022' + "Equipment" + '\u0022' + ".category_id = public." + '\u0022' + "Eqp_category" + '\u0022' + ".id" +
                " JOIN public." + '\u0022' + "Users" + '\u0022' + " ON public." + '\u0022' + "Equipment" + '\u0022' + ".responsible_id = public." + '\u0022' + "Users" + '\u0022' + ".id;";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Equipment");
                
           //     (services.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                equipment.IsReadOnly = true;
                equipment.DataContext = iDataSet;
           
               





                connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
           }
        private void Marketing_table()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, name, budget, date_start, date_end, target
	FROM public." + '\u0022' + "Marketing" + '\u0022' + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Services");
              /*     string i, j;
                    foreach (DataRow row in iDataSet.Tables[0].Rows)
                    {

                        i = row["date_start"].ToString().Remove(row["date_start"].ToString().LastIndexOf(@" "));
                        j = row["date_end"].ToString().Remove(row["date_end"].ToString().LastIndexOf(@" "));

                        row.SetField("date_start", i);
                        row.SetField("date_end", j);
                    } */
            //   (marketing.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
             //  (marketing.Columns[5] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
                marketing.IsReadOnly = true;
                marketing.DataContext = iDataSet;
                
                    //     iDataSet.Tables["Services"].Columns.Add("activity", typeof(string));
                    //     iDataSet.Tables["Services"].Columns.Add("activity", typeof(string));






                    connecting.Close();

            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }

      
        private void Serv_edit_click(object sender, RoutedEventArgs e)
        {
            
            
          DataRowView rowView = services.SelectedValue as DataRowView;
           Serv_edit serv_edit = new Serv_edit();
            string idData = rowView[0].ToString();
            serv_edit.idData = idData;
            //   serv_edit.test.Text += rowView[0].ToString();
            serv_edit.Show();
            
            
        }

        

        private void Reload_page(object sender, RoutedEventArgs e)
        {
            Services_table();
            Marketing_table();
            Equipment_table();
            Contracts0_table();
            Supply_table();
            Contracts2_table();

        }

        private void Serv_add_click(object sender, RoutedEventArgs e)
        {
            Serv_add serv_add = new Serv_add();
            serv_add.Show();
        }

        private void Serv_excel(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT serv_name, about, price, channels, cinema, mobile_connection, equipment, speed, video, date
	FROM public." + '\u0022' + "Services" + '\u0022' + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Services");
         //       services.IsReadOnly = true;
           //     services.DataContext = iDataSet;
             
                connecting.Close();

                DataTable ct = iDataSet.Tables[0];
                
                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;
                sheet.Cells[1, 1] = "Название";
                sheet.Cells[1, 2] = "Описание";
                sheet.Cells[1, 3] = "Цена";
                sheet.Cells[1, 4] = "Каналы";
                sheet.Cells[1, 5] = "Онлайн-кинотеатр";
                sheet.Cells[1, 6] = "Мобильная связь";
                sheet.Cells[1, 7] = "Оборудование";
                sheet.Cells[1, 8] = "Скорость интернета";   
                sheet.Cells[1, 9] = "Видеозапись";   
                sheet.Cells[1, 10] = "Дата";   

                Excel.Range range = sheet.Range[sheet.Cells[2, 1], sheet.Cells[ct.Rows.Count, ct.Columns.Count]];
                for (int i = 0; i < ct.Rows.Count; ++i)
                    for (int j = 0; j < ct.Columns.Count; ++j)
                    {
                        range.Cells[1 + i, 1 + j] = ct.Rows[i][j].ToString();
                      //  MessageBox.Show(ct.Rows[i][j].ToString());
                    
                         if (j == 2) {
                            range.Cells[1 + i, 1 + j] = double.Parse(ct.Rows[i][j].ToString());
                        } 
                        else if (j == 9)
                        {
                            string dateExcel = ct.Rows[i][j].ToString().Remove(ct.Rows[i][j].ToString().LastIndexOf(@" ")); ;
                            range.Cells[1 + i, 1 + j] = dateExcel;
                        }
                        if (ct.Rows[i][j].ToString() == "True")
                        {
                            range.Cells[1 + i, 1 + j] = "Да";
                        } 
                        else if (ct.Rows[i][j].ToString() == "False")
                        {
                            range.Cells[1 + i, 1 + j] = "Нет";
                        }

                    }
                sheet.Cells.EntireColumn.AutoFit();
                sheet.Cells.EntireRow.AutoFit();
                sheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                sheet.PageSetup.Zoom = false;
                sheet.PageSetup.FitToPagesWide = 1;
                sheet.PageSetup.FitToPagesTall = false;
                sheet.PageSetup.ScaleWithDocHeaderFooter = true;
                sheet.PageSetup.AlignMarginsHeaderFooter = true;
                range = sheet.Range["A1", "X1"];
                range.Font.Bold = true;
                sheet.Range["A1", "X1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objExcel.Visible = true;
            }

            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
            
        }

        private void Mark_edit_show(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = marketing.SelectedValue as DataRowView;
            Mark_edit mark_edit = new Mark_edit();
            mark_edit.idData += rowView[0].ToString();
            mark_edit.Show();
        }

        private void Mark_add_show(object sender, RoutedEventArgs e)
        {
            Mark_add mark_add = new Mark_add();
            mark_add.Show();
        }
        
        private void Equipment_edit_show(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = equipment.SelectedValue as DataRowView;
            Equipment_edit equipment_edit = new Equipment_edit();
            equipment_edit.idData += rowView[0].ToString();
            equipment_edit.Show();
        }

        private void Equipment_add_show(object sender, RoutedEventArgs e)
        {
            Equipment_add equipment_add = new Equipment_add();
            equipment_add.Show();
        }
        
        private void Contracts_0_edit_show(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = contracts_0.SelectedValue as DataRowView;
            Contracts_0_edit contracts_0_edit = new Contracts_0_edit();
            contracts_0_edit.idData += rowView[0].ToString();
            contracts_0_edit.Show();
        }

        private void Contracts_0_add_show(object sender, RoutedEventArgs e)
        {
            Contracts_0_add contracts_0_add = new Contracts_0_add();
            contracts_0_add.Show();
        }
        private void Supply_edit_show(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = supply.SelectedValue as DataRowView;
            Supply_edit supply_edit = new Supply_edit();
            supply_edit.idData += rowView[0].ToString();
            supply_edit.Show();
        }

        private void Supply_add_show(object sender, RoutedEventArgs e)
        {
            Supply_add supply_add = new Supply_add();
            supply_add.Show();
        }

        private void Contract_2_show(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 5;
        }
        private void Mark_back(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }


        private void Forecast_show(object sender, RoutedEventArgs e)
        {

            DataRowView rowView = marketing.SelectedValue as DataRowView;
            Mark_forecast forecast = new Mark_forecast();                                                  //Прогноз
            forecast.idData += rowView[0].ToString();
            forecast.Show();

        }

        private void Equipment_report(object sender, RoutedEventArgs e)
        {
        }
        private void Contracts_0_report(object sender, RoutedEventArgs e)
        {
        }
        private void Supply_report(object sender, RoutedEventArgs e)
        {
        }

            private void Mark_report(object sender, RoutedEventArgs e)
        {
            
            try
            {
                connecting.Open();
             
                string sql = @"SELECT public." + '\u0022' + "Marketing" + '\u0022' + ".name, public." + '\u0022' + "Marketing" + '\u0022' + ".budget, public." + '\u0022' + "Marketing" + '\u0022' + ".target, public." + '\u0022' + "Marketing" + '\u0022' + ".actions, public." + '\u0022' + "Marketing" + '\u0022' + ".date_start, public." + '\u0022' + "Marketing" + '\u0022' + ".date_end, public." + '\u0022' + "Contracts" + '\u0022' + ".interior_number FROM public." + '\u0022' + "Marketing" + '\u0022' +
                "JOIN public." + '\u0022' + "Contracts" + '\u0022' + " ON public." + '\u0022' + "Marketing" + '\u0022' + ".contract = public." + '\u0022' + "Contracts" + '\u0022' + ".id;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "Marketing");
                //       services.IsReadOnly = true;
                //     services.DataContext = iDataSet;

                connecting.Close();

                DataTable ct = iDataSet.Tables[0];
              

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;
                sheet.Cells[1, 1] = "Название компании";
                sheet.Cells[1, 2] = "Бюджет";
                sheet.Cells[1, 3] = "Цели";
                sheet.Cells[1, 4] = "Действия";
                sheet.Cells[1, 5] = "Дата начала";
                sheet.Cells[1, 6] = "Дата окончания";
                sheet.Cells[1, 7] = "Номер связанного контракта";
                

                Excel.Range range = sheet.Range[sheet.Cells[2, 1], sheet.Cells[ct.Rows.Count, ct.Columns.Count]];
                for (int i = 0; i < ct.Rows.Count; ++i)
                    for (int j = 0; j < ct.Columns.Count; ++j)
                    {
                        range.Cells[2 + i, 1 + j] = ct.Rows[i][j].ToString();
                        //  MessageBox.Show(ct.Rows[i][j].ToString());

                         if (j == 1)
                          {
                              range.Cells[2 + i, 1 + j] = double.Parse(ct.Rows[i][j].ToString());
                          }  else if(j == 2)
                        {
                            string str = ct.Rows[i][j].ToString();
                            range.Cells[2 + i, 1 + j] = Regex.Replace(str, @"\s+", " "); 
                        } else if (j == 3) {
                            var values = (string[])ct.Rows[0]["actions"];
                            string str = string.Join(", ", values);
                            range.Cells[2 + i, 1 + j] = Regex.Replace(str, @"\s+", " "); 
                        }
                        else if (j == 4 || j ==5)
                          {
                              string dateExcel = ct.Rows[i][j].ToString().Remove(ct.Rows[i][j].ToString().LastIndexOf(@" ")); ;
                              range.Cells[2 + i, 1 + j] = dateExcel;
                          }
                         /* if (ct.Rows[i][j].ToString() == "True")
                          {
                              range.Cells[1 + i, 1 + j] = "Да";
                          }
                          else if (ct.Rows[i][j].ToString() == "False")
                          {
                              range.Cells[1 + i, 1 + j] = "Нет";
                          }*/

                    }
                sheet.Cells.EntireColumn.AutoFit();
                sheet.Cells.EntireRow.AutoFit();
                sheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                sheet.PageSetup.Zoom = false;
                sheet.PageSetup.FitToPagesWide = 1;
                sheet.PageSetup.FitToPagesTall = false;
                sheet.PageSetup.ScaleWithDocHeaderFooter = true;
                sheet.PageSetup.AlignMarginsHeaderFooter = true;
                range = sheet.Range["A1", "X1"];
                range.Font.Bold = true;
                sheet.Range["A1", "X1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Задание имени листа
                sheet.Name = "Маркетинговые компании";
                objExcel.Visible = true;
            }

            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }

        }
    }
    }

