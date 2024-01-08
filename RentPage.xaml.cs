using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Interaction logic for RentPage.xaml
    /// </summary>

    public partial class RentPage : Page
    {
        XDocument rentedCarsDoc = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        List<AllCarsTable> result = new List<AllCarsTable>(3);
        public RentPage()
        {
            InitializeComponent();
            InactiveRectangle.Width = 5000;
            InactiveRectangle.Height = 5000;
        }



        private void grid_Loaded(object sender, RoutedEventArgs e)
        {


            string currentLogin = (string)Application.Current.Properties["currentLogin"];

            // Load the XML files
            XDocument allCarsDoc = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            XDocument clientsDoc = XDocument.Load("..\\..\\..\\xml\\clients.xml");

            // Get the IDs of the rented cars
            var rentedCarIds = rentedCarsDoc.Descendants("car")
                                            .Select(c => (string)c.Element("Id"))
                                            .ToList();

            // Get the cars that are not rented
            var carsToAdd = allCarsDoc.Descendants("car")
                                      .Where(c => !rentedCarIds.Contains((string)c.Element("Id")));

            // Assuming 'table' is a DataTable instance
            foreach (var car in carsToAdd)
            {
                int id = (int)car.Element("Id");
                string model = (string)car.Element("Model");
                int carPrice = (int)car.Element("CarPrice");
                int manufactureYear = (int)car.Element("ManufactureYear");
                int rentalPeriod = (int)car.Element("RentalPeriod");
                int baseRentalPrice = (int)car.Element("BaseRentalPrice");


                XElement client = clientsDoc.Descendants("client")
                     .FirstOrDefault(c => c.Element("login").Value == currentLogin);
                int userRentalPeriod = int.Parse(client.Element("RentalRecord").Value);
                int fines = int.Parse(client.Element("FineRecord").Value);

                double rentalPrice = CalculateRentalPrice(baseRentalPrice, manufactureYear, rentalPeriod, userRentalPeriod, fines);
                Application.Current.Properties["currentyRentalPrice"] = rentalPrice;
                result.Add(new AllCarsTable(id, model, carPrice, manufactureYear, rentalPeriod, (int)rentalPrice));
            }







            grid.ItemsSource = result;
            grid.Columns[1].Header = "Модель";
            grid.Columns[2].Header = "Цена автомобиля";
            grid.Columns[3].Header = "Год производства";
            grid.Columns[4].Header = "Дней в эксплуатации";
            grid.Columns[5].Header = "Стоимость проката за день";
            grid.MaxHeight = 300;

        }
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AllCarsTable path = grid.SelectedItem as AllCarsTable;
            if (path == null)
            {
                return;
            }
            if (rentedCarsDoc.Descendants("car")
                   .Any(car => (string)car.Element("Id") == path.Id.ToString()))
            {
                MessageBox.Show("Ошибка", "Этот автомобиль уже арендован.", MessageBoxButton.OK);
                return;
            }

            Application.Current.Properties["currentCarId"] = path.Id.ToString();
            Application.Current.Properties["currentRentalPrice"] = path.rentCostPerDay.ToString();
            RentWindow rentWindow = new RentWindow();
            AddRectangle();
            rentWindow.Show();
        }

        private void Return_click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.OpenTablesPage();
            }
        }

        double CalculateRentalPrice(int baseRentalPrice, int manufactureYear, int rentalPeriod, int userRentalDays, int fines)
        {
            const double FineFactor = 0.05;
            const double GoodRecordDiscount = 0.004;

            double yearFactor = Math.Clamp((1.0 - ((DateTime.Now.Year - manufactureYear) / 500.0)), 0.8, 1);
            double recordFactor = Math.Clamp(userRentalDays * GoodRecordDiscount, 0.8, 1);
            double fineFactor = Math.Clamp((1 + (fines * FineFactor)), 1, 2);
            double rentalFactor = Math.Clamp((1 - rentalPeriod / 500), 0.8, 1);
            double rentalPrice = baseRentalPrice * rentalFactor * yearFactor * recordFactor * fineFactor;


            return rentalPrice;
        }

        public void AddRectangle()
        {
         InactiveRectangle.Visibility = Visibility.Visible;
        }
        public void RemoveRectangle()
        {
            InactiveRectangle.Visibility = Visibility.Hidden;
            Console.WriteLine("Rectangle removed");
        }


    }



    class AllCarsTable
    {
        public AllCarsTable(int id, string model, int carPrice, int manufactureYear, int rentalPeriod, int rentPrice)
        {
            this.Id = id;
            this.model = model;
            this.rentCostPerDay = rentPrice;
            this.carPrice = carPrice;
            this.daysOfRent = rentalPeriod;
            this.manufactureYear = manufactureYear;

        }
        public int Id { get; set; }
        public string model { get; set; }
        public int carPrice { get; set; }
        public int manufactureYear { get; set; }
        public int daysOfRent { get; set; }
        public int rentCostPerDay { get; set; }



    }

}