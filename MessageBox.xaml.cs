/*using System;
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

namespace WpfApp1_04._12
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        void AddButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("OK", MessageBoxResult.OK);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException("Unknown button value", "buttons");
            }
        }

        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button() { Content = text, IsCancel = isCancel };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }

        MessageBoxResult Result = MessageBoxResult.None;

        public static MessageBoxResult Show(string caption, string message,
                                            MessageBoxButton buttons)
        {
            var dialog = new MessageBox() { Title = caption };
            dialog.MessageContainer.Text = message;
            dialog.AddButtons(buttons);
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}*/


















using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1_04._12
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        private void ConfigureButtons(MessageBoxButton buttonType)
        {
            switch (buttonType)
            {
                case MessageBoxButton.OK:
                    CreateButton("OK", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    CreateButton("OK", MessageBoxResult.OK);
                    CreateButton("Cancel", MessageBoxResult.Cancel, true);
                    break;
                case MessageBoxButton.YesNo:
                    CreateButton("Yes", MessageBoxResult.Yes);
                    CreateButton("No", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    CreateButton("Yes", MessageBoxResult.Yes);
                    CreateButton("No", MessageBoxResult.No);
                    CreateButton("Cancel", MessageBoxResult.Cancel, true);
                    break;
                default:
                    throw new ArgumentException("Invalid button type", nameof(buttonType));
            }
        }

        private void CreateButton(string buttonText, MessageBoxResult buttonResult, bool isCancel = false)
        {
            var newButton = new Button() { Content = buttonText, IsCancel = isCancel, Width = 75, Height = 30 };
            newButton.BorderThickness = new Thickness(0);

            Style buttonStyle = new Style(typeof(Button));

            // Create a new setter for the Background property
            Setter backgroundSetter = new Setter(Button.BackgroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom("#b3e099")));

            // Create a new setter for the MinWidth property
            Setter minWidthSetter = new Setter(Button.MinWidthProperty, newButton.Width);

            // Create a new setter for the MinHeight property
            Setter minHeightSetter = new Setter(Button.MinHeightProperty, newButton.Height);

            // Add the setters to the style
            buttonStyle.Setters.Add(backgroundSetter);
            buttonStyle.Setters.Add(minWidthSetter);
            buttonStyle.Setters.Add(minHeightSetter);

            // Apply the style to the button
            newButton.Style = buttonStyle;
            newButton.Click += (sender, e) => { Result = buttonResult; DialogResult = true; };
            ButtonContainer.Children.Add(newButton);
        }

        private MessageBoxResult Result = MessageBoxResult.None;

        public static MessageBoxResult Show(string title, string text, MessageBoxButton buttonType)
        {
            var messageBox = new MessageBox() { Title = title };
            messageBox.MessageContainer.Text = text;
            messageBox.ConfigureButtons(buttonType);
            messageBox.ShowDialog();
            return messageBox.Result;
        }
    }
}