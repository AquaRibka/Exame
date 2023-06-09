using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Dropbox.Api.TeamLog.FedExtraDetails;

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Supply_add.xaml
    /// </summary>
    public partial class Supply_add : Window
    {
        readonly string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public string IdData { get; set; }
        public Supply_add()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connecting = new NpgsqlConnection(connectPostgre);
            try
            {

                connecting.Open();

                string sql = @"
                   SELECT id, interior_number FROM public.""Contracts"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataTable iDataTable = new DataTable();
                iAdapter.Fill(iDataTable);
                Contract.Items.Clear();

                // Добавление элементов в ComboBox из данных таблицы
                foreach (DataRow row in iDataTable.Rows)
                {
                    string id = row["id"].ToString();
                    string name = row["interior_number"].ToString();

                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = name,
                        Tag = id
                    };

                    Contract.Items.Add(item);
                }
                string sql_org = @"
                   SELECT id, name FROM public.""Organization"";";
                NpgsqlCommand cmd_org = new NpgsqlCommand(sql_org, connecting);
                NpgsqlDataAdapter Adapter = new NpgsqlDataAdapter(cmd_org);
                DataTable DataTable = new DataTable();
                Adapter.Fill(DataTable);
                Organization.Items.Clear();

                // Добавление элементов в ComboBox из данных таблицы
                foreach (DataRow row in DataTable.Rows)
                {
                    string id = row["id"].ToString();
                    string name = row["name"].ToString();

                    ComboBoxItem item_org = new ComboBoxItem
                    {
                        Content = name,
                        Tag = id
                    };

                    Organization.Items.Add(item_org);
                }
                connecting.Close();
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            UploadData();
        }
        //Выгрузка данных из бд
        private void UploadData()
        {
            if (IdData != null)
            {
                if (!string.IsNullOrEmpty(IdData))
                {
                    Cancel.Visibility = Visibility.Collapsed;
                    Del.Visibility = Visibility.Visible;
                    AddButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Visible;
                    try
                    {
                        connecting.Open();
                        //SQl запрос
                        string sql = @"
                           SELECT * FROM public.""Supply""
                           WHERE id = " + IdData + ";";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                        NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable iDataTable = new DataTable();
                        iAdapter.Fill(iDataTable);
                        foreach (DataRow row in iDataTable.Rows)
                        {
                            if (!row.IsNull("contract_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["contract_id"].ToString(); // Получаем значение

                                foreach (ComboBoxItem item in Contract.Items)
                                {
                                    if (item.Tag != null && item.Tag.ToString() == value)
                                    {
                                        Contract.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("org_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["org_id"].ToString(); // Получаем значение

                                foreach (ComboBoxItem item in Organization.Items)
                                {
                                    if (item.Tag != null && item.Tag.ToString() == value)
                                    {
                                        Organization.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("about")) // Проверка, что значение не является NULL
                            {
                                About.Text = row["about"].ToString().TrimEnd();
                            }
                            if (!row.IsNull("summ")) // Проверка, что значение не является NULL
                            {
                                Sum.Text = row["summ"].ToString();
                            }
                            if (!row.IsNull("date")) // Проверка, что значение не является NULL
                            {
                                Date.Text = ((DateTime)row["date"]).ToShortDateString();
                            }
                        }
                        connecting.Close();
                    }
                    catch (Exception ex)
                    {
                        connecting.Close();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    Cancel.Visibility = Visibility.Visible;
                    Del.Visibility = Visibility.Collapsed;
                }
            }
        }
        //Заакрытие окна
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Удаление данных
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эти данные?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = "DELETE FROM public.\"Supply\" WHERE id = " + IdData + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Запись успешно удалена.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось найти запись с указанным идентификатором.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении данных: " + ex.Message);
            }
        }
        //Добавление данных
        private void Add(object sender, RoutedEventArgs e)
        {
            //Првоерка валидности
            if (Contract.SelectedItem == null)
            {
                MessageBox.Show("Выберите контракт");
                return;
            }
            else if (Organization.SelectedItem == null)
            {
                MessageBox.Show("Выберите организацию");
                return;
            }
            else if (About.Text == null)
            {
                MessageBox.Show("Проверьте поле Описание");
                return;
            }
            else if (Sum.Text == null || !Regex.IsMatch(Sum.Text, @"^\d{1,10}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Сумма поставки");
                return;
            }
            else if (Date.Text == null || !Regex.IsMatch(Date.Text, @"\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Срок поставки");
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = @"
            INSERT INTO public.""Supply"" (contract_id, org_id, summ, about, date)
            VALUES (" + (Contract.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", " + (Organization.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", " + Sum.Text.Replace(',', '.') + ", '" + About.Text + "', '" + Date.Text.Replace(',', '.') + "') RETURNING id;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                int insertedId = (int)cmd.ExecuteScalar();
                if (insertedId > 0)
                {
                    MessageBox.Show("Значения успешно обновлены в базе данных.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось обновить значения в базе данных.");
                }

                connecting.Close();
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        //Изменеие данных
        private void Edit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //Проверка валидности
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (Contract.SelectedItem == null)
            {
                MessageBox.Show("Выберите контракт");
                return;
            }
            else if (Organization.SelectedItem == null)
            {
                MessageBox.Show("Выберите организацию");
                return;
            }
            else if (About.Text == null)
            {
                MessageBox.Show("Проверьте поле Описание");
                return;
            }
            else if (Sum.Text == null || !Regex.IsMatch(Sum.Text, @"^\d{1,10}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Сумма поставки");
                return;
            }
            else if (Date.Text == null || !Regex.IsMatch(Date.Text, @"\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Срок поставки");
                return;
            }
            try
            {
                //SQl запрос
                string sql = @"
                    UPDATE public.""Supply""
                    SET contract_id = @contractId, org_id = @orgId, about = @about, summ = @summ, date = @date
                    WHERE id = @id;";
                connecting.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting))
                {
                    //Параметры запроса
                    cmd.Parameters.AddWithValue("contractId",int.Parse((Contract.SelectedItem as ComboBoxItem)?.Tag?.ToString()));
                    cmd.Parameters.AddWithValue("orgId", int.Parse((Organization.SelectedItem as ComboBoxItem)?.Tag?.ToString()));
                    cmd.Parameters.AddWithValue("about", About.Text);
                    cmd.Parameters.AddWithValue("summ", decimal.Parse(Sum.Text.Replace('.', ',')));
                    cmd.Parameters.AddWithValue("date", DateTime.Parse(Date.Text.Replace(',', '.')));
                    cmd.Parameters.AddWithValue("id", int.Parse(IdData));
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Значения успешно обновлены в базе данных.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить значения в базе данных.");
                    }

                    connecting.Close();
                }
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        //Очистка полей ввода
        private void Clear(object sender, RoutedEventArgs e)
        {
            Contract.SelectedItem = null;
            Organization.SelectedItem = null;
            About.Text = null;
            Sum.Text = null;
            Date.Text = null;
        }
    }
}
