using System;
using System.IO;
using System.Windows;
using EFTLauncher.ClientLogic;
using EFTLauncher.ServerLogic;
using EFTLauncher.Utility;

namespace EFTLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;
        Server server;

        public MainWindow()
        {
            // Initialize window
            InitializeComponent();
        }

        private void StartClientButtonClicked(object sender, RoutedEventArgs e)
        {
            // check server status
            if (client != null)
            {
                return;
            }

            // start server
            client = new Client();
            client.Start(emailText.Text, passwordText.Text);
        }

        private void StartServerButtonClicked(object sender, RoutedEventArgs e)
        {
            // check server status
            if (server != null)
            {
                return;
            }
                
            // start server
            server = new Server();
            server.Start(domainText.Text, System.Convert.ToInt32(portText.Text));
        }

        private void StopServerButtonClicked(object sender, RoutedEventArgs e)
        {
            // check server status
            if (server == null)
            {
                return;
            }

            // stop server
            server.Stop();
            server = null;
        }

        private void DeleteLogsButtonClicked(object sender, RoutedEventArgs e)
        {
            // log status
            Logger.Log("ALERT: Deleting all log files");

            // delete all files and directories in <serverdir>/logs
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + @"/logs/");

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}