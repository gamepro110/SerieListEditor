using Microsoft.Win32;
using System;
using Newtonsoft.Json;
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
using System.Diagnostics;

namespace Serie_List_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //instance of the data to display and save
        private SaveDataJson m_data = new SaveDataJson();

        private Grid m_grid;

        public MainWindow()
        {
            //starting the program
            InitializeComponent();

            MakeGrid();

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
                throw ex;
            }
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            //m_grid = new Grid();

            OpenFileDialog _dialog = new OpenFileDialog
            {
                Filter = MyConsts.JsonFilter,
                InitialDirectory = $"{RootPath}\\{RootFolderName}",
                Title = "Open Data Container",
            };

            if (_dialog.ShowDialog() == true)
            {
                string m_file = _dialog.FileName;
                FileNameButton.Content = _dialog.FileName;
                FileNameButton.HorizontalContentAlignment = HorizontalAlignment.Left;

                try
                {
                    m_data = new SaveDataJson();
                    m_data = JsonConvert.DeserializeObject<SaveDataJson>(File.ReadAllText(m_file));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{MyConsts.FileUnreadable} {GetFileName(_dialog.FileName)} \n{ex.Message}");
                }

                DrawContent();
            }
        }

        private void Create_Temp_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveDataJson _data = new SaveDataJson();

            _data.AddNewEntry("Arrow", 03, 16);
            _data.AddNewEntry("Flash", 05, 06);
            _data.AddNewEntry("The 100", 01, 37);

            SaveFileDialog _dialog = new SaveFileDialog
            {
                Filter = MyConsts.JsonFilter,
                InitialDirectory = $"{RootPath}\\{RootFolderName}"
            };

            if (_dialog.ShowDialog() == true)
            {
                File.WriteAllText(_dialog.FileName, JsonConvert.SerializeObject(_data, Formatting.Indented));
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog()
            {
                Filter = MyConsts.JsonFilter,
                InitialDirectory = $"{RootPath}\\{RootFolderName}",
                Title = "Save Data Container",
            };

            if (_dialog.ShowDialog() == true)
            {
                File.WriteAllText(_dialog.FileName, JsonConvert.SerializeObject(m_data));

                FileNameButton.Content = _dialog.FileName;
                FileNameButton.HorizontalContentAlignment = HorizontalAlignment.Left;
            }
        }

        private void FileNameButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)FileNameButton.Content != MyConsts.NoFileYet)
            {
                Process.Start($"{GetFileFolder((string)FileNameButton.Content)}");
            }
        }

        private void New_Entry_Button_Click(object sender, RoutedEventArgs e)
        {
            m_data.AddNewEntry("Title", null, null);

            MakeGrid();
            DrawContent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_data.RemoveEntry(m_data.Title.Count - 1);

            MakeGrid();
            DrawContent();
        }
    }
}

//Grid info
//https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/how-to-position-the-child-elements-of-a-grid
//https://stackoverflow.com/questions/11878217/add-items-to-combobox-in-wpf
//https://stackoverflow.com/questions/3745594/how-to-set-grid-row-and-column-positions-programmatically