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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public AdminPage()
        {
            InitializeComponent();
        }





        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            ;
/*            XDocument rdoc = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
            XDocument cdoc = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            List<RentedCarsTable> result = new List<RentedCarsTable>(3);
            grid.Height = 200;
            grid.Width = 550;

            foreach (string Id in rentedCarsId)
            {

                string model;

                var dates = rdoc.Descendants("car")
                    .Where(x => (string)x.Element("Id") == Id)
                    .FirstOrDefault();
                DateOnly.TryParse((string)dates.Element("RentStart"), out DateOnly sdate);
                DateOnly.TryParse((string)dates.Element("RentEnd"), out DateOnly edate);
                var models = cdoc.Descendants("car")
                    .Where(x => (string)x.Element("Id") == Id)
                    .FirstOrDefault();
                model = (string)models.Element("Model");

                result.Add(new RentedCarsTable(Id, model, sdate, edate));
            }
            grid.ItemsSource = result;*/
        }
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ;
/*            RentedCarsTable path = grid.SelectedItem as RentedCarsTable;
            MessageBox.Show("Автомобиль", " ID: " + path.Id + "\n Модель: " + path.Модель + "\n Начало проката: " + path.НачалоПроката
                + "\n Конец Проката: " + path.КонецПроката, MessageBoxButton.OK);
            ;*/
        }

        private void Clients_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }
        private void Cars_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }
        private void Return_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }
    }
}
