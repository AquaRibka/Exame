using System;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Data;
using System.Diagnostics.Contracts;
using Model_eTOM.Add;

namespace Model_eTOM
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        //Переменные для поключения к БД по данным из файла App.config
        string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        
        
        public Main()
        {
            //Подключение к БД
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();
            Services_table();
            Marketing_table();
            Equipment_table();
            Contracts_0_table();
            Supply_table();
            Contract_2_table();
        }
        //Выгрузка из БД таблицы услуг
        private void Services_table()
        {
            try
            {
                connecting.Open();
                //Строка с SQL запросом
                string sql = @"SELECT id, price, serv_name, about, date FROM public.""Services"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                    //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                           //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                          //
                //Запрет на изменение таблицы services
                services.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                services.DataContext = DataSet;
                connecting.Close();

            }
            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        //Выгрузка из БД таблицы маркетинга
        private void Marketing_table()
        {
            try
            {
                connecting.Open();
                //Строка с SQL запросом
                string sql = @"
                    SELECT id, name, budget, date_start, date_end, target
                    FROM public.""Marketing"";";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                //Запрет на изменение таблицы marketing
                marketing.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                marketing.DataContext = DataSet;
                connecting.Close();
            }
            //Вывод ошибок при выполнении кода
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
                //Строка с SQL запросом
                connecting.Open();
                string sql = @"
                    SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Equipment"".doc_number
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id;";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                //Запрет на изменение таблицы equipment
                equipment.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                equipment.DataContext = DataSet;
                connecting.Close();
            }
            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        private void Contracts_0_table()
        {
            try
            {
                //Строка с SQL запросом
                connecting.Open();
                string sql = @"
                    SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                //Запрет на изменение таблицы equipment
                contracts_0.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                contracts_0.DataContext = DataSet;
                connecting.Close();
            }
            //Вывод ошибок при выполнении кода
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
                //Строка с SQL запросом
                connecting.Open();
                string sql = @"
                    SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                    FROM public.""Supply""
                    JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                    JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                //Запрет на изменение таблицы equipment
                supply.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                supply.DataContext = DataSet;
                connecting.Close();
            }
            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        private void Contract_2_table()
        {
            try
            {
                //Строка с SQL запросом
                connecting.Open();
                string sql = @"
                    SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");
               
                //Запрет на изменение таблицы contracts_2
                contracts_2.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                contracts_2.DataContext = DataSet;
                connecting.Close();
            }
            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        //Переключение на страницу с рекламными контрактами со страницы Маркетинг
        private void Contract_2_show(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 5;
        }
        //Возвращение со страницы с рекламными контрактами на Маркетинг
        private void Mark_back(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }
        //Замена текстовго поля на выпадающий список
        private void Service_add(object sender, RoutedEventArgs e)
        {
            Service_add service_Add = new Service_add();
            service_Add.Show();
        }
        private void Mark_add(object sender, RoutedEventArgs e)
        {
            Mark_add mark_Add = new Mark_add();
            mark_Add.Show();
        }
        private void ShowForecast(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = marketing.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Forecast forecast = new Forecast();                                                  //Прогноз
            forecast.idData += idData;
            forecast.Show();
        }
        private void Eqp_add(object sender, RoutedEventArgs e)
        {
            Eqp_add eqp_Add = new Eqp_add();
            eqp_Add.Show();
        }
        private void Contract_add(object sender, RoutedEventArgs e)
        {
            Contracts_add contract_Add = new Contracts_add();
            contract_Add.Show();
        }
        private void Supply_add(object sender, RoutedEventArgs e)
        {
           Supply_add supply_Add = new Supply_add();
            supply_Add.Show();
        }
        private void Supply_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchSupply.Text == null || string.IsNullOrWhiteSpace(searchSupply.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchSupplyText.Text == null || string.IsNullOrWhiteSpace(searchSupplyText.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql=null;
            switch (searchSupply.Text)
            {
                case "Статус":
                    sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".name = '" + searchSupplyText.Text + "';";
                    break;
                case "Дата поставки":
                    sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".date = " + '\u0027' + searchSupplyText.Text + '\u0027' + ";";
                    break; 
                case "Контракт":
                    sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".interior_number = " + searchSupplyText.Text + ";";
                    break; 
                case "Сумма":
                    sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".summ = " + searchSupplyText.Text + ";";
                    break;
            }
            NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
            NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
            DataSet DataSet = new DataSet();                                            //  для их хранения
            Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                        //Запрет на изменение таблицы equipment
            supply.IsReadOnly = true;
            //Заполнение таблицы services данными, выгруженными из БД
            supply.DataContext = DataSet;
            connecting.Close();
        }
    }
}
