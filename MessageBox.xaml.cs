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
                    CreateButton("Отмена", MessageBoxResult.Cancel, true);
                    break;
                case MessageBoxButton.YesNo:
                    CreateButton("Да", MessageBoxResult.Yes);
                    CreateButton("Нет", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    CreateButton("Добавить", MessageBoxResult.Yes);
                    CreateButton("Убавить", MessageBoxResult.No);
                    CreateButton("Отмена", MessageBoxResult.Cancel, true);
                    break;
                default:
                    throw new ArgumentException("Неправильный тип кнопки", nameof(buttonType));
            }
        }

        private void CreateButton(string buttonText, MessageBoxResult buttonResult, bool isCancel = false)
        {
            var newButton = new Button() { Content = buttonText, IsCancel = isCancel, Width = 75, Height = 30 };
            newButton.BorderThickness = new Thickness(0);

            Style buttonStyle = new Style(typeof(Button));

            Setter backgroundSetter = new Setter(Button.BackgroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom("#b3e099")));

            Setter minWidthSetter = new Setter(Button.MinWidthProperty, newButton.Width);

            Setter minHeightSetter = new Setter(Button.MinHeightProperty, newButton.Height);

            buttonStyle.Setters.Add(backgroundSetter);
            buttonStyle.Setters.Add(minWidthSetter);
            buttonStyle.Setters.Add(minHeightSetter);

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