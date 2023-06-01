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

namespace Model_eTOM.Add
{
    /// <summary>
    /// Логика взаимодействия для Mark_add.xaml
    /// </summary>
    public partial class Mark_add : Window
    {
        private int planCount = 0;
        private int contractCount = 0;
        
        public Mark_add()
        {
            InitializeComponent();
        }

        private void PlanContainerAdd_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            if (planCount < 10)
            {
                // Создать новый TextBox
                TextBox newTextBox = new TextBox();
                Button newButton = new Button();
                // Присвоить уникальный идентификатор
                newTextBox.Name = "TextBox_" + planCount;
                newTextBox.Text += planCount + 1 +". ";
                newTextBox.TextAlignment = TextAlignment.Justify;
                newTextBox.Margin = new Thickness(0, 0, 0, 10);
                newTextBox.Style = app.Resources["TextBoxModuleSecond"] as Style;
                newButton.Tag = planCount.ToString();
                newButton.Name = "Button_"+planCount.ToString();
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
        private void ContractContainerAdd_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            if (contractCount < 10)
            {
                // Создать новый TextBox
                TextBox newTextBox = new TextBox();
                Button newButton = new Button();
                // Присвоить уникальный идентификатор
                newTextBox.Name = "TextBox_" + contractCount;
                newTextBox.Text += contractCount + 1 + ". ";
                newTextBox.TextAlignment = TextAlignment.Justify;
                newTextBox.Margin = new Thickness(0, 0, 0, 10);
                newTextBox.Style = app.Resources["TextBoxModuleSecond"] as Style;
                newButton.Tag = contractCount.ToString();
                newButton.Name = "Button_" + contractCount.ToString();
                newButton.Content = "⁻";
                newButton.Width = 40;
                newButton.FontSize = 30;
                newButton.FontWeight = FontWeights.Bold;
                newButton.Margin = new Thickness(0, 0, 0, 10);
                newButton.Click += ContractContainerRemove_Click;
                // Добавить TextBox в контейнер
                contractContainer.Children.Add(newTextBox);
                contractContainerButton.Children.Add(newButton);
                contractCount++;
            }
            else MessageBox.Show("Максимум 10 полей");
        }  
        private void ContractContainerRemove_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string data = button.Tag as string;
            TextBox textBoxToRemove = contractContainer.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "TextBox_" + data);
            Button buttonToRemove = contractContainerButton.Children.OfType<Button>().FirstOrDefault(btn => btn.Tag as string == data);
            contractContainer.Children.Remove(textBoxToRemove);
            contractContainerButton.Children.Remove(buttonToRemove);
            contractCount--;
        }
        
    }
}
