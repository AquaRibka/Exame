using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
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


namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Mark_add.xaml
    /// </summary>
    public partial class Mark_add : Window
    {
        private int planCount = 0;
        string connectPostgre = ConfigurationManager.ConnectionStrings["ConnectBD"].ConnectionString;
        private NpgsqlConnection connecting;
        public string IdData { get; set; }
        public Mark_add()
        {
            connecting = new NpgsqlConnection(connectPostgre);
            InitializeComponent();

        }
        private void PlanContainerAdd_Click(object sender, RoutedEventArgs e)
        {
            PlanContainerAdd();
        }
        private void PlanContainerAdd()
        {
            var app = Application.Current;
            if (planCount < 10)
            {
                // Создать новый TextBox
                TextBox newTextBox = new TextBox();
                Button newButton = new Button();
                // Присвоить уникальный идентификатор
                newTextBox.Name = "TextBox_" + planCount;
                newTextBox.TextAlignment = TextAlignment.Justify;
                newTextBox.Margin = new Thickness(0, 0, 0, 10);
                newTextBox.Style = app.Resources["TextBoxModuleSecond"] as Style;
                newButton.Tag = planCount.ToString();
                newButton.Name = "Button_" + planCount.ToString();
                newButton.Content = "⁻";
                newButton.Margin = new Thickness(0, 0, 0, 10);
                newButton.Width = 40;
                newButton.FontSize = 30;
                newButton.FontWeight = FontWeights.Bold;
                newButton.Click += PlanContainerRemove_Click;
                // Добавить TextBox в контейнер
                planContainer.Children.Add(newTextBox);
                planContainerButton.Children.Add(newButton);
                planCount++;
            }
            else MessageBox.Show("Максимум 10 полей");
        }
        private void PlanContainerRemove_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string data = button.Tag as string;
            TextBox textBoxToRemove = planContainer.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "TextBox_" + data);
            Button buttonToRemove = planContainerButton.Children.OfType<Button>().FirstOrDefault(btn => btn.Tag as string == data);
            planContainer.Children.Remove(textBoxToRemove);
            planContainerButton.Children.Remove(buttonToRemove);
            planCount--;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {

            if (Dates.Text == null || !Regex.IsMatch(Dates.Text, @"\d{2}\.\d{2}\.\d{4}-\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Сроки");
                return;
            }
            else if (Budget.Text == null || !Regex.IsMatch(Budget.Text, @"^\d{1,10}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Бюджет");
                return;
            }
            else if (Target.Text == null)
            {
                MessageBox.Show("Проверьте цель");
                return;
            }
            else if (Contract.SelectedIndex == -1)
            {
                MessageBox.Show("Проверьте Контракт");
                return;
            }
            List<string> updatedActions = new List<string>();

            for (int i = 0; i < planCount; i++)
            {
                TextBox textBox = planContainer.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "TextBox_" + i);
                if (textBox != null)
                {
                    // Получить значение из TextBox
                    string updatedValue = textBox.Text;

                    // Добавить значение в новый массив
                    updatedActions.Add(updatedValue);
                }
            }
            string name = Name.Text;
            string budget = Budget.Text;
            string target = Target.Text;

            int contractId = Contract.SelectedIndex;

            string sql = @"
            INSERT INTO public.""Marketing"" (name, budget, target, contract_id, date_start, date_end, actions)
            VALUES ('"+name+"', "+budget+", '" + target+"', " + contractId + ", @DateStart, @DateEnd, @Actions) RETURNING id;;";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
            string[] dateRange = Dates.Text.Split('-');
            if (dateRange.Length == 2)
            {
                DateTime dateStart, dateEnd;
                if (DateTime.TryParse(dateRange[0].Trim(), out dateStart) && DateTime.TryParse(dateRange[1].Trim(), out dateEnd))
                {
                    cmd.Parameters.AddWithValue("@dateStart", dateStart);
                    cmd.Parameters.AddWithValue("@dateEnd", dateEnd);
                }
            }
            cmd.Parameters.AddWithValue("Actions", updatedActions.ToArray());
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
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (Dates.Text == null || !Regex.IsMatch(Dates.Text, @"\d{2}\.\d{2}\.\d{4}-\d{2}\.\d{2}\.\d{4}"))
            {
                MessageBox.Show("Проверьте поле Сроки");
                return;
            }
            else if (Budget.Text == null || !Regex.IsMatch(Budget.Text, @"^\d{1,10}([.,]\d{1,2})?$"))
            {
                MessageBox.Show("Проверьте поле Бюджет");
                return;
            }
            else if (Target.Text == null)
            {
                MessageBox.Show("Проверьте цель");
                return;
            }else if (Contract.SelectedIndex == -1)
            {
                MessageBox.Show("Проверьте Контракт");
                return;
            }
            List<string> updatedActions = new List<string>();

            for (int i = 0; i < planCount; i++)
            {
                TextBox textBox = planContainer.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "TextBox_" + i);
                if (textBox != null)
                {
                    // Получить значение из TextBox
                    string updatedValue = textBox.Text;

                    // Добавить значение в новый массив
                    updatedActions.Add(updatedValue);
                }
            }
            string name = Name.Text;
            string budget = Budget.Text;
            string target = Target.Text;
            
            int contractId = Contract.SelectedIndex;

            string sql = @"
    UPDATE public.""Marketing""
    SET name = '" + name + "', budget = '" + budget + "', target = '" + target + "', contract_id = " + contractId + ", date_start = @dateStart, date_end = @dateEnd, actions = @Actions " +
    "WHERE id = " + IdData + "; ";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, connecting);
            string[] dateRange = Dates.Text.Split('-');
            if (dateRange.Length == 2)
            {
                DateTime dateStart, dateEnd;
                if (DateTime.TryParse(dateRange[0].Trim(), out dateStart) && DateTime.TryParse(dateRange[1].Trim(), out dateEnd))
                {
                    cmd.Parameters.AddWithValue("@dateStart", dateStart);
                    cmd.Parameters.AddWithValue("@dateEnd", dateEnd);
                }
            }
            cmd.Parameters.AddWithValue("Actions", updatedActions.ToArray());
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Значения успешно обновлены в базе данных.");
            }
            else
            {
                MessageBox.Show("Не удалось обновить значения в базе данных.");
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = null;
            Budget.Text = null;
            Target.Text = null;
            Dates.Text = null;
            Contract.SelectedItem = null;
            foreach (TextBox textBox in planContainer.Children.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
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
                string sql = "DELETE FROM public.\"Marketing\" WHERE id = " + IdData + ";";
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
               
                connecting.Close();
            }
            catch (Exception ex)
            {
                connecting.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            Data_Upload();
        }
        public class MarketingItem
        {
            public string name { get; set; }
            public decimal budget { get; set; }
            public string target { get; set; }
            public int contract_id { get; set; }
            public string[] actions { get; set; }
            public DateTime date_start { get; set; }
            public DateTime date_end { get; set; }

        }
        private void Data_Upload()
        {
            connecting.Open();
            if (IdData != null)
            {
                if (!string.IsNullOrEmpty(IdData))
                {
                    Cancel.Visibility = Visibility.Collapsed;
                    Del.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Collapsed;
                    Edit.Visibility = Visibility.Visible;
                    string sql = "SELECT * FROM public.\"Marketing\" WHERE id = " + IdData + ";";
                    NpgsqlCommand command = new NpgsqlCommand(sql, connecting);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(1);
                            decimal budget = reader.GetDecimal(2);
                            string target = reader.GetString(3);
                            int contract_id = reader.GetInt32(7);
                            string[] actions = (string[])reader.GetValue(4); // Преобразуйте значение в массив строк
                            DateTime date_start = reader.GetDateTime(5); // Прочитайте значение даты из столбца
                            DateTime date_end = reader.GetDateTime(6); // Прочитайте значение даты из столбца, учитывая возможные значения NULL
                            for (int i = 0; i < actions.Length; i++)
                            {
                                actions[i] = actions[i].TrimEnd();
                            }
                            
                            int count = actions.Length;
                            while (planCount < count) {
                                PlanContainerAdd();
                            }
                            FillPlanContainer(actions);
                            Name.Text = name;
                            Budget.Text = budget.ToString();
                            Target.Text = target.TrimEnd();
                            Dates.Text = date_start.ToShortDateString() + "-" + date_end.ToShortDateString();
                            Contract.SelectedIndex = contract_id;
                        }
                    }
                }
            }
        }
        private void FillPlanContainer(string[] actions)
        {
            var actionTextBoxes = planContainer.Children.OfType<TextBox>().Where(tb => tb.Name.StartsWith("TextBox_")).ToList();

            for (int i = 0; i < actions.Length; i++)
            {
                if (i < actionTextBoxes.Count)
                {
                    actionTextBoxes[i].Text = actions[i];
                }
                else
                {
                    // Создать новый TextBox
                    TextBox newTextBox = new TextBox();
                    Button newButton = new Button();
                    newTextBox.Name = "TextBox_" + i;                    
                    newTextBox.TextAlignment = TextAlignment.Justify;
                    newTextBox.Margin = new Thickness(0, 0, 0, 10);
                    newTextBox.Style = Application.Current.Resources["TextBoxModuleSecond"] as Style;
                    newButton.Tag = planCount.ToString();
                    newButton.Name = "Button_" + planCount.ToString();
                    newButton.Content = "⁻";
                    newButton.Margin = new Thickness(0, 0, 0, 10);
                    newButton.Width = 40;
                    newButton.FontSize = 30;
                    newButton.FontWeight = FontWeights.Bold;
                    newButton.Click += PlanContainerRemove_Click;
                    // Добавить TextBox в контейнер
                    planContainer.Children.Add(newTextBox);
                    planContainerButton.Children.Add(newButton);
                    planCount++;
                }
            }
        }

    }
}
