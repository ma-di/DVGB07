using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notepad_text_editor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string[] readText= new string[1000];
        string filnamn;
        //FileOpenPicker picker;
        StorageFile file;
        public MainPage()
        {
            this.InitializeComponent();
            //picker = new FileOpenPicker();
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tBox_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {

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
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                fileName.Text = file.Name;
                filnamn = file.Name;
                string txt = await FileIO.ReadTextAsync(file);
                tBox.Text = txt;
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
            }
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, tBox.Text);
            }
            else
            {
                Debug.WriteLine("No file to save.");


                var savePicker = new FileSavePicker();
                StorageFolder folder = ApplicationData.Current.LocalFolder;

                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                savePicker.SuggestedFileName = "New Document";
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    try
                    {
                        await FileIO.WriteTextAsync(file, tBox.Text);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Handle unauthorized access exception by prompting the user to choose a different location
                        var picker2 = new FileSavePicker();
                        picker2.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                        picker2.SuggestedFileName = file.Name; // Use the original file name as the suggested name
                        StorageFile newFile = await picker2.PickSaveFileAsync();
                        if (newFile != null)
                        {
                            await FileIO.WriteTextAsync(newFile, tBox.Text);
                        }
                    }
                }

            }
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            if (!string.IsNullOrEmpty(filnamn))
            {
                var fil = await storageFolder.GetFileAsync(filnamn);
                await FileIO.WriteTextAsync(fil, tBox.Text);
                //tBox.Text = result.ToString();
                Debug.WriteLine($"File name: {filnamn}");
            }
            else
                Debug.WriteLine("Could not find the file!");
        }
        public async Task<string> ReadTextFromFileAsync(string fileName)
        {
            try
            {
                // Get the local folder
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                // Get the file
                StorageFile file = await localFolder.GetFileAsync(fileName);

                // Read the text from the file
                string text = await FileIO.ReadTextAsync(file);

                return text;
            }
            catch (Exception ex)
            {
                // Handle any errors appropriately
                return null;
            }
        }

        private async void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            //if (picker.FileTypeChoices.Count > 0)

            picker.SuggestedFileName = "New Document";
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, tBox.Text);
            }
        }

        
    }
}
