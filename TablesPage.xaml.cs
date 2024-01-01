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
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    public partial class TablesPage : Page
    {
        string currentLogin;
        string Name;
        string Patronym;

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
        }
    }
}
