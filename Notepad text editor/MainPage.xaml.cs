using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notepad_text_editor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string filnamn;
        StorageFile file;
        bool IsCharacterReceived= false; // Is received char saved?
        bool isSaved = false;
        bool shouldClose = false;

        public MainPage()
        {
            this.InitializeComponent();
            fileName.Text = "New Document";
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //lineCount();
            cahrsCounter.Text = "Chars: " + allCharCount().ToString();
            charCounter.Text = "Char: " + charCount().ToString();
            wordCounter.Text = "Words: " + wordCountr().ToString();
            lineCounter.Text = "Lines: " + lineCount().ToString();
        }

        private void tBox_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            IsCharacterReceived = true;
            //charCount();
            Debug.WriteLine("Character received!");
            if (file != null)
                fileName.Text = "*" + file.Name;
            else 
                fileName.Text = "*" + "New Document";



            args.Handled = true;
        }

        private void tBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                var ss = tBox.SelectionStart;
                tBox.Text = tBox.Text.Insert(ss, "\t");
                tBox.SelectionStart = ss + "\t".Length;
             
                e.Handled = true;
            }
        }

        private async void OpenBtn_Click(object sender, RoutedEventArgs e)
        {

            if (IsCharacterReceived)
            {
                //ShowMessageDialog();
                if (file != null)
                {
                    var msgDialog = new MessageDialog($"Do you want to save changes to {file.Name}") { Title = "Tip title" };
                    msgDialog.Commands.Add(new UICommand("Yes", uiCommand => { 
                        Debug.WriteLine($"You click：{uiCommand.Label}"); 
                        saveBtn_Click(sender, e);
                        if (isSaved)
                        {
                            openFile();
                        }
                    
                        }));
                    msgDialog.Commands.Add(new UICommand("No", uiCommand => { 
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        openFile();
                        
                        }));
                    msgDialog.Commands.Add(new UICommand("Cancel", uiCommand => { 
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        IsCharacterReceived = true;
                        return;
                    }));
                    await msgDialog.ShowAsync();
                }
                else
                {
                    var msgDialog = new MessageDialog($"Do you want to save changes to New Document") { Title = "Tip title" };
                    msgDialog.Commands.Add(new UICommand("Yes", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        saveBtn_Click(sender, e);
                        if (isSaved)
                        {
                            openFile();
                        }

                    }));
                    msgDialog.Commands.Add(new UICommand("No", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        openFile();
                    }));
                    msgDialog.Commands.Add(new UICommand("Cancel", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        IsCharacterReceived = true;
                        return;
                    })); await msgDialog.ShowAsync();
                }
            }
            else
            {
                openFile();
                Debug.WriteLine($"IsCharReceived：{IsCharacterReceived}");
            }
            
        }
        private async void openFile()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".txt");

            file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                fileName.Text = file.Name;
                string txt = await FileIO.ReadTextAsync(file);
                tBox.Text = txt;
                isSaved = true;
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
                isSaved = false;
            }
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, tBox.Text);
                fileName.Text = file.Name;
                IsCharacterReceived = false;
                if (!IsCharacterReceived && shouldClose)
                    Application.Current.Exit();

                //isSaved = true;
            }
            else
            {
                var picker = new FileSavePicker();
                picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                picker.SuggestedFileName = "New Document";
                file = await picker.PickSaveFileAsync();
                //IsCharacterReceived = false;
                if (file != null)
                {
                    await FileIO.WriteTextAsync(file, tBox.Text);
                    fileName.Text = file.Name;
                    IsCharacterReceived = false;
                    //isSaved= true;
                }
                if(!IsCharacterReceived && shouldClose)
                    Application.Current.Exit();
            }
        }

        private async void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            picker.SuggestedFileName = "New Document";
            file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, tBox.Text);
                fileName.Text = file.Name;
                IsCharacterReceived = false;
            }
        }
        private int allCharCount()
        {
            int count = 0;
            foreach (char c in tBox.Text)
            {
                count++;
            }
            Debug.WriteLine($"char count: {count}");
            return count;
        }

        private int charCount()
        {
            int count = 0;
            foreach (char c in tBox.Text)
            {
                if (c == '\n' || c == ' ' || c == '\t' || c== '\r')
                    continue;
                count++;
            }
            Debug.WriteLine($"char count: {count}");
            return count;
        }
        private int wordCountr()
        {
            char[] delimiters = new char[] { ' ', '\r', '\n', '\t' };
            int count = 0;
            count= tBox.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            return count = tBox.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length; ;
        }

        private int lineCount()
        {
            int count = tBox.Text.Split('\r').Length;
            Debug.WriteLine($"lines count: {count}");
            return count;
        }

        private async void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsCharacterReceived)
            {
                //ShowMessageDialog();
                if (file != null)
                {
                    var msgDialog = new MessageDialog($"Do you want to save changes to {file.Name}") { Title = "Tip title" };
                    msgDialog.Commands.Add(new UICommand("Yes", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        // Do the same as Save btn
                        saveBtn_Click(sender, e);
                        shouldClose = true;
                    }));
                    msgDialog.Commands.Add(new UICommand("No", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        Application.Current?.Exit();
                    }));
                    msgDialog.Commands.Add(new UICommand("Cancel", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        IsCharacterReceived = true;
                        return;
                    }));
                    await msgDialog.ShowAsync();
                }
                else
                {
                    var msgDialog = new MessageDialog($"Do you want to save changes to New Document") { Title = "Tip title" };
                    msgDialog.Commands.Add(new UICommand("Yes", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        // Do the same as Save btn
                        saveBtn_Click(sender, e);
                        shouldClose = true;
                    }));
                    msgDialog.Commands.Add(new UICommand("No", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");

                        Application.Current?.Exit();
                    }));
                    msgDialog.Commands.Add(new UICommand("Cancel", uiCommand => {
                        Debug.WriteLine($"You click：{uiCommand.Label}");
                        IsCharacterReceived = true;
                        return;
                    })); 
                    await msgDialog.ShowAsync();
                }
            }
            else
            {
                Application.Current?.Exit();
            }
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            tBox.Text = "";

        }
    }
}
