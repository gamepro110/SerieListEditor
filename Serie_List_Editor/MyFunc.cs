using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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

        private string m_oldText = "";
        private string m_oldNoteText = "";
        private int m_oldNum = 1;
        private string m_lastFocusedTitle = "";

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

        private void UpdateUI()
        {
            MakeGrid();
            DrawContent();
        }

        internal void MakeGrid()
        {
            int _rows = m_data.Title != null ? (m_data.Title.Count > 0 ? m_data.Title.Count : 1) : 1;
            int _lengthFromBotom = 140;
            int _rowSize = 35;
            m_grid.Height = _rows * _rowSize < BaseGrid.ActualHeight - _lengthFromBotom ? _rows * _rowSize : BaseGrid.ActualHeight - _lengthFromBotom >= 0 ? BaseGrid.ActualHeight - _lengthFromBotom : 0;
        }

        internal void DrawContent()
        {
            //clears the columns and rows
            m_grid.ColumnDefinitions.Clear();
            m_grid.RowDefinitions.Clear();

            //amount of columns
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// title
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// season
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// episode
            m_grid.ColumnDefinitions.Add(new ColumnDefinition());// notes
            //add one here for the notes

            if (m_data.Title != null)
            {
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
                    _titleTextBlock.LostFocus += TitleTextBlock_LostFocus;

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

                    for (int j = 0; j < 100; j++)
                    {
                        _boxEpisode.Items.Add(j + 1);
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
                    _noteTextBlock.GotFocus += NoteTextBlock_GotFocus;
                    _noteTextBlock.LostFocus += NoteTextBlock_LostFocus;

                    Grid.SetColumn(_noteTextBlock, 3);
                    Grid.SetRow(_noteTextBlock, i);
                    m_grid.Children.Add(_noteTextBlock);

                    m_lastFocusedTitle = m_data.Title[0];
                }
            }
        }

        #region combo box Season and Episode

        // Episode combo box
        private void BoxEpisode_GotFocus(object sender, RoutedEventArgs e)
        {
            var _num = e.Source as ComboBox;
            m_oldNum = _num.SelectedIndex + 1;

            int index = m_data.Episode.IndexOf(m_oldNum);

            m_lastFocusedTitle = m_data.Title[index];
        }

        private void BoxEpisode_DataContextChanged(object sender, SelectionChangedEventArgs e)
        {
            int _index = m_data.Episode.IndexOf(m_oldNum);
            var _num = e.Source as ComboBox;
            m_data.Episode[_index] = _num.SelectedIndex + 1;
        }

        // Season combo box
        private void BoxSeason_GotFocus(object sender, RoutedEventArgs e)
        {
            var _num = e.Source as ComboBox;
            m_oldNum = _num.SelectedIndex + 1;

            int index = m_data.Season.IndexOf(m_oldNum);
            m_lastFocusedTitle = m_data.Title[index];
        }

        private void BoxSeason_DataContextChanged(object sender, SelectionChangedEventArgs e)
        {
            int _index = m_data.Season.IndexOf(m_oldNum);
            var _num = e.Source as ComboBox;
            m_data.Season[_index] = _num.SelectedIndex + 1;
        }

        #endregion combo box Season and Episode

        #region TextBlocks Title and Note

        // Note text block
        private void NoteTextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            m_oldNoteText = _text.Text;

            m_lastFocusedTitle = m_data.Title[m_data.Note.IndexOf(_text.Text)];
        }

        private void NoteTextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            m_data.Note[m_data.Note.IndexOf(m_oldNoteText)] = _text.Text;

            m_lastFocusedTitle = m_data.Title[m_data.Note.IndexOf(m_oldNoteText)];
        }

        //Title text block
        private void TitleTextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            m_oldText = _text.Text;

            m_lastFocusedTitle = m_data.Title[m_data.Title.IndexOf(_text.Text)];
        }

        private void TitleTextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            var _text = e.OriginalSource as TextBox;
            m_data.Title[m_data.Title.IndexOf(m_oldText)] = _text.Text;

            m_lastFocusedTitle = m_data.Title[m_data.Title.IndexOf(_text.Text)];
        }

        #endregion TextBlocks Title and Note

        private bool IsNetworkAvailable
        {
            get
            {
                Ping _ping = new Ping();
                PingReply _reply = _ping.Send("www.omdbapi.com");

                switch (_reply.Status)
                {
                    case IPStatus.Success:
                        {
                            return true;
                        }
                    case IPStatus.DestinationNetworkUnreachable:
                    case IPStatus.DestinationHostUnreachable:
                    case IPStatus.DestinationProtocolUnreachable:
                    case IPStatus.DestinationPortUnreachable:
                    case IPStatus.NoResources:
                    case IPStatus.BadOption:
                    case IPStatus.HardwareError:
                    case IPStatus.PacketTooBig:
                    case IPStatus.TimedOut:
                    case IPStatus.BadRoute:
                    case IPStatus.TtlExpired:
                    case IPStatus.TtlReassemblyTimeExceeded:
                    case IPStatus.ParameterProblem:
                    case IPStatus.BadDestination:
                    case IPStatus.DestinationUnreachable:
                    case IPStatus.TimeExceeded:
                    case IPStatus.BadHeader:
                    case IPStatus.UnrecognizedNextHeader:
                    case IPStatus.IcmpError:
                    case IPStatus.DestinationScopeMismatch:
                    case IPStatus.Unknown:
                    default:
                        {
                            return false;
                        }
                }
            }
        }
    }
}