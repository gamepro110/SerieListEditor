using System;
using System.Collections.Generic;
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
        public DisplayInfo()
        {
            InitializeComponent();
        }

        public DisplayInfo(OMDbApiNet.Model.Item _item)
        {
            InitializeComponent();
            ImgBlock.Source = new BitmapImage(new Uri(_item.Poster));
            TitleBlock.Text = _item.Title;
            DescriptionBlock.Document.Blocks.Clear();
            DescriptionBlock.Document.Blocks.Add(new Paragraph(new Run(_item.Plot)));
            GenInfo.Text = $"Type: {_item.Type}\tYear: {_item.Year}\tRated: {_item.Rated}";
        }
    }
}