using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    //
    public partial class TablesPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        string currentLogin;
        string Name;
        string Patronym;
        List<string> rentedCarsId = new List<string>();
        public TablesPage()
        {
            InitializeComponent();
            currentLogin = (string)Application.Current.Properties["currentLogin"];
            XDocument doc = XDocument.Load("..\\..\\..\\xml\\clients.xml");

            var client = doc.Descendants("client")
                            .Where(x => (string)x.Element("login") == currentLogin)
                            .FirstOrDefault();

            if (client != null)
            {
                Name = (string)client.Element("Name");
                Patronym = (string)client.Element("Patronym");
            }
            else 
            {                 
                MessageBox.Show("Ошибка", "Не удалось найти пользователя.", MessageBoxButton.OK);
            }

            WelcomeBlock.Text = "Здравствуйте, " + Name + " " + Patronym + "!";
            WelcomeBlock.FontSize = 20;

            XmlDocument rdoc = new XmlDocument();
           
            rdoc.Load("..\\..\\..\\xml\\rented_cars.xml");
       
            XmlElement? root = rdoc.DocumentElement;
            
            XmlNodeList? nodes = root.SelectNodes("car");
            foreach (XmlNode node in nodes)
            {
                if (node["Login"].InnerText == currentLogin)
                {

                    rentedCarsId.Add(node["Id"].InnerText);
                }
            }
            if (rentedCarsId.Count == 0)
            {
                grid.Visibility = Visibility.Collapsed;
                CarsBlock.Text = "У вас нет арендованных автомобилей.";
            }
            
        }
        
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            XDocument rdoc = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
            XDocument cdoc = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            List<RentedCarsTable> result = new List<RentedCarsTable>(3);
            grid.Height = 200;
            grid.Width = 550;

            foreach (string Id in rentedCarsId){
                
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

                result.Add(new RentedCarsTable(int.Parse(Id), model, sdate, edate));
            }
            grid.ItemsSource = result;
        }
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RentedCarsTable path = grid.SelectedItem as RentedCarsTable;
            MessageBox.Show("Автомобиль", " ID: " + path.Id + "\n Модель: " + path.Модель + "\n Начало проката: " + path.НачалоПроката
                + "\n Конец Проката: " + path.КонецПроката, MessageBoxButton.OK);
            ;
        }
        private void Return_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }

        private void Rent_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenRentPage();
        }
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            if (rentedCarsId.Count != 0)
            {
                MessageBox.Show("Ошибка", "У вас есть арендованные автомобили.", MessageBoxButton.OK);
                return;
            }
            if (MessageBoxResult.OK == MessageBox.Show("Удаление аккаунта", "Вы уверены, что хотите удалить аккаунт?", MessageBoxButton.OKCancel))
            {
                MessageBox.Show("Успех", "Вы удалили аккаунт.", MessageBoxButton.OK);
/*                XDocument doc = XDocument.Load("..\\..\\..\\xml\\clients.xml");
                var client = doc.Descendants("client")
                            .Where(x => (string)x.Element("login") == currentLogin)
                            .FirstOrDefault();
                client.Remove();
                doc.Save("..\\..\\..\\xml\\clients.xml");
                mainWindow.OpenLoginPage();*/
            }   
        }

    }

    class RentedCarsTable
    {
        public RentedCarsTable(int Id, string Model, DateOnly RentStart, DateOnly RentEnd)
        {
            this.Id = Id;
            this.Модель = Model;
            this.НачалоПроката = RentStart;
            this.КонецПроката = RentEnd;
        }
        public int Id { get; set; }
        public string Модель { get; set; }
        public DateOnly НачалоПроката { get; set; }
        public DateOnly КонецПроката { get; set; }



    }
}   
