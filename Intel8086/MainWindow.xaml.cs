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
        string selected;
        string selected2;
        bool properlyCheck;
        bool properlyCheck2;
        TextBox registerFirst = new TextBox();
        TextBox registerFirstHelper = new TextBox();
        TextBox registerSecond = new TextBox();
        TextBox registerSecondHelper = new TextBox();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MOV(object sender, RoutedEventArgs e)
        {
            if(properlyCheck && properlyCheck2 || !properlyCheck && !properlyCheck2)
            registerFirst.Text = registerSecond.Text;
            if (registerFirstHelper != null && registerSecondHelper != null)
            {
                registerFirstHelper.Text = registerSecondHelper.Text;
            }
        }

        private void PUSH()
        {
            //rejestr do stosu
            //SP+2
        }

        private void POP()
        {
            //sp-2
            //rejestr odejmujemy od stosu
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
            
            if (selected2 == "AX")
            {
                registerFirst = ahTextBox;
                registerFirstHelper = alTextBox;
                properlyCheck = false;
            }
            else if (selected2 == "BX")
            {
                registerFirst = bhTextBox;
                registerFirstHelper = blTextBox;
                properlyCheck = false;
            }
            else if (selected2 == "CX")
            {
                registerFirst = chTextBox;
                registerFirstHelper = clTextBox;
                properlyCheck = false;
            }
            else if (selected2 == "DX")
            {
                registerFirst = dhTextBox;
                registerFirstHelper = dlTextBox;
                properlyCheck = false;
            } 
            else
            {
                registerFirstHelper = null;
                properlyCheck = true;
            }
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
            if(selected == "AX")
            {
                registerSecond = ahTextBox;
                registerSecondHelper = alTextBox;
                properlyCheck2 = false;
            }
            else if (selected == "BX")
            {
                registerSecond = bhTextBox;
                registerSecondHelper = blTextBox;
                properlyCheck2 = false;
            }
            else if (selected == "CX")
            {
                registerSecond = chTextBox;
                registerSecondHelper = clTextBox;
                properlyCheck2 = false;
            }
            else if (selected == "DX")
            {
                registerSecond = dhTextBox;
                registerSecondHelper = dlTextBox;
                properlyCheck2 = false;

            }
            else
            {
                registerSecondHelper = null;
                properlyCheck2 = true;
            }
        }

        private void XCHNG(object sender, RoutedEventArgs e)
        {
            TextBox holder = new TextBox();
            TextBox holderHelper = new TextBox();
            if (properlyCheck && properlyCheck2 || !properlyCheck && !properlyCheck2)
            {
                holder.Text = registerSecond.Text;
                registerSecond.Text = registerFirst.Text;
                registerFirst.Text = holder.Text;
            }
            if (registerFirstHelper != null & registerSecondHelper != null)
            {
                holderHelper.Text = registerSecondHelper.Text;
                registerFirstHelper.Text = holderHelper.Text;
                registerSecondHelper.Text = registerFirstHelper.Text;
            }

        }
    }
}
