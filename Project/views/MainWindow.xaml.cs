using System.Windows;

namespace EFTServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server;  // game server

        public MainWindow()
        {
            // Initialize window
            InitializeComponent();
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            // check server status
            if (server != null)
            {
                return;
            }
                
            // start server
            server = new Server();
            server.Start(emailText.Text, passwordText.Text, domainText.Text, System.Convert.ToInt32(portText.Text));
        }

        private void StopButtonClicked(object sender, RoutedEventArgs e)
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
    }
}