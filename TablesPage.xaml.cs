using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using System.Xml;
using System.Xml.Linq;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    public partial class TablesPage : Page
    {
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
            DebugBlock.Text = string.Join(", ", rentedCarsId);
        }
        
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            XDocument rdoc = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
            XDocument cdoc = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            List<RentedCarsTable> result = new List<RentedCarsTable>(3);


            foreach (string Id in rentedCarsId){
                
                string sdate;
                string edate;
                string model;

                var dates = rdoc.Descendants("car")
                    .Where(x => (string)x.Element("Id") == Id)
                    .FirstOrDefault();
                sdate = (string)dates.Element("RentStart");
                edate = (string)dates.Element("RentEnd");
                var models = cdoc.Descendants("car")
                    .Where(x => (string)x.Element("Id") == Id)
                    .FirstOrDefault();
                model = (string)models.Element("Model");

                result.Add(new RentedCarsTable(Id, model, sdate, edate));
            }
            grid.ItemsSource = result;
        }
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /* MyTable path = grid.SelectedItem as MyTable;
             MessageBox.Show("Ответ", " ID: " + path.Id + "\n Исполнитель: " + path.Vocalist + "\n Альбом: " + path.Album
                 + "\n Год: " + path.Year, MessageBoxButton.OK);*/
            ;
        }

    }

    class RentedCarsTable
    {
        public RentedCarsTable(string Id, string Model, string RentStart, string RentEnd)
        {
            this.Id = Id;
            this.Модель = Model;
            this.НачалоПроката = RentStart;
            this.КонецПроката = RentEnd;
        }
        public string Id { get; set; }
        public string Модель { get; set; }
        public string НачалоПроката { get; set; }
        public string КонецПроката { get; set; }
    }

}
