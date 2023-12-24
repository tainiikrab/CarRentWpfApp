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
            doc.Load("..\\..\\..\\xml\\clients.xml");
            var items = doc;
            string login = LoginBox.Text;
            string passwordHash = PasswordHasher.GetHash(PasswordBox.Password);
            XmlElement? root = doc.DocumentElement;

            XmlNodeList? nodes = root.SelectNodes("client");
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
        }


        private void Register_click(object sender, RoutedEventArgs e)
        {
            RegisterPage rp = new();
            this.Content = rp;
        }
    }
}
