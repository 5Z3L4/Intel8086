using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Intel8086
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isClickedLMB = false;
        bool isClickedRMB = false;
        string selected;
        string selected2;
        TextBox registerFirst = new TextBox();
        TextBox registerSecond = new TextBox();
        TextBox test2 = new TextBox();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MOV(object sender, RoutedEventArgs e)
        {
            registerFirst.Text = registerSecond.Text;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string item = (sender as TextBox).Text;
            int n = 0;
            if (!int.TryParse(item,System.Globalization.NumberStyles.HexNumber,System.Globalization.NumberFormatInfo.CurrentInfo, out n) && item !=String.Empty)
            {
                (sender as TextBox).Text = item.Remove(item.Length - 1, 1);
                (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            }
        }

        private void firstRegister_DropDownClosed(object sender, EventArgs e)
        {
            selected2 = (sender as ComboBox).Text;

            if (selected2 == "AH")
                registerFirst = ahTextBox;
            else if (selected2 == "AL")
                registerFirst = alTextBox;
            else if (selected2 == "BH")
                registerFirst = bhTextBox;
            else if (selected2 == "BL")
                registerFirst = blTextBox;
            else if (selected2 == "CH")
                registerFirst = chTextBox;
            else if (selected2 == "CL")
                registerFirst = clTextBox;
            else if (selected2 == "DH")
                registerFirst = dhTextBox;
            else if (selected2 == "DL")
                registerFirst = dlTextBox;

        }

        private void secondRegister_DropDownClosed(object sender, EventArgs e)
        {
            selected = (sender as ComboBox).Text;
            if (selected == "AH")
                registerSecond = ahTextBox;
            else if (selected == "AL")
                registerSecond = alTextBox;
            else if (selected == "BH")
                registerSecond = bhTextBox;
            else if (selected == "BL")
                registerSecond = blTextBox;
            else if (selected == "CH")
                registerSecond = chTextBox;
            else if (selected == "CL")
                registerSecond = clTextBox;
            else if (selected == "DH")
                registerSecond = dhTextBox;
            else if (selected == "DL")
                registerSecond = dlTextBox;
        }

        private void XCHNG(object sender, RoutedEventArgs e)
        {
            test2.Text = registerSecond.Text;
            registerSecond.Text = registerFirst.Text;
            registerFirst.Text = test2.Text;
        }
    }
}
