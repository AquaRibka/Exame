using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
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
    /// Логика взаимодействия для Service_add.xaml
    /// </summary>
    public partial class Service_add : Window
    {
        string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public string IdData { get; set; }
        public Service_add()
        {
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();
            SpeedBox.ItemsSource = speed;
            Channels.ItemsSource = boolchouse;
            Cinema.ItemsSource = boolchouse;
            mobileConnection.ItemsSource = boolchouse;
            Video.ItemsSource = boolchouse;
            Equipment.ItemsSource = boolchouse;
        }
        //Классы для хранения данных
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Data_Upload();
        }
        //ВЫгрузка данных из БД
        private void Data_Upload()
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
                           SELECT * FROM public.""Services""
                           WHERE id = " + IdData + ";";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                        NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable iDataTable = new DataTable();
                        iAdapter.Fill(iDataTable);
                        foreach (DataRow row in iDataTable.Rows)
                        {
                            if (!row.IsNull("channels")) // Проверка, что значение не является NULL
                            {
                                string value = row["channels"].ToString(); // Получаем значение

                                foreach (BoolFind item in Channels.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Channels.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("cinema")) // Проверка, что значение не является NULL
                            {
                                string value = row["cinema"].ToString(); // Получаем значение

                                foreach (BoolFind item in Cinema.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Cinema.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("mobile_connection")) // Проверка, что значение не является NULL
                            {
                                string value = row["mobile_connection"].ToString(); // Получаем значение

                                foreach (BoolFind item in mobileConnection.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        mobileConnection.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("equipment")) // Проверка, что значение не является NULL
                            {
                                string value = row["equipment"].ToString(); // Получаем значение

                                foreach (BoolFind item in Equipment.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Equipment.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("video")) // Проверка, что значение не является NULL
                            {
                                string value = row["video"].ToString(); // Получаем значение

                                foreach (BoolFind item in Video.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Video.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            } 
                            if (!row.IsNull("speed")) // Проверка, что значение не является NULL
                            {
                                string value = row["speed"].ToString(); // Получаем значение

                                foreach (Speed item in SpeedBox.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        SpeedBox.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("serv_name")) // Проверка, что значение не является NULL
                            {
                                name.Text = row["serv_name"].ToString().TrimEnd();
                            }
                            if (!row.IsNull("about")) // Проверка, что значение не является NULL
                            {
                                about.Text = row["about"].ToString().TrimEnd();
                            }
                            if (!row.IsNull("price")) // Проверка, что значение не является NULL
                            {
                                price.Text = row["price"].ToString().TrimEnd();
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
        //Очистка полей ввода
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Channels.SelectedItem = null;
            Cinema.SelectedItem = null;
            mobileConnection.SelectedItem = null;
            Equipment.SelectedItem = null;
            Video.SelectedItem = null;
            SpeedBox.SelectedItem = null;
            name.Text = null;
            about.Text = null;
            price.Text = null;
        }
        //Закрытие окна
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //УУдаление данных
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
                string sql = "DELETE FROM public.\"Services\" WHERE id = " + IdData + ";";
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
        //Изменение данных
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //Проверка валидности
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (Channels.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Каналы");
                return;
            }
            else if (Cinema.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Онлайн-кинотеатр");
                return;
            }
            else if (mobileConnection.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Мобильное соединение");
                return;
            }
            else if (Equipment.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле оборудование");
                return;
            }
            else if (Video.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Видеонаблюдение");
                return;
            }
            else if (SpeedBox.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Скорость интернета");
                return;
            }
            else if (name.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (about.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (price.Text == null || !Regex.IsMatch(price.Text, @"^\d{1,7}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Срок использования");
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = @"UPDATE public.""Services""
                SET cinema = @cinema, mobile_connection = @mobile_connection, equipment = @equipment, video = @video, channels = @channels, speed = @speed,
                serv_name = @serv_name, about = @about, price = @price
                WHERE id = @id";
                using (var command = new NpgsqlCommand(sql, connecting))
                {
                    //Параметры запроса
                    command.Parameters.AddWithValue("@cinema", (Cinema.SelectedItem as BoolFind)?.Value);
                    command.Parameters.AddWithValue("@mobile_connection", (mobileConnection.SelectedItem as BoolFind)?.Value);
                    command.Parameters.AddWithValue("@equipment", (Equipment.SelectedItem as BoolFind)?.Value);
                    command.Parameters.AddWithValue("@video", (Video.SelectedItem as BoolFind)?.Value);
                    command.Parameters.AddWithValue("@channels", (Channels.SelectedItem as BoolFind)?.Value);
                    command.Parameters.AddWithValue("@speed", (SpeedBox.SelectedItem as Speed)?.Value);
                    command.Parameters.AddWithValue("@serv_name", name.Text.TrimEnd());
                    command.Parameters.AddWithValue("@about", about.Text.TrimEnd());
                    command.Parameters.AddWithValue("@price", decimal.Parse(price.Text.TrimEnd().Replace('.', ',')));
                    command.Parameters.AddWithValue("@id", int.Parse(IdData));
                    int rowsAffected = command.ExecuteNonQuery();
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
        //Добавление данных
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Првоерка валидности
            if (Channels.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Каналы");
                return;
            }
            else if (Cinema.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Онлайн-кинотеатр");
                return;
            }
            else if (mobileConnection.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Мобильное соединение");
                return;
            }
            else if (Equipment.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле оборудование");
                return;
            }
            else if (Video.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Видеонаблюдение");
                return;
            }
            else if (SpeedBox.SelectedItem == null)
            {
                MessageBox.Show("Проверьте поле Скорость интернета");
                return;
            }
            else if (name.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (about.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (price.Text == null || !Regex.IsMatch(price.Text, @"^\d{1,7}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Срок использования");
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = @"
            INSERT INTO public.""Services"" (cinema, mobile_connection, equipment, video, channels, speed, serv_name, about, price)
            VALUES (" + (Cinema.SelectedItem as BoolFind)?.Value + ", " + (mobileConnection.SelectedItem as BoolFind)?.Value + ", " + (Equipment.SelectedItem as BoolFind)?.Value + ", " + (Video.SelectedItem as BoolFind)?.Value + ", " + (Channels.SelectedItem as BoolFind)?.Value + ", "+ (SpeedBox.SelectedItem as Speed)?.Value + ", '"+ name.Text.TrimEnd() + "', '"+ about.Text.TrimEnd() + "', " + price.Text.Replace('.', ',').TrimEnd() + ") RETURNING id;";
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
    }
}
