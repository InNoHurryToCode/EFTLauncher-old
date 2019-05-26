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
            server.Start();
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
