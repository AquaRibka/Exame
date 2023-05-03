using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
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

namespace eTOM
{
    /// <summary>
    /// Логика взаимодействия для Mark_forecast.xaml
    /// </summary>
    public partial class Mark_forecast : Window
    {
        private string connectPostgre = String.Format("Server=Localhost;Port=5432;User Id=postgres;password=1111;Database=eTOM");
        private NpgsqlConnection connect;
        public string idData { get; set; }
        public Mark_forecast()
        {
            InitializeComponent();

        
        }
        // Создаем список точек для графика
        string GetForecastString(List<DataPoint> dataPoints)
        {
            string dataString = "";
            foreach (var point in dataPoints)
            {
              //  MessageBox.Show(point.Forecast.ToString());
                dataString += point.Forecast + ",";
            }
            return dataString.TrimEnd(',');
        }
       
        // Класс для хранения точек данных графика
        class DataPoint
        {
          //  public string Year { get; set; }
            public int Forecast { get; set; }
            

            public DataPoint(int forecast)
            {
              //  Year = year;
                Forecast = forecast;
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
                connect = new NpgsqlConnection(connectPostgre);
                Data_Upload();
        }

        private void Data_Upload()
        {
            int dateEnd;
            List<DataPoint> dataPoints = new List<DataPoint>();
            Random random = new Random();
            try
            {

                connect.Open();
                string sql = @"SELECT *
	FROM public." + '\u0022' + "Marketing" + '\u0022' + "WHERE id = " + idData + ";";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(cmd);
                DataTable iDataSet = new DataTable();
                iAdapter.Fill(iDataSet);
                DataRow[] data_row = iDataSet.Select();
                string budget = data_row[0]["budget"].ToString();
                // string dateStart = data_row[0]["date_start"].ToString();
                budget = budget.Remove(budget.LastIndexOf(@","));
                int budgetValue = int.Parse(budget);
                int yearStart = int.Parse(data_row[0]["date_start"].ToString().Substring(6, 4));
                int yearEnd = int.Parse(data_row[0]["date_end"].ToString().Substring(6, 4));

             //   MessageBox.Show(data_row[0]["date_start"].ToString().Substring(0, 10));
                if (yearEnd-yearStart <= 1) {
                    DateTime date1 = DateTime.ParseExact(data_row[0]["date_start"].ToString().Substring(0, 10), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    DateTime date2 = DateTime.ParseExact(data_row[0]["date_end"].ToString().Substring(0, 10), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    TimeSpan span = date2.Subtract(date1); // вычисляем разницу между датами
                    double months = span.TotalDays / 30.436875; // переводим дни в месяцы
                    months = Math.Round(months, MidpointRounding.AwayFromZero); // округляем до целого числа
                    
                    dateEnd = (int)months+3;
                    double[] dataValue = new double[dateEnd];
                    for (int i = 0; i < dataValue.Length; i++)
                    {
                        double randomNumber = random.NextDouble();
                        // MessageBox.Show(randomNumber.ToString());
                        if (i == 0)
                        {
                            dataValue[i] = budgetValue / dateEnd * (randomNumber * randomNumber) * 0.1;
                          //  MessageBox.Show(dataValue[i].ToString());
                        }
                        else
                        {
                            dataValue[i] = dataValue[i -1]+(budgetValue / dateEnd * (randomNumber * randomNumber) * 0.1);
                            
                        }
                      
                        dataPoints.Add(new DataPoint((int)Math.Round(dataValue[i]))); 
                    }
                } else
                {
                    
                    dateEnd = yearEnd - yearStart;
                    double[] dataValue = new double[dateEnd];
                    for (int i = 0; i < dataValue.Length; i++)
                    {
                        double randomNumber = random.NextDouble();
                        // MessageBox.Show(randomNumber.ToString());
                        if (i == 0)
                        {
                            dataValue[i] = budgetValue / dateEnd * (randomNumber * randomNumber) * 0.1;
                            //  MessageBox.Show(dataValue[i].ToString());
                        }
                        else
                        {
                            dataValue[i] = dataValue[i - 1] + (budgetValue / dateEnd * (randomNumber * randomNumber) * 0.1);

                        }

                        dataPoints.Add(new DataPoint((int)Math.Round(dataValue[i])));
                    }
                } 
                //  MessageBox.Show(yearStart.ToString());




                // Формируем URL для запроса к API
                string url = "https://chart.googleapis.com/chart" +
                    "?cht=lc" + // Тип графика - линейный
                    "&chs=500x300" + // Размер графика
                    "&chxt=x,y" + // Оси X и Y
                    "&chxr=0,0," + dateEnd + ",1|1,0," + (int)Math.Round((budgetValue / dateEnd) / 1.2) + // Диапазоны значений осей
                    "&chds=0," + (budgetValue / dateEnd) / 1.2 + // Минимальное и максимальное значение данных
                    "&chco=0077CC,FF9900" + // Цвета линий
                    "&chd=t:0," + GetForecastString(dataPoints) +// Данные графика
                    "&chdl=Прирост клиентов" + // Легенда графика
                    "&chtt=Прогноз компании " + data_row[0]["name"].ToString();
                  //   "&chxl=x:|Номер месяца|y:|Клиенты|"; // Заголовок графика

                // Отправляем запрос к API и получаем ответ в формате изображения
                WebClient client = new WebClient();
                byte[] imageBytes = client.DownloadData(url);


                // Создаем BitmapImage из полученных байтов и устанавливаем его в качестве источника изображения для элемента Image
                BitmapImage chartBitmap = new BitmapImage();
                chartBitmap.BeginInit();
                chartBitmap.StreamSource = new System.IO.MemoryStream(imageBytes);
                chartBitmap.EndInit();
                chartImage.Source = chartBitmap;

            }
            catch (Exception ex)
            {
                connect.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
            

           
        }
    }

}

