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

namespace Lab2_lotto
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int n;
        int [] dinRad;
        int[] lotto;
        TextBox[] tbox;
        int numDraw;
        public MainPage()
        {
            this.InitializeComponent();
            txtBlockLottoRad.Text = "Din lottorad:";
            n = 7;
            dinRad = new int[n];
            lotto = new int[n];
            tbox = new TextBox[] {tBoxNum1, tBoxNum2,tBoxNum3, tBoxNum4, tBoxNum5,tBoxNum6, tBoxNum7 };
            Debug.WriteLine("This is a debug message.");
            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            try
            {
                numDraw = int.Parse(tBoxNumDraw.Text.ToString());
                int fiveCorr = 0, sixCorr = 0, sevCorr = 0;
                dinLottoRad();

                if (IsDuplicateInput())
                {
                    DialogBox("Duplicate numbers",
                                "You have entered Duplicated numbers");
                }
                else if (!InRange())
                {
                    //InRange();
                    DialogBox(  "Out of Range",
                                "Your input is out of range. Enter a number between 1 and 35 included.");
                }
                else if (numDraw <= 0)
                {
                    DialogBox("Invalid number of draws",
                                "Number of draw should at least be 1.");
                }
                else
                {
                    while (numDraw != 0)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            int rand;
                            bool isDuplicate;

                            do
                            {
                                rand = random.Next(1, 36);
                                isDuplicate = false;

                                for (int j = 0; j < i; j++) // check for duplicate => if so generate random again
                                {
                                    if (lotto[j] == rand)
                                    {
                                        isDuplicate |= true;
                                        break;
                                    }
                                }
                            } while (isDuplicate);

                            lotto[i] = rand;

                        }
                        Array.Sort(lotto);

                        bool isEqual5 = CompareArrays(lotto, dinRad, 5);
                        bool isEqual6 = CompareArrays(lotto, dinRad, 6);
                        bool isEqual7 = CompareArrays(lotto, dinRad, 7);

                        if (isEqual5)
                        {
                            fiveCorr++;
                        }
                        if (isEqual6)
                        {
                            sixCorr++;
                            fiveCorr--;
                        }

                        if (isEqual7)
                        {
                            sevCorr++;
                            sixCorr--;
                        }

                        numDraw--;
                    }
                    Debug.WriteLine($"5 Correct: {fiveCorr}");
                    Debug.WriteLine($"6 Correct: {sixCorr}");
                    Debug.WriteLine($"7 Correct: {sevCorr}");
                    tBox5Correct.Text = fiveCorr.ToString();
                    tBox6Correct.Text = sixCorr.ToString();
                    tBox7Correct.Text = sevCorr.ToString();

                }
            }
            catch(FormatException)
            {
                Debug.WriteLine("Format exp");
                DialogBox("Invalid Input", "Please enter a valid format");
            }
            catch (OverflowException)
            {
                Debug.WriteLine("Too big or small number!");
                DialogBox("Extreme Value", "Your inpur is too big or small number. Please enter a valid number");
            }

            

        }
        void dinLottoRad() 
        {            
            for (int i = 0; i<n; i++)
            {
                dinRad[i] = int.Parse(tbox[i].Text.ToString());
            }            
            
        }

        bool IsDuplicateInput()
        {
            for (int i = 0; i < n; i++)
            {
                bool isDuplicate;
                do
                {
                    dinRad[i] = int.Parse(tbox[i].Text.ToString());
                    isDuplicate = false;

                    for (int j = 0; j < i; j++) // check for duplicate input=> if so DialogBox
                    {
                        if (dinRad[j] == int.Parse(tbox[i].Text.ToString()))
                        {
                            isDuplicate |= true;
                            Debug.WriteLine("Dupicated Input!");
                            return true;
                        }
                    }
                } while (isDuplicate);
                dinRad[i] = int.Parse(tbox[i].Text.ToString());
            }
            return false;
        }

        bool InRange()
        {
            for (int i = 0;i < n; i++)
            {
                if (dinRad[i] <= 0 || dinRad[i] > 35)
                {
                    Debug.WriteLine("Your input is out of the range!");
                    return false;
                }
            }
            return true;
        }
        bool CompareArrays(int[] array1, int[] array2, int length)
        {
            HashSet<int> set1 = new HashSet<int>(array1);
            HashSet<int> set2 = new HashSet<int>(array2);
            HashSet<int> commonElements = new HashSet<int>(set1.Intersect(set2));
            return commonElements.Count >= length;
        }
        async void DialogBox(string tittle, string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = tittle,
                Content = content,
                PrimaryButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
