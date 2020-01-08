using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Serie_List_Editor
{
    public partial class MainWindow
    {
        internal static readonly string RootFolderName = "Saved SerieList Editor Map";
        internal static readonly string RootFileName = $"{Environment.UserName}.json";

        internal static string RootPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private const int m_boxHeight = 20;
        private const int m_boxWidth = 70;

        internal static string GetFileName(string path)
        {
            string[] fullPath = path.Split('\\');
            return fullPath[fullPath.Length - 1];
        }

        internal static string GetFileFolder(string path)
        {
            string[] fullPath = path.Split('\\');
            string newPath = "";

            for (int i = 0; i < fullPath.Length - 1; i++)
            {
                newPath += $"{fullPath[i]}\\";
            }

            return newPath;
        }

        internal void MakeGrid()
        {
            m_grid = new Grid
            {
                Margin = new Thickness(10, 50, 10, 90),
                Background = Brushes.Gray,
            };
            BaseGrid.Children.Add(m_grid);
        }

        internal void DrawContent()
        {
            //amount of columns
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());

            //amount of rows for the seasons and
            for (int i = 0; i < m_data.Title.Count; i++)
            {
                m_grid.RowDefinitions.Add(new RowDefinition());

                //Title
                TextBox _titleTextBlock = new TextBox()
                {
                    Foreground = Brushes.Yellow,
                    BorderBrush = Brushes.Cyan,
                    Background = Brushes.Gray,
                    Height = m_boxHeight,
                    Width = m_boxWidth * 1.5f,
                    Text = m_data.Title[i],
                };
                _titleTextBlock.TextChanged += OnTextChangeGrid;

                Grid.SetColumn(_titleTextBlock, 0);
                Grid.SetRow(_titleTextBlock, i);
                m_grid.Children.Add(_titleTextBlock);

                //Season
                ComboBox _boxS = new ComboBox
                {
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    SelectedValue = m_data.Season[i],
                };
                _boxS.DataContextChanged += BoxS_DataContextChanged;

                Grid.SetColumn(_boxS, 1);
                Grid.SetRow(_boxS, i);
                m_grid.Children.Add(_boxS);

                for (int j = 0; j < 100; j++)
                {
                    _boxS.Items.Add(j + 1);
                }

                //Episode number
                ComboBox _boxE = new ComboBox
                {
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    SelectedValue = m_data.Episode[i]
                };
                _boxE.DataContextChanged += BoxE_DataContextChanged;

                Grid.SetColumn(_boxE, 2);
                Grid.SetRow(_boxE, i);
                m_grid.Children.Add(_boxE);

                for (int j = 0; j < 100; j++)
                {
                    _boxE.Items.Add(j + 1);
                }

                //NoteBox
                TextBox _noteTextBlock = new TextBox()
                {
                    Foreground = Brushes.Yellow,
                    BorderBrush = Brushes.Cyan,
                    Background = Brushes.Gray,
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    Text = m_data.Note[i],
                };
                _noteTextBlock.TextChanged += NoteTextBlock_TextChanged;
                //TODO setcoolumn
                //TODO setrow
                //TODO add to m_grid children
                //TODO add to DrawContent()
            }
        }

        private void NoteTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BoxE_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show($"{e.NewValue}");
        }

        private void BoxS_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show($"{e.NewValue}");
        }

        private void OnTextChangeGrid(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show($"{sender}");
            if (m_data.Title.Contains(sender))
            {
                MessageBox.Show($"its here");
            }

            //MessageBox.Show(e.Source.ToString());
        }
    }
}