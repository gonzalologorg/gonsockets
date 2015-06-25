using System;
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
using System.Windows.Shapes;
using System.IO;

namespace SocketKeys
{
    /// <summary>
    /// Interaction logic for cusomControls.xaml
    /// </summary>
    public partial class cusomControls : Window
    {
        public cusomControls()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String data = wildcard.Text+"!"+shellrun.Text+"!"+parameters.Text;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\custom"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\custom");

            File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\" + wildcard.Text + ".skt").Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\"+wildcard.Text+".skt", data,Encoding.UTF8);
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            manage mng = new manage();
            mng.Show();
        }

    }
}
