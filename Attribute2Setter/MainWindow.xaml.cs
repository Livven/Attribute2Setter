using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace Attribute2Setter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ToolTip toolTip = new ToolTip();
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        void attributeToSetterButton_Click(object sender, RoutedEventArgs e)
        {
            string[] attributes = attributeText.Text.Split('"');
            setterText.Text = "";
            for (int i = 0; i < attributes.Length - 1; i += 2)
            {
                string property = attributes[i].Trim(' ', '=');
                setterText.Text += "<Setter Property=\"" + property + "\" Value=\"" + attributes[i + 1] + "\"/>\n";
            }
            setterText.Text = setterText.Text.Trim();
            Clipboard.SetText(setterText.Text);
            ShowToolTip("Style setters");
        }

        void setterToAttributeText_Click(object sender, RoutedEventArgs e)
        {
            string[] setters = setterText.Text.Split('"');
            attributeText.Text = "";
            for (int i = 1; i < setters.Length; i += 4)
            {
                attributeText.Text += setters[i] + "=\"" + setters[i + 2] + "\" ";
            }
            attributeText.Text = attributeText.Text.Trim();
            Clipboard.SetText(attributeText.Text);
            ShowToolTip("Attributes");
        }

        void ShowToolTip(string type)
        {
            toolTip.IsOpen = false;
            toolTip = new ToolTip()
            {
                Content = type + " successfully copied to clipboard",
                PlacementTarget = setterText,
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

        void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            label.Visibility = Visibility.Collapsed;
            label.Target.Focus();
        }

        void attributeText_LostFocus(object sender, RoutedEventArgs e)
        {
            TrimEachLine((TextBox)sender);
            ToggleLabel(sender, attributeLabel);
        }
        void attributeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToggleLabel(sender, attributeLabel);
        }

        void setterText_LostFocus(object sender, RoutedEventArgs e)
        {
            TrimEachLine((TextBox)sender);
            ToggleLabel(sender, setterLabel);
        }
        void setterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToggleLabel(sender, setterLabel);
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
    }
}
