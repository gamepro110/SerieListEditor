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

        public DisplayInfo(string source)
        {
            InitializeComponent();

            string responce = GetWebPage(source);

            JArray jsonData = JArray.Parse(responce);

            MessageBox.Show(jsonData[0].ToString());

            //Uri titleImg = new Uri("https://upload.wikimedia.org/wikipedia/commons/3/30/Googlelogo.png");
            //ImgBlock.Source = new BitmapImage(titleImg);
            //TitleBlock.Text = "no title yet";
            //DescriptionBlock.Text = "no description yet";
        }

        //display movie api from
        private string GetWebPage(string address)
        {
            string responseText;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    responseText = responseStream.ReadToEnd();
                }
            }

            return responseText;
        }
    }
}