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
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

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
            LoginPage lp = new LoginPage();
            this.Content = lp;

        }

        public void OpenTablesPage()
        {
            TablesPage tp = new TablesPage();
            this.Content = tp;
        }
        public void OpenRegisterPage()
        {
            RegisterPage rp = new RegisterPage();
            this.Content = rp;
        }
        public void OpenLoginPage()
        {
            LoginPage lp = new LoginPage();
            this.Content = lp;
        }
    }
}
