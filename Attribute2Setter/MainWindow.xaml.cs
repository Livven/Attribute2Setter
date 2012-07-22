using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Attribute2Setter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToolTip toolTip = new ToolTip();
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        //void attributeToSetterButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string[] attributes = attributeText.Text.Split('"');
        //    setterText.Text = "";
        //    for (int i = 0; i < attributes.Length - 1; i += 2)
        //    {
        //        string property = attributes[i].Trim(' ', '=');
        //        setterText.Text += "<Setter Property=\"" + property + "\" Value=\"" + attributes[i + 1] + "\"/>\n";
        //    }
        //    setterText.Text = setterText.Text.Trim();
        //    Clipboard.SetText(setterText.Text);
        //    ShowToolTip("Style setters");
        //}

        //void setterToAttributeText_Click(object sender, RoutedEventArgs e)
        //{
        //    string[] setters = setterText.Text.Split('"');
        //    attributeText.Text = "";
        //    for (int i = 1; i < setters.Length; i += 4)
        //    {
        //        attributeText.Text += setters[i] + "=\"" + setters[i + 2] + "\" ";
        //    }
        //    attributeText.Text = attributeText.Text.Trim();
        //    Clipboard.SetText(attributeText.Text);
        //    ShowToolTip("Attributes");
        //}

        void button_Click(object sender, RoutedEventArgs e)
        {
            string result = Converter.Convert(Clipboard.GetText());
            textBox.Text = result;
            //Clipboard.SetText(result);
        }

        void ShowToolTip(string type)
        {
            toolTip.IsOpen = false;
            toolTip = new ToolTip()
            {
                Content = type + " successfully copied to clipboard",
                //PlacementTarget = setterText,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                VerticalOffset = 6,
                IsOpen = true
            };
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            toolTip.IsOpen = false;
            ((DispatcherTimer)sender).Stop();
        }

        void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            label.Target.Focus();
        }

        void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TrimEachLine((TextBox)sender);
            ToggleLabel(sender, label);
        }
        void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToggleLabel(sender, label);
        }

        void TrimEachLine(TextBox textBox)
        {
            string s = "";
            for (int i = 0; i < textBox.LineCount; i++)
            {
                string line = textBox.GetLineText(i).Trim();
                if (line != "")
                    s += (i == textBox.LineCount - 1 ? line : line + "\n");
            }
            textBox.Text = s;
        }

        void ToggleLabel(object sender, Label label)
        {
            if (((TextBox)sender).Text == "")
                label.Visibility = Visibility.Visible;
            else
                label.Visibility = Visibility.Collapsed;
        }

        void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }
    }
}
