using Microsoft.Win32;
using System;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
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
using OMDbApiNet;
using OMDbApiNet.Model;
using OMDbApiNet.Utilities;

namespace Serie_List_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //instance of the data to display and save
        private SaveDataJson m_data = new SaveDataJson();

        private string m_file = "";

        public MainWindow()
        {
            //starting the program
            InitializeComponent();

            UpdateUI();

            FileNameButton.Content = MyConsts.NoFileYet;
            FileNameButton.HorizontalContentAlignment = HorizontalAlignment.Center;

            try
            {
                string m_file = $"{RootPath}\\{RootFolderName}\\{RootFileName}";

                //checks if the root folder "Saved SerieList Editor Map" exitsts, if not it will create it
                if (!Directory.Exists($"{RootPath}\\{RootFolderName}"))
                {
                    Directory.CreateDirectory($"{RootPath}\\{RootFolderName}");
                }

                //checks if the default file exitsts, if not it will create it
                if (!File.Exists(m_file))
                {
                    File.Create(m_file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SizeChanged += WindowSizeChange;
        }

        private void WindowSizeChange(object sender, SizeChangedEventArgs args)
        {
            if (args.HeightChanged)
            {
                UpdateUI();
            }
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _dialog = new OpenFileDialog
            {
                Filter = MyConsts.JsonFilter,
                InitialDirectory = $"{RootPath}\\{RootFolderName}",
                Title = "Open Data Container",
            };

            if (_dialog.ShowDialog() == true)
            {
                m_file = _dialog.FileName;
                FileNameButton.Content = _dialog.FileName;
                FileNameButton.HorizontalContentAlignment = HorizontalAlignment.Left;

                try
                {
                    m_data = new SaveDataJson();
                    m_data = JsonConvert.DeserializeObject<SaveDataJson>(File.ReadAllText(m_file));

                    UpdateUI();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{MyConsts.FileUnreadable} {GetFileName(_dialog.FileName)} \n{ex.Message}");
                }
            }
        }

        private void Create_Temp_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveDataJson _data = new SaveDataJson(true);

                _data.AddNewEntry("Arrow", 5, 9);
                _data.AddNewEntry("Flash", 3, 5);
                _data.AddNewEntry("The 100", 3);

                SaveFileDialog _dialog = new SaveFileDialog
                {
                    Filter = MyConsts.JsonFilter,
                    InitialDirectory = $"{RootPath}\\{RootFolderName}",
                };

                if (_dialog.ShowDialog() == true)
                {
                    File.WriteAllText(_dialog.FileName, JsonConvert.SerializeObject(_data, Formatting.Indented));
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (m_file != "")
            {
                string json = JsonConvert.SerializeObject(m_data);
                MessageBox.Show(json);
                File.WriteAllText(m_file, json);
            }
        }

        private void Save_As_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog()
            {
                Filter = MyConsts.JsonFilter,
                InitialDirectory = $"{RootPath}\\{RootFolderName}",
                Title = "Save Data Container",
            };

            if (_dialog.ShowDialog() == true)
            {
                string json = JsonConvert.SerializeObject(m_data);
                MessageBox.Show(json);
                File.WriteAllText(_dialog.FileName, json);

                FileNameButton.Content = $"File Location: {_dialog.FileName}";
                FileNameButton.HorizontalContentAlignment = HorizontalAlignment.Left;
            }
        }

        private void File_folder_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)FileNameButton.Content != MyConsts.NoFileYet)
            {
                Process.Start($"{GetFileFolder((string)FileNameButton.Content)}");
            }
            else
            {
                Process.Start($"{RootPath}\\{RootFolderName}");
            }
        }

        private void New_Entry_Button_Click(object sender, RoutedEventArgs e)
        {
            m_data.AddNewEntry("New Title", 1, 1, "Empty Note");
            UpdateUI();
        }

        private void Delete_Last_Entry_Button_Click(object sender, RoutedEventArgs e)
        {
            m_data.RemoveEntry(m_data.Title.Count - 1);
            UpdateUI();
        }

        private void Display_Online_Info_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNetworkAvailable)
                {
                    if (m_lastFocusedTitle != null && m_lastFocusedTitle != "")
                    {
                        OmdbClient _client = new OmdbClient(MyConsts.API_KEY);
                        Item _responce = _client.GetItemByTitle(m_lastFocusedTitle, true);
                        DisplayInfo display = new DisplayInfo(_responce);
                        display.ShowDialog();
                    }
                    else
                    {
                        DisplayInfo display = new DisplayInfo();
                        display.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("No internet connetction found...", "Could not find a connection", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show($"{EX.Message}", "Title Not Found \nPlease enter a valid Title", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

//Grid info
//https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/how-to-position-the-child-elements-of-a-grid
//https://stackoverflow.com/questions/11878217/add-items-to-combobox-in-wpf
//https://stackoverflow.com/questions/3745594/how-to-set-grid-row-and-column-positions-programmatically