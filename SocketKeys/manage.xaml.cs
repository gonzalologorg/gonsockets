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
    /// Interaction logic for manage.xaml
    /// </summary>
    public partial class manage : Window
    {
        public manage()
        {
            InitializeComponent();
            populateListView();
        }

        public void populateListView()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\");

            foreach (string item in files)
            {
                listView.Items.Add(item.Replace(".skt", "").Replace(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\",""));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\" + listView.SelectedItem + ".skt");
                listView.Items.Remove(listView.SelectedItem);
            }
        }

        private void ButtonB_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\" + listView.SelectedItem + ".skt"))
            {
                string fil = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\" + listView.SelectedItem + ".skt");
                string[] fil_mod = fil.Split('!');
                MessageBox.Show("Wildcard: " + fil_mod[0] + "\nShell command: " + fil_mod[1] + "\n" + "Parameters: " + fil_mod[2]);
            }
        }

        private void ButtonC_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
