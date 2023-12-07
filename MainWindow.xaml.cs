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
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class PasswordHasher
    {
        public static string GetHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
    public partial class MainWindow : Window
    {
        XmlDocument doc = new XmlDocument();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_click(object sender, RoutedEventArgs e)
        {
            doc.Load("..\\..\\..\\xml\\login_data.xml");
            var items = doc;
            string login = LoginBox.Text;
            string passwordHash = PasswordHasher.GetHash(PasswordBox.Password);
            XmlElement? root = doc.DocumentElement;

            XmlNodeList? nodes = root.SelectNodes("user");
            bool wrong_data = true;
            foreach (XmlNode node in nodes)
            {
                if (node["login"].InnerText == login && node["PasswordHash"].InnerText == passwordHash)
                {
                    TablesPage tp = new();
                    this.Content = tp;
                    wrong_data = false;
                    break;
                }
            }
            if (wrong_data == true)
                MessageBox.Show("Неправильнй логин или пароль!");
                wrong_data = true;
            //if (login == loginFromXML && passwordHash == passwordHashFromXML)
            //{
            //    TablesPage tp = new();
            //    this.Content = tp;
            //}
            //else
            //{
            //    MessageBox.Show("Wrong login or password");
            //TablesPage tp = new();
            //this.Content = tp;
        }


        private void Register_click(object sender, RoutedEventArgs e)
        {
            XDocument doc_write = XDocument.Load("..\\..\\..\\xml\\login_data.xml");
            string login = LoginBox.Text;
            doc.Load("..\\..\\..\\xml\\login_data.xml");


            XmlElement? root = doc.DocumentElement;

            XmlNodeList? nodes = root.SelectNodes("user");
            bool existing_data = false;
            foreach (XmlNode node in nodes)
            {
                if (node["login"].InnerText == login)
                {
                    MessageBox.Show("Этот логин уже существует!");
                    return;
                }
            }

            string PasswordHash = PasswordHasher.GetHash(PasswordBox.Password);
            XElement root_new = new XElement("user");
            root_new.Add(new XElement("login", login));
            root_new.Add(new XElement("PasswordHash", PasswordHash));
            doc_write.Element("users").Add(root_new);
            doc_write.Save("..\\..\\..\\xml\\login_data.xml");

        }
    }
}
