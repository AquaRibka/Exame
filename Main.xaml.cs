using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Windows.Controls;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net.Http;


namespace Model_eTOM
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    /// 



    public partial class Main : Page
    {
        //Переменные для поключения к БД по данным из файла App.config
        readonly string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
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
                    SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id;";

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
            DataRowView rowView = services.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Service_add service_Add = new Service_add();
            service_Add.IdData = idData;
            service_Add.Show();
        }
        private void Service_add_new(object sender, RoutedEventArgs e)
        {
            Service_add service_Add = new Service_add();
            service_Add.Show();
        }
        private void Mark_add(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = marketing.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Mark_add mark_Add = new Mark_add();
            mark_Add.IdData = idData;
            mark_Add.Show();
        }
        private void Mark_add_new(object sender, RoutedEventArgs e)
        {
            Mark_add mark_Add = new Mark_add();
            mark_Add.Show();
        }
        private void ShowForecast(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = marketing.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Forecast forecast = new Forecast();                                                  //Прогноз
            forecast.IdData += idData;
            forecast.Show();
        }
        private async void RemoteConnect(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = equipment.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            try
            {
                //Строка с SQL запросом
                connecting.Open();
                string sql = @"
                    SELECT public.""Equipment"".ip, public.""Equipment"".status_id, public.""Equipment"".name  FROM public.""Equipment"" WHERE public.""Equipment"".id =" + idData + ";";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                connecting.Close();
                string ip = "";
                string status = "";
                if (DataSet.Tables["DataBD"].Rows.Count > 0)
                {
                    ip = DataSet.Tables["DataBD"].Rows[0]["ip"].ToString();
                    status = DataSet.Tables["DataBD"].Rows[0]["status_id"].ToString();
                }
                else
                {
                    MessageBox.Show("Устройство не поддерживает удаленный доступ");
                }
                if (status == "2")
                {
                    MessageBox.Show("Устройство в данный момент занято");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(ip))
                {
                    string command = $"/v:{ip} /admin";
                    Process process = new Process();
                    process.StartInfo.FileName = "mstsc.exe";
                    process.StartInfo.Arguments = command;
                    process.Start();
                    string message = "Вы забронировали устройство "+ DataSet.Tables["DataBD"].Rows[0]["name"].ToString() + " на 60 минут. Через 60 минут управление может быть передано другому пользователю.";
                    await SendMessage("1108697409", message);
                        connecting.Open();
                        string sql_up = @"
                        UPDATE public.""Equipment"" SET status_id = 2 WHERE public.""Equipment"".id =" + idData + ";";
                        NpgsqlCommand cmd_up = new NpgsqlCommand(sql_up, connecting);
                        cmd_up.ExecuteNonQuery();
                        connecting.Close();
                }
            }
            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }

        }
        private async Task SendMessage(string chatId, string messageText)
        {
            string botToken = "5928131605:AAEGK63z1Fy_H3xPBbNwRxsOhBOMUOJQ9jU";

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";
                var content = new StringContent($"{{\"chat_id\": \"{chatId}\", \"text\": \"{messageText}\"}}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Сообщение успешно отправлено
                    Console.WriteLine("Сообщение успешно отправлено.");
                }
                else
                {
                    // Ошибка при отправке сообщения
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Ошибка при отправке сообщения: " + errorMessage);
                }
            }
        }
        private void Eqp_add(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = equipment.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Eqp_add eqp_Add = new Eqp_add();
            eqp_Add.IdData = idData;
            eqp_Add.Show();
        }
        private void Eqp_add_new(object sender, RoutedEventArgs e)
        {
            Eqp_add eqp_Add = new Eqp_add();
            eqp_Add.Show();
        }
        private void Contract_add(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = contracts_0.SelectedItem as DataRowView;
            if (rowView == null)
            {
                rowView = contracts_2.SelectedItem as DataRowView;
            }
            string idData = rowView.Row["id"].ToString();
            Contracts_add contract_Add = new Contracts_add();
            contract_Add.IdData += idData;
            contract_Add.Show();
        }
        private void Contract_add_new(object sender, RoutedEventArgs e)
        {
            Contracts_add contract_Add = new Contracts_add();
            contract_Add.Show();
        }
        private void Supply_add(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = supply.SelectedItem as DataRowView;
            string idData = rowView.Row["id"].ToString();
            Supply_add supply_Add = new Supply_add();
            supply_Add.IdData = idData;
            supply_Add.Show();
        }
        private void Supply_add_new(object sender, RoutedEventArgs e)
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
            string sql = null;
            switch (searchSupply.Text)
            {
                case "Статус":
                    bool hasDigits = Regex.IsMatch(searchSupplyText.Text, @"\d");
                    if (hasDigits)
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    else
                    {
                        sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".name = '" + searchSupplyText.Text + "';";
                    }
                    break;
                case "Дата поставки":
                    string inputValue = searchTextServ.Text;
                    DateTime dateValue;
                    bool isValidDate = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDate)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".date = " + '\u0027' + formattedDate + '\u0027' + ";";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
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
                    bool isValidPrice = (Regex.IsMatch(searchTextServ.Text, @"^\d+(\,\d{1,2})?$") || Regex.IsMatch(searchTextServ.Text, @"^\d+(\.\d{1,2})?$"));
                    if (isValidPrice)
                    {
                        sql = @"
                        SELECT public.""Supply"".id, public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id
                        WHERE public.""Supply_status"".summ = " + searchSupplyText.Text.Replace('.', ',') + ";";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                supply.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                supply.DataContext = DataSet;
            }
            connecting.Close();
        }
        private void Service_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchServ.Text == null || string.IsNullOrWhiteSpace(searchServ.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchTextServ.Text == null || string.IsNullOrWhiteSpace(searchTextServ.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql = null;
            switch (searchServ.Text)
            {
                case "Название":
                    sql = @"
                       SELECT id, price, serv_name, about, date FROM public.""Services""
                        WHERE public.""Services"".serv_name = '" + searchTextServ.Text + "';";
                    break;
                case "Цена":
                    bool isValidPrice = (Regex.IsMatch(searchTextServ.Text, @"^\d+(\,\d{1,2})?$") || Regex.IsMatch(searchTextServ.Text, @"^\d+(\.\d{1,2})?$"));
                    if (isValidPrice)
                    {
                        sql = @"
                        SELECT id, price, serv_name, about, date FROM public.""Services""
                        WHERE public.""Services"".price = '" + searchTextServ.Text.Replace('.', ',') + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Дата создания":
                    string inputValue = searchTextServ.Text;
                    DateTime dateValue;
                    bool isValidDate = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDate)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT id, price, serv_name, about, date FROM public.""Services""
                        WHERE public.""Services"".date = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;

            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                services.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                services.DataContext = DataSet;
            }
            connecting.Close();
        }
        private void Marketing_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchMarketing.Text == null || string.IsNullOrWhiteSpace(searchMarketing.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchMarketingText.Text == null || string.IsNullOrWhiteSpace(searchMarketingText.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql = null;
            string inputValue = searchMarketingText.Text;
            DateTime dateValue;
            switch (searchMarketing.Text)
            {
                case "Название":
                    sql = @"
                       SELECT id, name, budget, date_start, date_end, target FROM public.""Marketing""
                        WHERE public.""Marketing"".name = '" + searchMarketingText.Text + "';";
                    break;
                case "Бюджет":
                    bool isValidPrice = (Regex.IsMatch(searchMarketingText.Text, @"^\d+(\,\d{1,2})?$") || Regex.IsMatch(searchMarketingText.Text, @"^\d+(\.\d{1,2})?$"));
                    if (isValidPrice)
                    {

                        sql = @"
                       SELECT id, name, budget, date_start, date_end, target FROM public.""Marketing""
                         WHERE public.""Marketing"".budget = '" + searchMarketingText.Text.Replace('.', ',') + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Дата начала":
                    inputValue = searchMarketingText.Text;

                    bool isValidDate = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDate)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT id, name, budget, date_start, date_end, target FROM public.""Marketing""
                         WHERE public.""Marketing"".date_start = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Окончания":
                    bool isValidDateEnd = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDateEnd)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                       SELECT id, name, budget, date_start, date_end, target FROM public.""Marketing""
                        WHERE public.""Marketing"".date_end = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;

            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                marketing.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                marketing.DataContext = DataSet;
            }
            connecting.Close();
        }
        private void Equipment_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchEquipment.Text == null || string.IsNullOrWhiteSpace(searchEquipment.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchEquipmentText.Text == null || string.IsNullOrWhiteSpace(searchEquipmentText.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql = null;
            string inputValue = searchEquipmentText.Text;
            DateTime dateValue;
            switch (searchEquipment.Text)
            {
                case "Тип":
                    bool hasDigits = Regex.IsMatch(searchSupplyText.Text, @"\d");
                    if (hasDigits)
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    else
                    {
                        sql = @"
                       SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id
                        WHERE public.""Eqp_category"".cat_name = '" + searchEquipmentText.Text + "';";
                    }
                    break;
                case "Название":
                    sql = @"
                      SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id
                        WHERE public.""Equipment"".name = '" + searchEquipmentText.Text.Replace('.', ',') + "';";
                    break;
                case "Ответственный":
                    bool isOnlyLetters = Regex.IsMatch(searchEquipmentText.Text, @"^[A-Z][a-z]*$");
                    if (isOnlyLetters)
                    {
                        sql = @"
                        SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id
                        WHERE public.""Users"".fio LIKE '" + searchEquipmentText.Text + "%';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Срок годности":
                    bool isValidDateEnd = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDateEnd)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id
                        WHERE public.""Equipment"".suitability = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Документ":
                    bool isValidDock = Regex.IsMatch(searchEquipmentText.Text, @"\d");
                    if (isValidDock)
                    {
                        sql = @"
                       SELECT public.""Equipment"".id, public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id
                        WHERE public.""Contracts"".interior_number = '" + searchEquipmentText.Text + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;

            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                equipment.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                equipment.DataContext = DataSet;
            }
            connecting.Close();
        }
        private void Contracts_0_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchContracts0.Text == null || string.IsNullOrWhiteSpace(searchContracts0.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchContracts0Text.Text == null || string.IsNullOrWhiteSpace(searchContracts0Text.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql = null;
            switch (searchContracts0.Text)
            {
                case "Номер":
                    bool hasDigits = Regex.IsMatch(searchContracts0Text.Text, @"\d");
                    if (hasDigits)
                    {
                        sql = @"
                       SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0 AND public.""Contracts"".interior_number = '" + searchContracts0Text.Text + "';";
                        
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Организация":
                    sql = @"
                      SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0 AND public.""Organization"".name = '" + searchContracts0Text.Text + "';";
                    break;
                case "Сумма":
                    bool isValidPrice = (Regex.IsMatch(searchContracts0Text.Text, @"^\d+(\,\d{1,2})?$") || Regex.IsMatch(searchContracts0Text.Text, @"^\d+(\.\d{1,2})?$"));
                    if (isValidPrice)
                    {
                        sql = @"
                       SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0 AND public.""Contracts"".sum = '" + searchContracts0Text.Text.Replace('.', ',') + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Дата окончания":
                    string inputValue = searchContracts0Text.Text;
                    DateTime dateValue;
                    bool isValidDate = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDate)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0 AND public.""Contracts"".date_end =  = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;

            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                contracts_0.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                contracts_0.DataContext = DataSet;
            }
            connecting.Close();


        } 
        private void Contracts_2_find(object sender, RoutedEventArgs e)
        {

            //Проверка, что выбран параметр для поиска
            if (searchContracts2.Text == null || string.IsNullOrWhiteSpace(searchContracts2.Text)) { MessageBox.Show("Выберите поле для поиска"); return; }
            //Проверка, что введено значение для поиска
            else if (searchContracts2Text.Text == null || string.IsNullOrWhiteSpace(searchContracts2Text.Text)) { MessageBox.Show("Введите данные для поиска"); return; }
            //Поиск в базе указанных значений
            connecting.Open();
            string sql = null;
            switch (searchContracts2.Text)
            {
                case "Номер":
                    bool hasDigits = Regex.IsMatch(searchContracts2Text.Text, @"\d");
                    if (hasDigits)
                    {
                        sql = @"
                       SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2 AND public.""Contracts"".interior_number = '" + searchContracts2Text.Text + "';";

                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Организация":
                    sql = @"
                      SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2 AND public.""Organization"".name = '" + searchContracts2Text.Text + "';";
                    break;
                case "Сумма":
                    bool isValidPrice = (Regex.IsMatch(searchContracts2Text.Text, @"^\d+(\,\d{1,2})?$") || Regex.IsMatch(searchContracts2Text.Text, @"^\d+(\.\d{1,2})?$"));
                    if (isValidPrice)
                    {
                        sql = @"
                       SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2 AND public.""Contracts"".sum = '" + searchContracts2Text.Text.Replace('.', ',') + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;
                case "Дата окончания":
                    string inputValue = searchContracts2Text.Text;
                    DateTime dateValue;
                    bool isValidDate = DateTime.TryParseExact(inputValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (isValidDate)
                    {
                        string formattedDate = dateValue.ToString("yyyy-MM-dd");
                        sql = @"
                        SELECT public.""Contracts"".id, public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2 AND public.""Contracts"".date_end = '" + formattedDate + "';";
                    }
                    else
                    {
                        MessageBox.Show("Проверьте введенное значение");
                    }
                    break;

            }
            if (sql != null)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                     //
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);                     //  Выгрузка данных из БД и создание DataSet 
                DataSet DataSet = new DataSet();                                            //  для их хранения
                Adapter.Fill(DataSet, "DataBD");                                            //
                                                                                            //Запрет на изменение таблицы equipment
                contracts_2.IsReadOnly = true;
                //Заполнение таблицы services данными, выгруженными из БД
                contracts_2.DataContext = DataSet;
            }
            connecting.Close();


        }
        private void Service_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"SELECT serv_name, about, price, channels, cinema, mobile_connection, equipment, speed, video, date FROM public.""Services"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Название", "Описание", "Цена", "Каналы", "Онлайн-кинотеатр", "Мобильная связь", "Оборудование", "Скорость интернета", "Видеозапись", "Дата" };
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        if (value is bool boolValue)
                        {
                            value = boolValue ? "Да" : "Нет";
                        }
                        else if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        if (col == 0)
                        {
                            Excel.Range cellRange = sheet.Cells[row + 2, col + 1];
                            cellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                        if (col == 7)
                        {
                            if ((int)value < 100)
                            {
                                value += " Gb/s";
                            } else
                            {
                                value += " mb/s";
                            }
                        }
                        if (col==2)
                        {
                            value += " \u20BD";
                            
                        }
                        sheet.Cells[row + 2, col + 1] = value;
                    }
                }

                // Автонастройка ширины столбцов
                sheet.Columns.AutoFit();

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Marketing_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"SELECT name, budget, date_start, date_end, target, actions FROM public.""Marketing"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Название", "Бюджет", "Дата начала", "Дата окончания", "Цель", "Действия"};
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        
                        if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        if (col == 0)
                        {
                            Excel.Range cellRange = sheet.Cells[row + 2, col + 1];
                            cellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                        if (col == 1)
                        {
                            value += " \u20BD";

                        }
                        if (value is string[])
                        {
                            if (value is Array arrayValue)
                            {
                                // Приведение типа к ожидаемому типу элементов массива (в данном случае string[])
                                string[] stringArray = arrayValue.Cast<string>().ToArray();
                                // Создание нового массива с числами перед каждым элементом
                                string[] numberedArray = new string[stringArray.Length];
                                for (int i = 0; i < stringArray.Length; i++)
                                {
                                    numberedArray[i] = $"{i + 1}. {stringArray[i]}";
                                }
                                // Объединение элементов массива в строку, разделенную переносами строк
                                string joinedString = string.Join(Environment.NewLine, numberedArray);
                                value = joinedString;

                            }
                        }
                        sheet.Cells[row + 2, col + 1] = value.ToString().TrimEnd();
                        Excel.Range columnRange = sheet.Cells[2, col + 1];
                        
                    }
                }


                // Автонастройка ширины столбцов
                for (int col = 1; col <= ct.Columns.Count; col++)
                {
                    Excel.Range columnRange = sheet.Columns[col];
                    columnRange.AutoFit();
                }
                Excel.Range allColumnsRange = sheet.UsedRange;
                allColumnsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Equipment_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"
                    SELECT public.""Eqp_category"".cat_name, public.""Equipment"".name, public.""Users"".fio, public.""Equipment"".suitability, public.""Contracts"".interior_number, public.""Equipment"".ip
                    FROM public.""Equipment""
                    JOIN public.""Eqp_category"" ON public.""Equipment"".category_id = public.""Eqp_category"".id
                    JOIN public.""Users"" ON public.""Equipment"".responsible_id = public.""Users"".id
                    JOIN public.""Contracts"" ON public.""Equipment"".doc_number = public.""Contracts"".id;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Категория", "Название", "Ответственный", "Срок использования", "Номер документа", "ip"};
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        sheet.Cells[row + 2, col + 1] = value.ToString().TrimEnd();
                        Excel.Range columnRange = sheet.Cells[2, col + 1];
                        
                    }
                }


                // Автонастройка ширины столбцов
                for (int col = 1; col <= ct.Columns.Count; col++)
                {
                    Excel.Range columnRange = sheet.Columns[col];
                    columnRange.AutoFit();
                }
                Excel.Range allColumnsRange = sheet.UsedRange;
                allColumnsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Supply_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"
                        SELECT public.""Supply_status"".name, public.""Supply"".about, public.""Supply"".summ, public.""Supply"".date, public.""Contracts"".interior_number
                        FROM public.""Supply""
                        JOIN public.""Contracts"" ON public.""Supply"".contract_id = public.""Contracts"".id
                        JOIN public.""Supply_status"" ON public.""Supply"".status_id = public.""Supply_status"".id";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Статус", "Описание", "Сумма", "Дата поставки", "Номер контракта"};
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        if (col == 2)
                        {
                            value += " \u20BD";

                        }
                        sheet.Cells[row + 2, col + 1] = value.ToString().TrimEnd();
                        Excel.Range columnRange = sheet.Cells[2, col + 1];
                        
                    }
                }


                // Автонастройка ширины столбцов
                for (int col = 1; col <= ct.Columns.Count; col++)
                {
                    Excel.Range columnRange = sheet.Columns[col];
                    columnRange.AutoFit();
                }
                Excel.Range allColumnsRange = sheet.UsedRange;
                allColumnsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Contracts0_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"
                        SELECT public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 0";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Внутренний номер", "Организация", "Сумма", "Дата окончания"};
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        if (col == 2)
                        {
                            value += " \u20BD";

                        }
                        sheet.Cells[row + 2, col + 1] = value.ToString().TrimEnd();
                        Excel.Range columnRange = sheet.Cells[2, col + 1];
                        
                    }
                }


                // Автонастройка ширины столбцов
                for (int col = 1; col <= ct.Columns.Count; col++)
                {
                    Excel.Range columnRange = sheet.Columns[col];
                    columnRange.AutoFit();
                }
                Excel.Range allColumnsRange = sheet.UsedRange;
                allColumnsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Contracts2_report(object sender, RoutedEventArgs e)
        {
            try
            {
                connecting.Open();
                // Строка с SQL-запросом
                string sql = @"
                        SELECT public.""Contracts"".interior_number, public.""Organization"".name, public.""Contracts"".sum, public.""Contracts"".date_end
                    FROM public.""Contracts""
                    JOIN public.""Organization"" ON public.""Contracts"".org_id = public.""Organization"".id
                    WHERE public.""Contracts"".type_id = 2";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd);
                DataTable ct = new DataTable();
                Adapter.Fill(ct);
                connecting.Close();

                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook workbook = objExcel.Workbooks.Add();
                Excel.Worksheet sheet = workbook.ActiveSheet;

                // Запись заголовков столбцов
                string[] columnNames = { "Внутренний номер", "Организация", "Сумма", "Дата окончания"};
                for (int col = 0; col < columnNames.Length; col++)
                {
                    sheet.Cells[1, col + 1] = columnNames[col];
                    // Применение форматирования к заголовкам
                    Excel.Range headerRange = sheet.Cells[1, col + 1];
                    headerRange.Font.Bold = true;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Запись данных из DataTable
                for (int row = 0; row < ct.Rows.Count; row++)
                {
                    for (int col = 0; col < ct.Columns.Count; col++)
                    {
                        // Преобразование значений и запись в ячейку
                        object value = ct.Rows[row][col];
                        if (value is DateTime dateTimeValue)
                        {
                            value = dateTimeValue.ToShortDateString();
                        }
                        if (col == 2)
                        {
                            value += " \u20BD";

                        }
                        sheet.Cells[row + 2, col + 1] = value.ToString().TrimEnd();
                        Excel.Range columnRange = sheet.Cells[2, col + 1];
                        
                    }
                }


                // Автонастройка ширины столбцов
                for (int col = 1; col <= ct.Columns.Count; col++)
                {
                    Excel.Range columnRange = sheet.Columns[col];
                    columnRange.AutoFit();
                }
                Excel.Range allColumnsRange = sheet.UsedRange;
                allColumnsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                // Отображение Excel
                objExcel.Visible = true;
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void Reload(object sender, RoutedEventArgs e)
        {
            Services_table();
            Marketing_table();
            Equipment_table();
            Contracts_0_table();
            Supply_table();
            Contract_2_table();
        }
    }
}
