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
using System.Xml;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

  
        XmlDocument doc = new XmlDocument();
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public string currentLogin = "DEFAULT_LOGIN";
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
 
                    currentLogin = login;
                    Application.Current.Properties["currentLogin"] = login;
                    wrong_data = false;
                    if (login == "admin")
                    {
                        mainWindow.OpenAdminPage();
                        break;
                    }
                    mainWindow.OpenTablesPage();
                    break;
                }
            }
            if (wrong_data == true)
                MessageBox.Show("Ошибка", "Неправильный логин или пароль.", MessageBoxButton.OK);

            wrong_data = true;
            wrong_data = true;
        }


        private void Register_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenRegisterPage();

        }
        public string GetCurrentLogin()
        {
            return currentLogin;

        }
    }
}


