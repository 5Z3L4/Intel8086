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
        string secondRegisterSelected;
        string firstRegisterSelected;
        bool isItHalfRegisterFirst;
        bool isItHalfRegisterSecond;
        int memoryIndex;
        string addressRegisterValue = "0"; // value of BX, BP, DI or SI register
        uint segmentRegisterValue;
        string[] memory = new string[65536]; //Memory for ..h(first) 8 bit registers
        string[] memoryHelper = new string[65536]; // memory for ..l(second) 8 bit registers
        TextBox wholeBx = new TextBox(); //main registers are handled as two parts, but BX is needed as one to handle memory index calculations
        TextBox registerFirst = new TextBox(); // first half of register (left)
        TextBox registerFirstHelper = new TextBox(); // second half of register (right)
        TextBox registerSecond = new TextBox(); //same but with second register chosen
        TextBox registerSecondHelper = new TextBox();
        Stack<string> intelStackForFirstHalfRegister = new Stack<string>(); //Handling separate stacks for each half of register is easier than dividing text
        Stack<string> intelStackForSecondHalfRegister = new Stack<string>();

        public MainWindow()
        {
            InitializeComponent();
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
            firstRegisterSelected = (sender as ComboBox).Text;

            if (firstRegisterSelected == "AH")
                registerFirst = ahTextBox;
            else if (firstRegisterSelected == "AL")
                registerFirst = alTextBox;
            else if (firstRegisterSelected == "BH")
                registerFirst = bhTextBox;
            else if (firstRegisterSelected == "BL")
                registerFirst = blTextBox;
            else if (firstRegisterSelected == "CH")
                registerFirst = chTextBox;
            else if (firstRegisterSelected == "CL")
                registerFirst = clTextBox;
            else if (firstRegisterSelected == "DH")
                registerFirst = dhTextBox;
            else if (firstRegisterSelected == "DL")
                registerFirst = dlTextBox;
            else if (firstRegisterSelected == "memory")
                registerFirst = null;
            
            
            if (firstRegisterSelected == "AX")
            {
                registerFirst = ahTextBox;
                registerFirstHelper = alTextBox;
                isItHalfRegisterFirst = false;
            }
            else if (firstRegisterSelected == "BX")
            {
                registerFirst = bhTextBox;
                registerFirstHelper = blTextBox;
                wholeBx.Text = registerFirstHelper.Text + registerFirst.Text;
                isItHalfRegisterFirst = false;
            }
            else if (firstRegisterSelected == "CX")
            {
                registerFirst = chTextBox;
                registerFirstHelper = clTextBox;
                isItHalfRegisterFirst = false;
            }
            else if (firstRegisterSelected == "DX")
            {
                registerFirst = dhTextBox;
                registerFirstHelper = dlTextBox;
                isItHalfRegisterFirst = false;
            } 
            else
            {
                registerFirstHelper = null;
                isItHalfRegisterFirst = true;
            }
        }

        private void secondRegister_DropDownClosed(object sender, EventArgs e)
        {
            secondRegisterSelected = (sender as ComboBox).Text;
            if (secondRegisterSelected == "AH")
                registerSecond = ahTextBox;
            else if (secondRegisterSelected == "AL")
                registerSecond = alTextBox;
            else if (secondRegisterSelected == "BH")
                registerSecond = bhTextBox;
            else if (secondRegisterSelected == "BL")
                registerSecond = blTextBox;
            else if (secondRegisterSelected == "CH")
                registerSecond = chTextBox;
            else if (secondRegisterSelected == "CL")
                registerSecond = clTextBox;
            else if (secondRegisterSelected == "DH")
                registerSecond = dhTextBox;
            else if (secondRegisterSelected == "DL")
                registerSecond = dlTextBox;
            else if (secondRegisterSelected == "memory")
                registerSecond = null;
            if (secondRegisterSelected == "AX")
            {
                registerSecond = ahTextBox;
                registerSecondHelper = alTextBox;
                isItHalfRegisterSecond = false;
            }
            else if (secondRegisterSelected == "BX")
            {
                registerSecond = bhTextBox;

                registerSecondHelper = blTextBox;
                wholeBx.Text = registerSecond.Text + registerSecondHelper.Text;
                Trace.WriteLine(wholeBx.Text);
                isItHalfRegisterSecond = false;
            }
            else if (secondRegisterSelected == "CX")
            {
                registerSecond = chTextBox;
                registerSecondHelper = clTextBox;
                isItHalfRegisterSecond = false;
            }
            else if (secondRegisterSelected == "DX")
            {
                registerSecond = dhTextBox;
                registerSecondHelper = dlTextBox;
                isItHalfRegisterSecond = false;

            }
            else
            {
                registerSecondHelper = null;
                isItHalfRegisterSecond = true;
            }
        }

        private void Mov(object sender, RoutedEventArgs e)
        {
            segmentRegisterValue = 0;
            //MOV 8 bit (only half of register) AH,BH etc.
            if ((isItHalfRegisterFirst && isItHalfRegisterSecond || !isItHalfRegisterFirst && !isItHalfRegisterSecond) && registerFirst != null && registerSecond != null)
            {
                registerFirst.Text = registerSecond.Text;
            }
            //MOV 16 bit whole registers AX, BX etc.
            if (registerFirstHelper != null && registerSecondHelper != null)
            {
                registerFirstHelper.Text = registerSecondHelper.Text;
            }
            //check if movTextBox isn't empty (its not working atm)
            if (movTextBox.Text != null)
            {
                //MOV value to memory
                if (firstRegisterSelected == "memory")
                {
                    ChangeRegisterValues(registerSecond, registerSecondHelper, true, false);
                }
                else if (secondRegisterSelected == "memory")
                {
                    ChangeRegisterValues(registerFirst, registerFirstHelper, false, false);
                }
            }
        }

        private void Xchg(object sender, RoutedEventArgs e)
        {
            TextBox holder = new TextBox();
            TextBox holderHelper = new TextBox();
            if (isItHalfRegisterFirst && isItHalfRegisterSecond || !isItHalfRegisterFirst && !isItHalfRegisterSecond) //8 bit
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

                if (firstRegisterSelected == "memory")
                {
                    ChangeRegisterValues(registerSecond, registerSecondHelper, false, true);
                }
                else if (secondRegisterSelected == "memory")
                {
                    ChangeRegisterValues(registerFirst, registerFirstHelper, false, true);
                }
            }

        }

        private void Push(object sender, RoutedEventArgs e)
        {
            intelStackForFirstHalfRegister.Push(registerFirst.Text);
            intelStackForSecondHalfRegister.Push(registerFirstHelper.Text);
            int temp = Convert.ToInt32(spTextBox.Text);
            temp += 2;
            spTextBox.Text = Convert.ToString(temp);
        }

        private void Pop(object sender, RoutedEventArgs e)
        {
            int temp = Convert.ToInt32(spTextBox.Text);
            temp -= 2;
            spTextBox.Text = Convert.ToString(temp);
            registerFirst.Text = intelStackForFirstHalfRegister.Pop();
            registerFirstHelper.Text = intelStackForSecondHalfRegister.Pop();
        }

        public void ChangeRegisterValues(TextBox register, TextBox registerHelper, bool moveDataHere, bool xchgData)
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
            else if (!xchgData)
            {
                memoryIndex = Convert.ToInt32(movTextBox.Text);
                memory[memoryIndex] = register.Text; //8 bit
                if (registerSecondHelper != null) //16 bit
                {
                    memoryHelper[memoryIndex] = registerHelper.Text;
                }
                return;
            }
            else if (xchgData)
            {
                memoryIndex = Convert.ToInt32(movTextBox.Text);
                string temp = register.Text;
                register.Text = memory[memoryIndex];
                memory[memoryIndex] = temp;
                if (registerHelper != null)
                {
                    temp = registerHelper.Text;
                    registerHelper.Text = memoryHelper[memoryIndex];
                    memoryHelper[memoryIndex] = temp;
                }
                return;
            }
            AssignMemory(register, registerHelper, moveDataHere, xchgData);
        }
        //handles base and index addressing modes
        public void ChooseAddressingMode(RadioButton bxOrSiRB, TextBox bxOrSiTB, RadioButton bpOrDiRB, TextBox bpOrDiTB, TextBox dsTB, TextBox ssTB, bool bToDS)
        {
            if ((bool)bxOrSiRB.IsChecked)
            {
                if (!bToDS)
                    addressRegisterValue = bxOrSiTB.Text;
                else
                {
                    segmentRegisterValue = Convert.ToUInt32(dsTB.Text, 16);
                    segmentRegisterValue += Convert.ToUInt32(bxOrSiTB.Text, 16);
                }

            }
            else if ((bool)bpOrDiRB.IsChecked)
            {
                if (!bToDS)
                    addressRegisterValue = bpOrDiTB.Text;
                else
                {
                    segmentRegisterValue = Convert.ToUInt32(ssTB.Text, 16);
                    segmentRegisterValue += Convert.ToUInt32(bpOrDiTB.Text, 16);
                }

            }

        }

        public void AssignMemory(TextBox leftHalfOfRegister, TextBox rightHalfOfRegister, bool moveDataHere, bool xchgData)
        {
            segmentRegisterValue += Convert.ToUInt32(addressRegisterValue, 16);
            segmentRegisterValue += Convert.ToUInt32(movTextBox.Text, 16);
            memoryIndex = Convert.ToInt32(segmentRegisterValue);
            if (!xchgData)
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
    }
}
