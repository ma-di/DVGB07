using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab2_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string inputVal = "";
        private string inputOp = "";
        private int currVal = 0;
        private int prevVal = 0;
        private bool isNewCalc = true;
        private bool isNumClicked = false;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnNum_Click(object sender, RoutedEventArgs e)
        {
            isNumClicked = true;
            //isOperatorClicked= false;
            Button button = (Button)sender;
            string buttonText = button.Content.ToString();
            inputVal += buttonText;
            txtBlockShow.Text = inputVal;
        }

        private void BtnOp_Click(object sender, RoutedEventArgs e)
        {
            if (!isNumClicked)
            {
                inputVal = "";
                isNumClicked = false;
            }
            else
            {
                if (!isNewCalc)
                    Calculate();

                try
                {
                    Button button = (Button)sender;
                    inputOp = button.Content.ToString();
                    if (string.IsNullOrEmpty(inputVal))
                        inputVal = currVal.ToString();
                    prevVal = int.Parse(inputVal);
                    isNewCalc = false;
                    inputVal = string.Empty;

                    //}
                }
                catch (OverflowException)
                {
                    txtBlockShow.Text = "Err: Too big or small number";
                }
            }

        }

        private void BtnClr_Click(object sender, RoutedEventArgs e)
        {
            inputVal = string.Empty;
            inputOp = string.Empty;
            currVal = 0;
            prevVal = 0;
            txtBlockShow.Text = "";
            isNewCalc = true;
        }

        private void BtnEql_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            inputVal = currVal.ToString();
            isNewCalc = true;
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(inputVal))
            {
                Debug.WriteLine("current Input is Empty");
                return;
            }
            int x = int.MaxValue;
            currVal = int.Parse(inputVal);

            switch (inputOp)
            {
                case "+":
                    if (prevVal > 0 && currVal > 0 && prevVal + currVal < prevVal)
                    {
                        txtBlockShow.Text = "Err: Too big or small number";
                        return;
                    }
                    currVal = prevVal + currVal;
                    break;
                case "-":
                    if (prevVal < 0 && currVal < 0 && prevVal - currVal < prevVal)
                    {
                        txtBlockShow.Text = "Err: Too big or small number";
                        return;
                    }
                    currVal = prevVal - currVal;
                    break;
                case "x":
                    if (prevVal != 0 && currVal != 0 && prevVal * currVal / currVal != prevVal)
                    {
                        txtBlockShow.Text = "Err: Too big or small number";
                        return;
                    }
                    currVal = prevVal * currVal;
                    break;
                case "÷":
                    if (currVal != 0)
                        currVal = prevVal / currVal;
                    else
                    {
                        //DisplayTextBox.FontSize = 12;
                        txtBlockShow.Text = "Undef: Division by zero";

                        return;
                    }
                    break;
            }

            txtBlockShow.Text = currVal.ToString();
            prevVal = currVal;
            inputVal = string.Empty;
        }
    }
}
