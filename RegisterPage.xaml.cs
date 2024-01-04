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
using System.Xml;
using System.Diagnostics;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        XmlDocument doc = new XmlDocument();
        public RegisterPage()
        {
            InitializeComponent();
        }
        private void Register_click(object sender, RoutedEventArgs e)
        {
            XDocument doc_write = XDocument.Load("..\\..\\..\\xml\\clients.xml");
            string login = LoginBox.Text;
            doc.Load("..\\..\\..\\xml\\clients.xml");


            XmlElement? root = doc.DocumentElement;

            XmlNodeList? nodes = root.SelectNodes("client");
            bool existing_data = false;
            foreach (XmlNode node in nodes)
            {
                if (node["login"]?.InnerText == login)
                {
                    MessageBox.Show("Ошибка", "Этот логин уже существует.", MessageBoxButton.OK);
                    return;
                }
                if (NameBox.Text == string.Empty || SurnameBox.Text == string.Empty || AddressBox.Text == string.Empty || PatronymBox.Text == string.Empty || TelephoneBox.Text == string.Empty || LoginBox.Text == string.Empty || PasswordBox.Text == string.Empty)
                {
                    MessageBox.Show("Ошибка", "Пропущено одно или несколько полей.", MessageBoxButton.OK);
                    return;
                }
            }

            string PasswordHash = PasswordHasher.GetHash(PasswordBox.Text);
            XElement root_new = new XElement("client");
            root_new.Add(new XElement("login", login));
            root_new.Add(new XElement("PasswordHash", PasswordHash));
            root_new.Add(new XElement("Name", NameBox.Text));
            root_new.Add(new XElement("Surname", SurnameBox.Text));
            root_new.Add(new XElement("Patronym", PatronymBox.Text));
            root_new.Add(new XElement("Telephone", TelephoneBox.Text));
            root_new.Add(new XElement("Address", AddressBox.Text));
            root_new.Add(new XElement("RentalRecord", 0));
            doc_write.Element("clients").Add(root_new);
            doc_write.Save("..\\..\\..\\xml\\clients.xml");

            mainWindow.OpenLoginPage();


        }
        private void Return_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenLoginPage();
        }
            private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ;
        }

        public void ClearText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
