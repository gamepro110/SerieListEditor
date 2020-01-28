using OMDbApiNet.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using OMDbApiNet;
using OMDbApiNet.Model;
using OMDbApiNet.Utilities;

namespace Serie_List_Editor
{
    /// <summary>
    /// Interaction logic for DisplayInfo.xaml
    /// </summary>
    public partial class DisplayInfo : Window
    {
        private readonly Item m_item;

        public DisplayInfo()
        {
            InitializeComponent();
        }

        public DisplayInfo(OMDbApiNet.Model.Item _item)
        {
            m_item = _item;
            InitializeComponent();

            ImgBlock.Source = new BitmapImage(new Uri(m_item.Poster));

            TitleBlock.Text = m_item.Title;

            DescriptionBlock.Document.Blocks.Clear();
            DescriptionBlock.Document.Blocks.Add(new Paragraph(new Run(m_item.Plot)));

            GenInfo.Text = $"Type: {_item.Type}\tYear: {_item.Year}\tRated: {m_item.Rated}";

            SerieInfoBlock.Text = $"Total seasons: {m_item.TotalSeasons}\n" +
                                  $"IMBD Rating: {m_item.ImdbRating}";
        }

        private void Open_link_Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo info;

            if (m_item != null && m_item.ImdbId != "") info = new ProcessStartInfo($"https://www.imdb.com/title/{m_item.ImdbId}/");
            else info = new ProcessStartInfo($"https://www.imdb.com/");

            Process.Start(info);
        }
    }
}