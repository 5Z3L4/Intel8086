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
        int memoryIndex;
        string[] memory = new string[65536];
        string[] memoryHelper = new string[65536];
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
            //MOV 8 bit (only half of register) AH,BH etc.
            if((properlyCheck && properlyCheck2 || !properlyCheck && !properlyCheck2) && registerFirst!=null && registerSecond !=null)
            {
                registerFirst.Text = registerSecond.Text;
            }
            //MOV 16 bit whole registers AX, BX etc.
            if (registerFirstHelper != null && registerSecondHelper != null)
            {
                registerFirstHelper.Text = registerSecondHelper.Text;
            }
            
            //TO DO Based Indexed BX BP and one of SI DI
            //MOV [DI]+12, AL
            //
            //check if movTextBox isn't empty (its not working atm)
            if (movTextBox.Text!=null) 
            {
                //MOV value to memory
                if(selected2 == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            bxValue = bhTextBox.Text + blTextBox.Text;
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            bxValue = bpTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        memory[memoryIndex] = registerSecond.Text;
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        memory[memoryIndex] = registerSecond.Text;
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            dsValue += Convert.ToUInt32(bhTextBox.Text + blTextBox.Text, 16);
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            dsValue = Convert.ToUInt32(ssTextBox.Text, 16);
                            dsValue += Convert.ToUInt32(bpTextBox.Text, 16);
                        }
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }
                        
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        memory[memoryIndex] = registerSecond.Text;
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        Trace.WriteLine(memoryIndex);
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        memory[memoryIndex] = registerSecond.Text; //8 bit
                        if (registerSecondHelper != null) //16 bit
                        {
                            memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        }
                    }
                    
                }
                else if(selected == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        string bxValue ="5";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if((bool)bxRB.IsChecked)
                        {
                            bxValue = bhTextBox.Text + blTextBox.Text;
                        }
                        else if((bool)bpRB.IsChecked)
                        {
                            bxValue = bpTextBox.Text;
                        }
                        
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        Trace.WriteLine(memoryIndex);
                        registerFirst.Text = memory[memoryIndex];
                        registerFirstHelper.Text = memoryHelper[memoryIndex];
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        string bxValue = "5";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text; ;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }

                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        Trace.WriteLine(memoryIndex);
                        registerFirst.Text = memory[memoryIndex];
                        registerFirstHelper.Text = memoryHelper[memoryIndex];
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            dsValue += Convert.ToUInt32(bhTextBox.Text + blTextBox.Text, 16);
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            dsValue = Convert.ToUInt32(ssTextBox.Text, 16);
                            dsValue += Convert.ToUInt32(bpTextBox.Text, 16);
                        }
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }

                        dsValue += Convert.ToUInt32(bxValue, 16);

                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        registerFirst.Text = memory[memoryIndex];
                        registerFirstHelper.Text = memoryHelper[memoryIndex];
                        Trace.WriteLine(memoryIndex);
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        registerFirst.Text = memory[memoryIndex];
                        if (registerFirstHelper != null)
                        {
                            registerFirstHelper.Text = memoryHelper[memoryIndex];
                        }
                    }
                }
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
            else if (selected2 == "memory")
                registerFirst = null;
            
            
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
            else if (selected == "memory")
                registerSecond = null;
            if (selected == "AX")
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
            if (properlyCheck && properlyCheck2 || !properlyCheck && !properlyCheck2) //8 bit
            {
                holder.Text = registerSecond.Text;
                registerSecond.Text = registerFirst.Text;
                registerFirst.Text = holder.Text;
            }
            if (registerFirstHelper != null & registerSecondHelper != null) //16 bit
            {
                holderHelper.Text = registerSecondHelper.Text;
                registerFirstHelper.Text = holderHelper.Text;
                registerSecondHelper.Text = registerFirstHelper.Text;
            }
            if (movTextBox.Text != null)
            {

                if (selected2 == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            bxValue = bhTextBox.Text + blTextBox.Text;
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            bxValue = bpTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerSecond.Text;
                        registerSecond.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        registerSecondHelper.Text = temporaryHelper;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerSecond.Text;
                        registerSecond.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        registerSecondHelper.Text = temporaryHelper;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            dsValue += Convert.ToUInt32(bhTextBox.Text + blTextBox.Text, 16);
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            dsValue = Convert.ToUInt32(ssTextBox.Text, 16);
                            dsValue += Convert.ToUInt32(bpTextBox.Text, 16);
                        }
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }

                        dsValue += Convert.ToUInt32(bxValue, 16);

                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerSecond.Text;
                        registerSecond.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        registerSecondHelper.Text = temporaryHelper;
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        string temp = memory[memoryIndex];
                        memory[memoryIndex] = registerSecond.Text;
                        registerSecond.Text = temp;
                        if (registerSecondHelper != null)
                        {
                            temp = registerSecondHelper.Text;
                            memoryHelper[memoryIndex] = registerSecondHelper.Text;
                            registerSecondHelper.Text = temp;
                        }
                    }
                    
                }
                else if (selected == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            bxValue = bhTextBox.Text + blTextBox.Text;
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            bxValue = bpTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerFirst.Text;
                        registerFirst.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerFirstHelper.Text;
                        registerFirstHelper.Text = temporaryHelper;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }
                        dsValue += Convert.ToUInt32(bxValue, 16);
                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerFirst.Text;
                        registerFirst.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerFirstHelper.Text;
                        registerFirstHelper.Text = temporaryHelper;
                        Trace.WriteLine(memoryIndex);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        string bxValue = "0";
                        uint dsValue = Convert.ToUInt32(dsTextBox.Text, 16);
                        if ((bool)bxRB.IsChecked)
                        {
                            dsValue += Convert.ToUInt32(bhTextBox.Text + blTextBox.Text, 16);
                        }
                        else if ((bool)bpRB.IsChecked)
                        {
                            dsValue = Convert.ToUInt32(ssTextBox.Text, 16);
                            dsValue += Convert.ToUInt32(bpTextBox.Text, 16);
                        }
                        if ((bool)siRB.IsChecked)
                        {
                            bxValue = siTextBox.Text;
                        }
                        else if ((bool)diRB.IsChecked)
                        {
                            bxValue = diTextBox.Text;
                        }

                        dsValue += Convert.ToUInt32(bxValue, 16);

                        dsValue += Convert.ToUInt32(movTextBox.Text, 16);
                        memoryIndex = Convert.ToInt32(dsValue);
                        string temporary = memory[memoryIndex];
                        memory[memoryIndex] = registerFirst.Text;
                        registerFirst.Text = temporary;
                        string temporaryHelper = memoryHelper[memoryIndex];
                        memoryHelper[memoryIndex] = registerFirstHelper.Text;
                        registerFirstHelper.Text = temporaryHelper;
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        string temp = registerFirst.Text;
                        registerFirst.Text = memory[memoryIndex];
                        memory[memoryIndex] = temp;
                        if (registerFirstHelper != null)
                        {
                            temp = registerFirstHelper.Text;
                            registerFirstHelper.Text = memoryHelper[memoryIndex];
                            memoryHelper[memoryIndex] = temp;

                        }
                    }
                    
                }
            }

        }
    }
}
