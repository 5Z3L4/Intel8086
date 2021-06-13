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
        string bxValue = "0";
        bool properlyCheck;
        bool properlyCheck2;
        int memoryIndex;
        uint dsValue;
        string[] memory = new string[65536];
        string[] memoryHelper = new string[65536];
        TextBox wholeBx = new TextBox();
        TextBox registerFirst = new TextBox();
        TextBox registerFirstHelper = new TextBox();
        TextBox registerSecond = new TextBox();
        TextBox registerSecondHelper = new TextBox();
        Stack<string> intelStack = new Stack<string>();
        Stack<string> intelStack2 = new Stack<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        //handles base and index addressing modes
        public void ChooseAddressingMode(RadioButton bxOrSiRB, TextBox bxOrSiTB, RadioButton bpOrDiRB, TextBox bpOrDiTB, TextBox dsTB, TextBox ssTB, bool bToDS)
        {
            if ((bool)bxOrSiRB.IsChecked)
            {
                if(!bToDS)
                    bxValue = bxOrSiTB.Text;
                else
                {
                    dsValue = Convert.ToUInt32(dsTB.Text, 16);
                    dsValue += Convert.ToUInt32(bxOrSiTB.Text, 16);
                }
                    
            }
            else if ((bool)bpOrDiRB.IsChecked)
            {
                if(!bToDS)
                    bxValue = bpOrDiTB.Text;
                else
                {
                    dsValue = Convert.ToUInt32(ssTB.Text, 16);
                    dsValue += Convert.ToUInt32(bpOrDiTB.Text, 16);
                }
                    
            }

        }

        public void AssignMemory(TextBox leftHalfOfRegister, TextBox rightHalfOfRegister, bool moveDataHere, bool xchgData)
        {
            dsValue += Convert.ToUInt32(bxValue, 16);
            dsValue += Convert.ToUInt32(movTextBox.Text, 16);
            memoryIndex = Convert.ToInt32(dsValue);
            if(!xchgData)
            {
                if (moveDataHere)
                {
                    memory[memoryIndex] = leftHalfOfRegister.Text;
                    memoryHelper[memoryIndex] = rightHalfOfRegister.Text;
                }
                else
                {
                    leftHalfOfRegister.Text = memory[memoryIndex];
                    rightHalfOfRegister.Text = memoryHelper[memoryIndex];
                }

                Trace.WriteLine(memoryIndex);
                Trace.WriteLine(memory[memoryIndex]);
                Trace.WriteLine(memoryHelper[memoryIndex]);
            }
            else
            {
                string temporary = memory[memoryIndex];
                memory[memoryIndex] = leftHalfOfRegister.Text;
                leftHalfOfRegister.Text = temporary;
                string temporaryHelper = memoryHelper[memoryIndex];
                memoryHelper[memoryIndex] = rightHalfOfRegister.Text;
                rightHalfOfRegister.Text = temporaryHelper;
                Trace.WriteLine(memoryIndex);
            }
            
        }

        private void MOV(object sender, RoutedEventArgs e)
        {
            dsValue = 0;
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
            //
            //check if movTextBox isn't empty (its not working atm)
            if (movTextBox.Text!=null) 
            {
                //MOV value to memory
                if(selected2 == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx , bpRB, bpTextBox, dsTextBox, ssTextBox,  false);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, true);
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        memory[memoryIndex] = registerSecond.Text; //8 bit
                        if (registerSecondHelper != null) //16 bit
                        {
                            memoryHelper[memoryIndex] = registerSecondHelper.Text;
                        }
                        return;
                    }
                    AssignMemory(registerSecond, registerSecondHelper, true, false);
                }
                else if(selected == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, false);
                        
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, true);
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else
                    {
                        memoryIndex = Convert.ToInt32(movTextBox.Text);
                        registerFirst.Text = memory[memoryIndex];
                        if (registerFirstHelper != null)
                        {
                            registerFirstHelper.Text = memoryHelper[memoryIndex];
                        }
                        return;
                    }
                    AssignMemory(registerFirst, registerFirstHelper, false, false);
                }
            }
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
                wholeBx.Text = registerFirstHelper.Text + registerFirst.Text;
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
                wholeBx.Text = registerSecond.Text + registerSecondHelper.Text;
                Trace.WriteLine(wholeBx.Text);
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
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, true);
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
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
                        return;
                    }
                    AssignMemory(registerSecond, registerSecondHelper, false, true);

                }
                else if (selected == "memory")
                {
                    if ((bool)BasedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)indexedCB.IsChecked)
                    {
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
                    }
                    else if ((bool)bindexedCB.IsChecked)
                    {
                        ChooseAddressingMode(bxRB, wholeBx, bpRB, bpTextBox, dsTextBox, ssTextBox, true);
                        ChooseAddressingMode(siRB, siTextBox, diRB, diTextBox, dsTextBox, ssTextBox, false);
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
                        return;
                    }
                    AssignMemory(registerFirst, registerFirstHelper, false, true);
                }
            }

        }

        private void PUSH(object sender, RoutedEventArgs e)
        {
            intelStack.Push(registerFirst.Text);
            intelStack2.Push(registerFirstHelper.Text);
            int temp = Convert.ToInt32(spTextBox.Text);
            temp += 2;
            spTextBox.Text = Convert.ToString(temp);
        }

        private void POP(object sender, RoutedEventArgs e)
        {
            int temp = Convert.ToInt32(spTextBox.Text);
            temp -= 2;
            spTextBox.Text = Convert.ToString(temp);
            registerFirst.Text = intelStack.Pop();
            registerFirstHelper.Text = intelStack2.Pop();
        }
    }
}
