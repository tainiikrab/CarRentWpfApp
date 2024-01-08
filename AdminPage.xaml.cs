using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Numerics;
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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public AdminPage()
        {
            InitializeComponent();
        }



        XmlDocument doc = new XmlDocument();

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {

            XDocument rdoc = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
            XDocument cdoc = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            List<ClientsTable> result = new List<ClientsTable>(3);
            grid.Height = 300;
            grid.Width = 1150;

            doc.Load("..\\..\\..\\xml\\clients.xml");
            XmlElement? root = doc.DocumentElement;
            XmlNodeList? nodes = root.SelectNodes("client");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine(node["login"].InnerText);
                if (node["login"] != null && node["Name"] != null && node["Surname"] != null && node["Patronym"] != null && node["Telephone"] != null && node["Address"] != null && node["RentalRecord"] != null && node["FineRecord"] != null)
                {
                    result.Add(new ClientsTable(node["login"].InnerText, node["Name"].InnerText, node["Surname"].InnerText, node["Patronym"].InnerText, node["Telephone"].InnerText, node["Address"].InnerText, int.Parse(node["RentalRecord"].InnerText), int.Parse(node["FineRecord"].InnerText)));
                }

            }
            grid.ItemsSource = result;
            grid.Columns[0].Header = "Логин";
            grid.Columns[1].Header = "Имя";
            grid.Columns[2].Header = "Фамилия";
            grid.Columns[3].Header = "Отчество";
            grid.Columns[4].Header = "Телефон";
            grid.Columns[5].Header = "Адрес";
            grid.Columns[6].Header = "Дней аренды";
            grid.Columns[7].Header = "Штрафы";
            grid.ItemsSource = result;
        }
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClientsTable path = grid.SelectedItem as ClientsTable;
            if (path == null)
            {
                return;
            }
            XDocument clients = XDocument.Load("..\\..\\..\\xml\\clients.xml");
            var client = clients.Descendants("client")
                                      .FirstOrDefault(login => login.Element("login").Value == path.login);
            if (client == null)
            {
                return;
            }
            XElement fineRecord = client.Element("FineRecord");
            if (fineRecord == null)
            {
                return;

            }
            MessageBoxResult result = MessageBox.Show("Штрафы: " + fineRecord.Value, "Добавить или убавить штраф пользователю " + path.login + ".", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                fineRecord.Value = (int.Parse(fineRecord.Value) + 1).ToString();
                clients.Save("..\\..\\..\\xml\\clients.xml");
                grid_Loaded(sender, e);
            }
            else if (result == MessageBoxResult.No)
            {
                if (int.Parse(fineRecord.Value) < 1)
                {
                    MessageBox.Show("Ошибка", "Штраф не может быть меньше нуля.", MessageBoxButton.OK);
                    return;
                }
                fineRecord.Value = (int.Parse(fineRecord.Value) - 1).ToString();
                clients.Save("..\\..\\..\\xml\\clients.xml");
                grid_Loaded(sender, e);
            }
        }
    

        private void Return_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }
        private void Abort_click(object sender, RoutedEventArgs e)
        {
            grid_Loaded(sender, e);
        }


        private void GroupCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            XDocument doc = XDocument.Load("..\\..\\..\\xml\\clients.xml");
            var type = (from x in doc.Element("clients").Elements("client")
                        let rentalRecord = x.Element("RentalRecord")
                        where rentalRecord != null
                        group x by rentalRecord.Value into g
                        select new
                        {
                            ДнейАренды = g.Key,
                            КоличествоПользователей = g.Count()
                        }).ToList();
            grid.ItemsSource = type;
        }

        private void GroupCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            grid_Loaded(sender, e);

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            XDocument doc = XDocument.Load("..\\..\\..\\xml\\clients.xml");
            var kod = (from x in doc.Element("clients").Elements("client")
                           where (string)x.Element("login") == SearchBox.Text
                           select new{
                            Код = x.Element("login").Value,
                            Название = x.Element("Name").Value,
                            Тип = x.Element("Surname").Value,
                            Материал = x.Element("Patronym").Value,
                            Bec = x.Element("Telephone").Value, 
                            Цена = x.Element("Address").Value,
                            ДнейАренды = x.Element("RentalRecord").Value,
                            Штрафы = x.Element("FineRecord").Value,
                           }).ToList();
                            grid.ItemsSource = kod;
        }

        private void AmountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            XDocument doc = XDocument.Load("..\\..\\..\\xml\\clients.xml");
            var type = (from x in doc.Element("clients").Elements("client")
                        where (string)x.Element("FineRecord") == AmountBox.Text
                        group x by x.Element("FineRecord").Value into g
                        select new
                        {
                            КоличествоШтрафов = g.Key,
                            ПользователейСТакимКоличеством = g.Count()
                        }).ToList();


            grid.ItemsSource = type;
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = "";
        }
    }
    class ClientsTable
    {
        public ClientsTable(string login, string name, string sname, string pname, string phone, string address, int rrecord, int fines)
        {
            this.login = login;
            this.name = name;
            this.sname = sname;
            this.pname = pname;
            this.phone = phone;
            this.address = address;
            this.rrecord = rrecord;
            this.fines = fines;
        }
        public string login { get; set; }
        public string name { get; set; }
        public string sname { get; set; }
        public string pname { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int rrecord { get; set; }
        public int fines { get; set; }



    }

}

