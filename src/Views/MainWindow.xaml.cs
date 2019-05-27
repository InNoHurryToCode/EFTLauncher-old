using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
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
        DispatcherTimer logUpdateTimer;
        LauncherSettings launcherSettings;

        public MainWindow()
        {
            // Initialize window
            InitializeComponent();
            InitializeLogUpdater();
            LoadSettings();
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
            client.Start(email.Text, password.Text, gameLocation.Text, gameDomain.Text);
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
            server.Start(serverDomain.Text);
        }

        private void DeleteLogsButtonClicked(object sender, RoutedEventArgs e)
        {
            Logger.DeleteAllLogs();
        }

        private void InitializeLogUpdater()
        {
            logUpdateTimer = new DispatcherTimer();
            logUpdateTimer.Interval = new TimeSpan(0, 0, 1);
            logUpdateTimer.Tick += new EventHandler(OnLogUpdate);
            logUpdateTimer.Start();
        }

        private void OnLogUpdate(Object source, EventArgs e)
        {
            // set log textbox to logged text
            logText.Text = Logger.log;
        }

        private void LoadSettings()
        {
            // load settings
            Logger.Log("INFO: Loading launcher settings");
            launcherSettings = JsonHelper.LoadJson<LauncherSettings>(Environment.CurrentDirectory + "/settings.json");

            // apply settings
            email.Text = launcherSettings.email;
            password.Text = launcherSettings.password;
            serverDomain.Text = launcherSettings.serverDomain;
            gameLocation.Text = launcherSettings.gameLocation;
            gameDomain.Text = launcherSettings.gameDomain;
        }

        private void SaveSettings()
        {
            // save settings
            Logger.Log("INFO: Saving launcher settings");
            JsonHelper.SaveJson<LauncherSettings>(Environment.CurrentDirectory + "/settings.json", launcherSettings);
        }

        private void EmailChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            launcherSettings.email = email.Text;
            SaveSettings();
        }

        private void PasswordChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            launcherSettings.password = password.Text;
            SaveSettings();
        }

        private void ServerDomainChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            launcherSettings.serverDomain = serverDomain.Text;
            SaveSettings();
        }

        private void GameLocationChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            launcherSettings.gameLocation = gameLocation.Text;
            SaveSettings();
        }

        private void GameDomainChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            launcherSettings.gameDomain = gameDomain.Text;
            SaveSettings();
        }
    }
}