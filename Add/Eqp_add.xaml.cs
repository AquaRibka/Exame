using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using static Model_eTOM.Add.Service_add;

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Eqp_add.xaml
    /// </summary>
    public partial class Eqp_add : Window
    {        
        readonly string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public string IdData { get; set; }
        public Eqp_add()
        {
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();
            LoadCab();
            LoadCategory();
            LoadUsers();
            LoadDocs();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           DataUpload(IdData);
        }
        public class Cab
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }
        List<Cab> cabs = new List<Cab>();
        private void LoadCab()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id,name FROM public.""Cabinet"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    cabs.Add(new Cab { Value = id, Display = name });
                }
                connecting.Close();
                Cabinet.ItemsSource = cabs;
            }

            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        public class User
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }
        List<User> users = new List<User>();
        private void LoadUsers()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, fio FROM public.""Users"" WHERE type_id = 6;";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    users.Add(new User { Value = id, Display = name });
                }
                connecting.Close();
                Responsible.ItemsSource = users;
            }

            //Вывод ошибок при выполнении кода
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error" + ex.Message);
            }
        }
        public class Doc
        {
            public int Value { get; set; }
            public string Display { get; set; }
        }
        List<Doc> docs = new List<Doc>();
        private void LoadDocs()
        {
            try
            {
                connecting.Open();
                string sql = @"SELECT id, interior_number FROM public.""Contracts"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetInt32(1).ToString();
                    docs.Add(new Doc { Value = id, Display = name });
                }
                connecting.Close();
                doc.ItemsSource = docs;
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
        private void DataUpload(string IdData)
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
                        string sql = @"
                   SELECT * FROM public.""Equipment""
                   WHERE id = " + IdData + ";";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                        NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable iDataTable = new DataTable();
                        iAdapter.Fill(iDataTable);
                        foreach (DataRow row in iDataTable.Rows)
                        {
                            if (!row.IsNull("category_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["category_id"].ToString(); // Получаем значение из определенного столбца

                                foreach (Cat item in Category.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Category.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("responsible_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["responsible_id"].ToString(); // Получаем значение из определенного столбца

                                foreach (User item in Responsible.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Responsible.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("cab_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["cab_id"].ToString(); // Получаем значение из определенного столбца

                                foreach (Cab item in Cabinet.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        Cabinet.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("doc_number")) // Проверка, что значение не является NULL
                            {
                                string value = row["doc_number"].ToString(); // Получаем значение из определенного столбца

                                foreach (Doc item in doc.Items)
                                {
                                    if (item.Value.ToString() == value)
                                    {
                                        doc.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("buy_place")) // Проверка, что значение не является NULL
                            {
                                Buy_place.Text = row["buy_place"].ToString().TrimEnd();
                            }
                            if (!row.IsNull("suitability")) // Проверка, что значение не является NULL
                            {
                                Suitability.Text = ((DateTime)row["suitability"]).ToString("dd.MM.yyyy");
                            }
                            if (!row.IsNull("ip")) // Проверка, что значение не является NULL
                            {
                                ip.Text = row["ip"].ToString().TrimEnd().TrimEnd();
                            }
                            if (!row.IsNull("name")) // Проверка, что значение не является NULL
                            {
                                name.Text = row["name"].ToString().TrimEnd();
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
        private void Clear(object sender, RoutedEventArgs e)
        {
            Category.SelectedItem = null;
            Responsible.SelectedItem = null;
            Cabinet.SelectedItem = null;
            doc.SelectedItem = null;
            Buy_place.Text = null;
            Suitability.Text = null;
            ip.Text = null;
            name.Text = null;
        }
        private void Edit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (Category.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
            else if (Responsible.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответственного");
                return;
            }
            else if (Cabinet.SelectedItem == null)
            {
                MessageBox.Show("Выберите кабинет");
                return;
            }
            else if (doc.SelectedItem == null)
            {
                MessageBox.Show("Выберите документ");
                return;
            }
            else if (ip.Text == null || !Regex.IsMatch(ip.Text, @"^(?:\d{1,3}\.){3}\d{1,3}$"))
            {
                MessageBox.Show("Проверьте поле ip");
                return;
            }
            else if (name.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (Suitability.Text == null || !Regex.IsMatch(Suitability.Text, @"\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Срок использования");
                return;
            }
            try
            {
                connecting.Open();

                string sql = @"
                UPDATE public.""Equipment""
                SET category_id = " + (Category.SelectedItem as Cat)?.Value + ", responsible_id = " + (Responsible.SelectedItem as User)?.Value + ", cab_id = " + (Cabinet.SelectedItem as Cab)?.Value + ", doc_number = " + (doc.SelectedItem as Doc)?.Value +
                ", buy_place = '" + Buy_place.Text.TrimEnd() + "', ip = '" + ip.Text.Replace(',', '.').TrimEnd() + "', suitability = '" + Suitability.Text.TrimEnd() + "', name = '" + name.Text +
                "' WHERE id = " + IdData + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);                
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
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
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
                string sql = "DELETE FROM public.\"Equipment\" WHERE id = " + IdData + ";";
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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Category.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
            else if (Responsible.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответственного");
                return;
            }
            else if (Cabinet.SelectedItem == null)
            {
                MessageBox.Show("Выберите кабинет");
                return;
            }
            else if (doc.SelectedItem == null)
            {
                MessageBox.Show("Выберите документ");
                return;
            }
            else if (ip.Text == null || !Regex.IsMatch(ip.Text, @"^(?:\d{1,3}\.){3}\d{1,3}$"))
            {
                MessageBox.Show("Проверьте поле ip");
                return;
            }
            else if (name.Text == "")
            {
                MessageBox.Show("Проверьте название");
                return;
            }
            else if (Suitability.Text == null || !Regex.IsMatch(Suitability.Text, @"\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Срок использования");
                return;
            }
            try
            {
                connecting.Open();
                string sql = @"
                    INSERT INTO public.""Equipment"" (status_id, category_id, responsible_id, cab_id, doc_number, buy_place, ip, suitability, name)
                    VALUES (3," + (Category.SelectedItem as Cat)?.Value + ", " + (Responsible.SelectedItem as User)?.Value + ", " + (Cabinet.SelectedItem as Cab)?.Value + ", " + (doc.SelectedItem as Doc)?.Value +
                    ", '" + Buy_place.Text.TrimEnd() + "', '" + ip.Text.Replace(',', '.').TrimEnd() + "', '" + Suitability.Text.TrimEnd() + "', '" + name.Text + "');";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
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
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
