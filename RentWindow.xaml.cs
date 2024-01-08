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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for RentWindow.xaml
    /// </summary>
    public partial class RentWindow : Window
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        string currentLogin = (string)Application.Current.Properties["currentLogin"];
        string currentCarId = (string)Application.Current.Properties["currentCarId"];
        string currentRentalPrice = (string)Application.Current.Properties["currentRentalPrice"];

        /*        string currentLogin = "petrick";
                string currentCarId = "2";
                string currentRentalPrice = "5555";*/

        public RentWindow()
        {
            InitializeComponent();
/*            this.Loaded += RentWindow_Loaded;
            this.Closed += RentWindow_Closed;*/
            StartDatePicker.DisplayDateStart = DateTime.Now;
            StartDatePicker.DisplayDateEnd = DateTime.Now.Add(TimeSpan.FromDays(365));
            EndDatePicker.DisplayDateStart = DateTime.Now.Add(TimeSpan.FromDays(1));
            EndDatePicker.DisplayDateEnd = DateTime.Now.Add(TimeSpan.FromDays(365 * 2));


            XDocument all_cars = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
            var currentCar = all_cars.Descendants("car")
                                     .FirstOrDefault(car => car.Element("Id").Value == currentCarId);
            CarBlock.Text = currentCar?.Element("Model")?.Value;
            PriceBlock.Text = "Стоимость в день: " + (string)currentRentalPrice + "р. Стоимость за весь срок: " + (string)currentRentalPrice + "р.";

            DateTime? selectedDate = StartDatePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateOnly date = DateOnly.FromDateTime(selectedDate.Value);
            }
        }
        




        private void Return_click(object sender, RoutedEventArgs e)
        {
            mainWindow.RentRemoveRectangle();
            this.Close();
        }
        private void Accept_click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Ошибка", "Пожалуйста, выберите дату.", MessageBoxButton.OK);
            }
            else
            {
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;
                if (startDate > endDate)
                {
                    MessageBox.Show("Ошибка", "Пожалуйста, выберите корректную дату.", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Успех", "Вы успешно зарезервировали автомобиль.", MessageBoxButton.OK);
                    TimeSpan span = (TimeSpan)(EndDatePicker.SelectedDate - StartDatePicker.SelectedDate);

                    XDocument rented_cars = XDocument.Load("..\\..\\..\\xml\\rented_cars.xml");
                    rented_cars.Root.Add(new XElement("car",
                        new XElement("Id", currentCarId),
                        new XElement("Login", currentLogin),
                        new XElement("RentStart", StartDatePicker.SelectedDate),
                        new XElement("RentEnd", EndDatePicker.SelectedDate)
                    ));
                    rented_cars.Save("..\\..\\..\\xml\\rented_cars.xml");


                    XDocument clients = XDocument.Load("..\\..\\..\\xml\\clients.xml");
                    var client = clients.Descendants("client")
                                              .FirstOrDefault(login => login.Element("login").Value == currentLogin);
                    if (client != null)
                    {
                        XElement rentalRecord = client.Element("RentalRecord");
                        if (rentalRecord != null)
                        {
                            rentalRecord.Value = (int.Parse(rentalRecord.Value) + span.Days).ToString();
                            clients.Save("..\\..\\..\\xml\\clients.xml");
                        }
                    }

                    XDocument all_cars = XDocument.Load("..\\..\\..\\xml\\all_cars.xml");
                    var currentCar = all_cars.Descendants("car")
                                             .FirstOrDefault(car => car.Element("Id").Value == currentCarId);
                    if (currentCar != null)
                    {
                        XElement rentalPeriod = currentCar.Element("RentalPeriod");
                        if (rentalPeriod != null)
                        {
                            rentalPeriod.Value = (int.Parse(rentalPeriod.Value) + span.Days).ToString();
                            all_cars.Save("..\\..\\..\\xml\\all_cars.xml");
                        }
                    }


                    mainWindow.RentRemoveRectangle();
                    this.Close();
                }
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.RentRemoveRectangle();
        }
        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = (DateTime)StartDatePicker.SelectedDate;
            EndDatePicker.DisplayDateStart = selectedDate.Add(TimeSpan.FromDays(1));
            if (EndDatePicker.SelectedDate != null)
            {
                TimeSpan span = (TimeSpan)(EndDatePicker.SelectedDate - StartDatePicker.SelectedDate);
                PriceBlock.Text = "Стоимость в день: " + currentRentalPrice + "р. Стоимость за весь срок: " + (int.Parse(currentRentalPrice) * span.TotalDays).ToString() + "р.";
            }
        }
        private void EndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = (DateTime)EndDatePicker.SelectedDate;
            StartDatePicker.DisplayDateEnd = selectedDate.Add(TimeSpan.FromDays(-1));
            if (StartDatePicker.SelectedDate != null)
            {
                TimeSpan span = (TimeSpan)(EndDatePicker.SelectedDate - StartDatePicker.SelectedDate);
                PriceBlock.Text = "Стоимость в день: " + currentRentalPrice + "р. Стоимость за весь срок: " + (int.Parse(currentRentalPrice) * span.TotalDays).ToString() + "р.";

            }
        }   
    }
}
