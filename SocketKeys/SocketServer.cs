using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NetSockets;
using System.Runtime.InteropServices;
using WindowsInput;
using System.IO;
using System.Diagnostics;

namespace SocketKeys

{

    class SocketServer
    {

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public int SERVERPORT = 8080;
        
        private Guid serverGuid = Guid.Empty;

        NetServer nS;

        public void Initialize()
        {
            StartListening();
        }


        public void StartListening()
        {
            if (nS != null && nS.IsOnline)
                nS.Stop();

            nS = new NetServer();
            nS.EchoMode = NetEchoMode.EchoAll;
            nS.OnClientConnected += nS_OnClientConnected;
            nS.OnClientDisconnected += nS_OnClientDisconnected;
            nS.OnReceived +=nS_OnReceived;

            try
            {
                nS.Start(IPAddress.Any, SERVERPORT);
            }
            catch (SocketException e)
            {
                MessageBox.Show("This port it's already in use, select a new one!", "Port already in use", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;

                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress item in ipHost.AddressList)
                {
                    if(!item.IsIPv6LinkLocal)
                        mw.appendConsole("Server IP: " + item.ToString() + ":" + SERVERPORT.ToString());    
                }
                
            }));
        }

        VirtualKeyCode getKey(string l){
            byte nKey = Convert.ToByte(l.ToUpper()[0]);
            VirtualKeyCode KEY = (VirtualKeyCode)nKey;
            return KEY;
        }

        bool usesText = false;

        private void nS_OnReceived(object sender, NetClientReceivedEventArgs<byte[]> e)
        {

            bool canConnect  =true;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                canConnect = (bool)mw.canConnect.IsChecked;
            }));

            if (canConnect)
            {
                String by = System.Text.Encoding.UTF8.GetString(e.Data);
                by = by.Replace("\n", "");
                bool isSQ = false;

                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\custom\\");
                string[] fl = new string[3];

                foreach (string item in files)
                {
                    fl = File.ReadAllText(item).Split('!');
                    if (fl[0] == by)
                        isSQ = true;
                }

                if (!isSQ)
                {
                    if (by.Contains("^"))
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.LCONTROL);

                    if (by.Contains("+"))
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.LSHIFT);

                    if (by.Contains("%"))
                        InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);

                    usesText = by.Contains("$");

                    by = by.Replace("^", "").Replace("+", "").Replace("%", "").Replace("$", "");

                    System.Windows.Forms.KeysConverter kc = new System.Windows.Forms.KeysConverter();
                    if (!usesText)
                    {
                        by = by.ToUpper();

                        bool isChar = false;

                        if (by == "TAB")
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                            isChar = true;
                        }
                        if (by == "ENTER")
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                            isChar = true;
                        }
                        if (by == "DEL")
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
                            isChar = true;
                        }
                        if (by == "CLICK"){
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LBUTTON);
                            isChar = true;
                        }
                        if (by == "CLICK2"){
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.RBUTTON);
                            isChar = true;
                        }
                        if (by == "SUPR"){
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.DELETE);
                            isChar = true;
                        }

                        if(!isChar)
                            InputSimulator.SimulateKeyPress(getKey(by[0].ToString()));

                        int isFKey = 0;
                        for (int i = 1; i < 13; i++)
                            if (by.Contains("F" + i.ToString()))
                                isFKey = i;

                        if (isFKey != 0)
                        {
                            VirtualKeyCode KEY = (VirtualKeyCode)Convert.ToByte(isFKey + 111);
                            InputSimulator.SimulateKeyPress(KEY);
                        }
                    }
                    else
                    {
                        InputSimulator.SimulateTextEntry(by);
                        
                    }

                    Thread.Sleep(100);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.LCONTROL);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.LSHIFT);
                    InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
                }
                else
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.Arguments = fl[2];
                    start.FileName = fl[1];
                    Process.Start(start);
                }
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    mw.appendConsole("[" + DateTime.Now.ToString() + "] " + by);
                }));
            }
        }

        void nS_OnClientDisconnected(object sender, NetClientDisconnectedEventArgs e)
        {
            //MessageBox.Show("Cellphone has been disconnected");
        }

        void nS_OnClientConnected(object sender, NetClientConnectedEventArgs e)
        {
        }

    }
}
