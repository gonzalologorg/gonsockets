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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;

namespace SocketKeys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SocketServer sck;
        TextBox tv;
        bool mustClose = false;

        public MainWindow()
        {
            InitializeComponent();
            sck = new SocketServer();
            appendConsole("Console initialized");
            sck.Initialize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mustClose = true;
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public void appendConsole(String txt)
        {
            tv = (TextBox)this.FindName("console");
            tv.AppendText(txt+"\n");
            tv.ScrollToEnd();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            cusomControls cC = new cusomControls();
            cC.Show();
        }

        private TaskbarIcon notifyIcon;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!mustClose)
            {
                notifyIcon = new TaskbarIcon();
                notifyIcon.Icon = Properties.Resources.icon;
                notifyIcon.ToolTipText = "Left-click to open server";
                notifyIcon.Visibility = Visibility.Visible;
                notifyIcon.TrayLeftMouseDown += notifyIcon_TrayLeftMouseDown;
                e.Cancel = true;
                Hide();
            }
        }

        private void notifyIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            if (notifyIcon != null)
                notifyIcon.Visibility = Visibility.Hidden;

            Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //the notify icon only closes automatically on WPF applications
            //-> dispose the notify icon manually
            notifyIcon.Dispose();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(port.Text) != null)
            {
                appendConsole("Listening at " + port.Text);
                sck.SERVERPORT = Convert.ToInt32(port.Text);
                sck.StartListening();
            }
            else
                MessageBox.Show("Port must be a valid number!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
        }

    }
}
