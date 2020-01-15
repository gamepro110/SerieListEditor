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

        private string OldText = "";
        private int? oldNum = null;
        private string lastFocusedTitle = "";

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
            BaseGrid.Children.Remove(m_grid);
            m_grid = new Grid
            {
                Margin = new Thickness(10, 50, 10, 90),
                Background = Brushes.Gray, //debugging size
            };
            BaseGrid.Children.Add(m_grid);
        }

        internal void DrawContent()
        {
            //amount of columns
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// title
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// season
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// episode
            //add one here for the notes

            //amount of rows for the seasons and
            for (int i = 0; i < m_data.Title.Count; i++)
            {
                m_grid.RowDefinitions.Add(new RowDefinition());// new row for every title

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

                Grid.SetColumn(_titleTextBlock, 0);
                Grid.SetRow(_titleTextBlock, i);
                m_grid.Children.Add(_titleTextBlock);

                _titleTextBlock.GotFocus += TitleTextBlock_GotFocus;
                _titleTextBlock.TextChanged += TitleTextBlock_TextChanged;

                //Season
                ComboBox _boxSeason = new ComboBox
                {
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    SelectedValue = m_data.Season[i],
                };

                for (int j = 0; j < 100; j++)
                {
                    _boxSeason.Items.Add(j + 1);
                }

                Grid.SetColumn(_boxSeason, 1);
                Grid.SetRow(_boxSeason, i);
                m_grid.Children.Add(_boxSeason);

                _boxSeason.GotFocus += BoxSeason_GotFocus;
                _boxSeason.SelectionChanged += BoxSeason_DataContextChanged;

                //Episode number
                ComboBox _boxEpisode = new ComboBox
                {
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    SelectedValue = m_data.Episode[i]
                };

                for (int j = 0; j <= 100; j++)
                {
                    _boxEpisode.Items.Add(j);
                }

                Grid.SetColumn(_boxEpisode, 2);
                Grid.SetRow(_boxEpisode, i);
                m_grid.Children.Add(_boxEpisode);

                _boxEpisode.GotFocus += BoxEpisode_GotFocus;
                _boxEpisode.SelectionChanged += BoxEpisode_DataContextChanged;

                //NoteBox
                TextBox _noteTextBlock = new TextBox()
                {
                    Foreground = Brushes.Yellow,
                    BorderBrush = Brushes.Cyan,
                    Background = Brushes.Gray,
                    Height = m_boxHeight,
                    Width = m_boxWidth,
                    Text = (m_data.Note.Count == m_data.Title.Count) ? m_data.Note[i] : "Empty Note",
                };

                if (m_data.Note.Count == m_data.Title.Count)
                {
                    _noteTextBlock.Text = m_data.Note[i];
                }
                else
                {
                    m_data.Note.Add("Empty Note");
                    _noteTextBlock.Text = "Empty Note";
                }
                _noteTextBlock.TextChanged += NoteTextBlock_TextChanged;
                _noteTextBlock.GotFocus += NoteTextBlock_GotFocus;

                //TODO setcoolumn
                //TODO setrow
                //TODO add to m_grid children
                //TODO Rearange the components so everything is visable
            }
        }

        #region combo box Season and Episode

        // Episode combo box
        private void BoxEpisode_GotFocus(object sender, RoutedEventArgs e)
        {
            var _num = e.Source as ComboBox;
            oldNum = _num.SelectedIndex;

            lastFocusedTitle = m_data.Title[m_data.Episode.IndexOf(oldNum)];
        }

        private void BoxEpisode_DataContextChanged(object sender, SelectionChangedEventArgs e)
        {
            int _index = m_data.Season.IndexOf(oldNum);
            var _num = e.Source as ComboBox;
            m_data.Season[_index] = _num.SelectedIndex + 1;
        }

        // Season combo box
        private void BoxSeason_GotFocus(object sender, RoutedEventArgs e)
        {
            var _num = e.Source as ComboBox;
            oldNum = _num.SelectedIndex + 1;

            lastFocusedTitle = m_data.Title[m_data.Season.IndexOf(oldNum)];
        }

        private void BoxSeason_DataContextChanged(object sender, SelectionChangedEventArgs e)
        {
            int _index = m_data.Season.IndexOf(oldNum);
            var _num = e.Source as ComboBox;
            m_data.Season[_index] = _num.SelectedIndex + 1;

            //TODO indexof()
        }

        #endregion combo box Season and Episode

        // Note text block
        private void NoteTextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBlock;
            m_data.Note[m_data.Note.IndexOf(OldText)] = _text.Text;

            lastFocusedTitle = m_data.Title[m_data.Note.IndexOf(_text.Text)];
        }

        private void NoteTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _text = e.OriginalSource as TextBlock;
            m_data.Note[m_data.Note.IndexOf(OldText)] = _text.Text;
        }

        //Title text block
        private void TitleTextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            OldText = _text.Text;

            lastFocusedTitle = m_data.Title[m_data.Title.IndexOf(_text.Text)];
        }

        private void TitleTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            m_data.Title[m_data.Title.IndexOf(OldText)] = _text.Text;
        }

        //https://stackoverflow.com/questions/27311082/how-to-get-old-text-and-changed-text-of-textbox-on-textchanged-event-of-textbox
    }
}