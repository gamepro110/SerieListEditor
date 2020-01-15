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
using Newtonsoft.Json.Linq;

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

        public DisplayInfo(Uri source)
        {
            InitializeComponent();

            MessageBox.Show($"");
            //JArray jsonData = JArray.Parse($"{www.GetResponse()}");

            //Uri titleImg = new Uri("https://upload.wikimedia.org/wikipedia/commons/3/30/Googlelogo.png");
            //ImgBlock.Source = new BitmapImage(titleImg);
            //TitleBlock.Text = "no title yet";
            //DescriptionBlock.Text = "no description yet";
        }

        //display movie api from
    }
}