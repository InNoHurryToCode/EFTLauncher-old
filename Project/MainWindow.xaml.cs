using System.Windows;

namespace EFTServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize window
            InitializeComponent();

            // Initialize server
            Server server = new Server();
        }
    }
}
