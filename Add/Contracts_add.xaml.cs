using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dropbox.Api;
using Dropbox.Api.Files;
using Npgsql;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Contracts_add.xaml
    /// </summary>
    public partial class Contracts_add : Window
    {
        //Токен облачного хранилища
        string token = "sl.Bf_PglTJSvxaYKweMAEhcJKnFxvX5Md25YdisQK97rZkh4f4U5oA6H7MLntbaXQP68biv1hpTzdYaMJvPLe2T9fDLh8knv5j9jvuVrHP44JPvBXHYx2jeY7xYejModMQLy-JC542dG2F";
        readonly string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        string selectedFilePath = string.Empty;
        public string IdData { get; set; }
        public Contracts_add()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connecting = new NpgsqlConnection(connectPostgre);
            try
            {
                connecting.Open();
                //SQL запрос
                string sql = @"
                   SELECT id, name FROM public.""Organization"";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataTable iDataTable = new DataTable();
                iAdapter.Fill(iDataTable);
                Organizastion.Items.Clear();
                // Добавление элементов в ComboBox из данных таблицы
                foreach (DataRow row in iDataTable.Rows)
                {
                    string id = row["id"].ToString();
                    string name = row["name"].ToString();

                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = name,
                        Tag = id
                    };

                    Organizastion.Items.Add(item);
                }
                connecting.Close();
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            Data_Upload();
        }
        //Загрузка файлов на облако
        private async Task AddFile(string id, string token)
        {
            if (!string.IsNullOrEmpty(selectedFilePath)&& !string.IsNullOrEmpty(FileNameLabel.Text)) 
            {
            var client = new DropboxClient(token);
            var fileContent = File.ReadAllBytes(selectedFilePath);
            var uploadResult = await client.Files.UploadAsync("/Contracts/"+id+".docx", WriteMode.Overwrite.Instance, body: new MemoryStream(fileContent));
            if (uploadResult.IsFile)
            {
                MessageBox.Show("Файл успешно загружен.");
            }
            else
            {
                MessageBox.Show("Не удалось сохранить файл.");
            }
            }
        }
        //Добавление данных в бд
        private async void Add(object sender, RoutedEventArgs e)
        {
            //Проверка полей ввода
            if (type.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип контракта");
                return;
            } else if (Organizastion.SelectedItem == null)
            {
                MessageBox.Show("Выберите организацию");
                return;
            } else if (Interial_number.Text == null || !Regex.IsMatch(Interial_number.Text, @"^\d+$"))
            {
                MessageBox.Show("Проверьте поле Внутренний номер");
                return;
            } else if (sum.Text == null || !Regex.IsMatch(sum.Text, @"^\d{1,7}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Сумма контракта");
                return;
            } else if (string.IsNullOrEmpty(FileNameLabel.Text))
            {
                MessageBox.Show("Добавьте файл контракта");
                return;
            } else if (dates.Text == null|| !Regex.IsMatch(dates.Text, @"\d{2}\.\d{2}\.\d{4}-\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Сроки контракта");
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = @"
            INSERT INTO public.""Contracts"" (type_id, org_id, interior_number, sum, date_start, date_end)
            VALUES (" + (type.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", " + (Organizastion.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", " + Interial_number.Text + ", " + sum.Text.Replace(',', '.') + ", @dateStart, @dateEnd)" +
            "RETURNING id; ";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                //Разделение на две даты данных из поля ввода
                string[] dateRange = dates.Text.Split('-');
            if (dateRange.Length == 2)
            {
                DateTime dateStart, dateEnd;
                 //Добавление дат параметрами
                if (DateTime.TryParse(dateRange[0].Trim(), out dateStart) && DateTime.TryParse(dateRange[1].Trim(), out dateEnd))
                {
                    cmd.Parameters.AddWithValue("@dateStart", dateStart);
                    cmd.Parameters.AddWithValue("@dateEnd", dateEnd);
                }
            }
            int insertedId = (int)cmd.ExecuteScalar();
             //Проверка изменения данных
            if (insertedId > 0)
            {
                MessageBox.Show("Значения успешно обновлены в базе данных.");
                await AddFile(insertedId.ToString(), token);
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
        //Загрузка файла пользователем
        private void Add_file(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Установка фильтра для типов файлов
            openFileDialog.Filter = "Документы (*.doc; *.docx; *.pdf)|*.doc;*.docx;*.pdf";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false; // Разрешить выбор только одного файла

            // Открытие диалогового окна выбора файла
            bool? result = openFileDialog.ShowDialog();

            // Обработка результатов выбора файла
            if (result == true)
            {

                    string selectedFileName = openFileDialog.SafeFileName;
                    FileNameLabel.Text = selectedFileName;
                selectedFilePath = openFileDialog.FileName;
                // Делайте что-то с выбранным файлом
            }
        }
        //Загрузка данных из бд
        private void Data_Upload()
        {
            if (IdData != null)
            {
                if (!string.IsNullOrEmpty(IdData))
                {
                    //Изменение кнопок
                    Cancel.Visibility = Visibility.Collapsed;
                    Del.Visibility = Visibility.Visible;
                    AddButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Visible;
                    try
                    {
                        connecting.Open();
                        //SQl запрос
                        string sql = @"
                   SELECT * FROM public.""Contracts""
                   WHERE id = " + IdData + ";";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                        NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                        DataTable iDataTable = new DataTable();
                        iAdapter.Fill(iDataTable);
                        foreach (DataRow row in iDataTable.Rows)
                        {
                            if (!row.IsNull("type_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["type_id"].ToString(); // Получаем значение

                                foreach (ComboBoxItem item in type.Items)
                                {
                                    if (item.Tag != null && item.Tag.ToString() == value)
                                    {
                                        type.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("org_id")) // Проверка, что значение не является NULL
                            {
                                string value = row["org_id"].ToString(); // Получаем значение

                                foreach (ComboBoxItem item in Organizastion.Items)
                                {
                                    if (item.Tag != null && item.Tag.ToString() == value)
                                    {
                                        Organizastion.SelectedItem = item; // Устанавливаем элемент в качестве выбранного
                                        break;
                                    }
                                }
                            }
                            if (!row.IsNull("interior_number")) // Проверка, что значение не является NULL
                            {
                                Interial_number.Text = row["interior_number"].ToString();
                            }
                            if (!row.IsNull("sum")) // Проверка, что значение не является NULL
                            {
                                sum.Text = row["sum"].ToString();
                            }
                            if (!row.IsNull("date_start")&& !row.IsNull("date_end")) // Проверка, что значение не является NULL
                            {
                                dates.Text = ((DateTime)row["date_start"]).ToShortDateString() + "-" + ((DateTime)row["date_end"]).ToShortDateString();
                            }
                            FileNameLabel.Text = row["id"].ToString() +".docx";
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
        private void Clear(object sender, RoutedEventArgs e)
        {
            type.SelectedItem = null;
            Organizastion.SelectedItem= null;
            Interial_number.Text = null;
            sum.Text = null;
            FileNameLabel.Text = null;
            dates.Text = null;
        }
        //Изменение данных
        private async void Edit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //Проверка вадидности введенных данных
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (type.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип контракта");
                return;
            }
            else if (Organizastion.SelectedItem == null)
            {
                MessageBox.Show("Выберите организацию");
                return;
            }
            //Проверка вадидности номера контракта
            else if (Interial_number.Text == null || !Regex.IsMatch(Interial_number.Text, @"^\d+$"))
            {
                MessageBox.Show("Проверьте поле Внутренний номер");
                return;
            }
            //Проверка вадидности суммы
            else if (sum.Text == null || !Regex.IsMatch(sum.Text, @"^\d{1,10}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Сумма контракта");
                return;
            }
            else if (FileNameLabel.Text == "")
            {
                MessageBox.Show("Добавьте файл контракта");
                return;
            }
            //Проверка вадидности даты
            else if (dates.Text == null || !Regex.IsMatch(dates.Text, @"\d{2}\.\d{2}\.\d{4}-\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Сроки контракта");
                return;
            }
            try
            {
                connecting.Open();
                //SQl запрос
                string sql = @"
        UPDATE public.""Contracts""
        SET type_id = " + (type.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", org_id = "+ (Organizastion.SelectedItem as ComboBoxItem)?.Tag?.ToString() + ", interior_number = " + Interial_number.Text + ", sum = "+ sum.Text.Replace(',', '.') + ", date_start = @dateStart, date_end = @dateEnd " +
        "WHERE id = "+IdData+";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                //Разделение дат из поля ввода
                string[] dateRange = dates.Text.Split('-');
                if (dateRange.Length == 2)
                {
                    DateTime dateStart, dateEnd;
                    if (DateTime.TryParse(dateRange[0].Trim(), out dateStart) && DateTime.TryParse(dateRange[1].Trim(), out dateEnd))
                    {
                        cmd.Parameters.AddWithValue("@dateStart", dateStart);
                        cmd.Parameters.AddWithValue("@dateEnd", dateEnd);
                    }
                }
                //Загрузка файла на облачное хранилище
                await AddFile(IdData, token);
                int rowsAffected = cmd.ExecuteNonQuery();
                //Проверка добавления данных
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
        //ЗАкрытие окна
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Удаление данных из базы
        private async void Del_Click(object sender, RoutedEventArgs e)
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
                string sql = "DELETE FROM public.\"Contracts\" WHERE id = "+IdData+";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
                await DelFile(IdData, token);
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
        //удаление файла из облака
        private async Task DelFile(string id, string token)
        {
            if (!string.IsNullOrEmpty(selectedFilePath) && !string.IsNullOrEmpty(FileNameLabel.Text))
            { 
                //Соединение с DropBox
                var client = new DropboxClient(token);
                var deleteResult = await client.Files.DeleteV2Async("/Contracts/" + id + ".docx");
                if (deleteResult.Metadata != null)
                {
                    MessageBox.Show("Файл успешно удален.");
                }
                else
                {
                    MessageBox.Show("Не удалось удалить файл.");
                }
            }
        }
    }
}
