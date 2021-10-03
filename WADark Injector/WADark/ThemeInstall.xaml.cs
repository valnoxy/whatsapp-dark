/* 
 * WADark - The new dark mode for WhatsApp Desktop
 * Copyright (c) 2018 - 2021 Exploitox.
 * 
 * WADark is licensed under MIT License (https://github.com/valnoxy/wadark/blob/main/LICENSE). 
 * So you are allowed to use freely and modify the application. 
 * I will not be responsible for any outcome. 
 * Proceed with any action at your own risk.
 * 
 * Source code: https://github.com/valnoxy/wadark
 */

using static WADark.InstallScript;
using static WADark.Scripts.CheckSys;
using static WADark.Scripts.Backup;
using System;
using System.Net;
using System.Web;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace WADark
{
    /// <summary>
    /// Interaktionslogik für ThemeInstall.xaml
    /// </summary>
    public partial class ThemeInstall : Window
    {
        public string Author;
        public string ThemeName;
        public string ThemeDataJson;

        public ThemeInstall(string themePath)
        {
            InitializeComponent();
            Parser(themePath);
            NameString.Content = $"Theme Name: {ThemeName}";
            AuthorString.Content = $"Author: {Author}";
        }

        public void Parser(string themePath)
        {
            // Parse name
            Uri themeUri = new Uri(themePath);
            Author = themeUri.Host;
            string a = themeUri.LocalPath;
            ThemeName = a.Remove(0, 1);
            var encodedJsonString = HttpUtility.ParseQueryString(themeUri.Query).Get("themedata");
            ThemeDataJson = WebUtility.UrlDecode(encodedJsonString);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    // Disable elements and enable Progress bar
                    ProgressB.IsIndeterminate = true;
                    ProgressB.Visibility = Visibility.Visible;
                    CloseBtn.IsEnabled = false;
                    InstallBtn.IsEnabled = false;
                    StatusTxt.Visibility = Visibility.Visible;
                    StatusTxt.Content = "Status: Check system ...";
                });

                // System check
                int systemstatus = CheckSystem();
                if (systemstatus == 201)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Old version detected.";
                    });
                    return;
                }

                // Taskkill WhatsApp
                Dispatcher.Invoke(() =>
                {
                    StatusTxt.Visibility = Visibility.Visible;
                    StatusTxt.Content = "Status: Terminate WhatsApp ...";
                });
                foreach (var process in Process.GetProcessesByName("WhatsApp"))
                {
                    process.Kill();
                }
                Thread.Sleep(3000);

                // Backup
                Dispatcher.Invoke(() =>
                {
                    StatusTxt.Visibility = Visibility.Visible;
                    StatusTxt.Content = "Status: Backup asar file ...";
                });
                int backupstatus = BackupAsar(true);
                if (backupstatus == 301)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Cannot extract asar file.";
                    });
                    return;
                }
                if (backupstatus == 302)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Cannot create backup.";
                    });
                    return;
                }

                // Installation
                Dispatcher.Invoke(() =>
                {
                    StatusTxt.Visibility = Visibility.Visible;
                    StatusTxt.Content = "Status: Patch asar file ...";
                });
                int installstatus = ThemeInstallation(ThemeDataJson);
                if (installstatus == 101)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Cannot patch file.";
                    });
                    return;
                }
                if (installstatus == 102)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Cannot modify style.";
                    });
                    return;
                }
                if (installstatus == 103)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        ProgressB.Visibility = Visibility.Visible;
                        CloseBtn.IsEnabled = true;
                        InstallBtn.IsEnabled = false;
                        StatusTxt.Content = "Error: Cannot create file.";
                    });
                    return;
                }
                if (installstatus == 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusTxt.Visibility = Visibility.Visible;
                        StatusTxt.Content = "Status: Installation complete.";
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                    });
                    return;
                }
            });
        }
    }
}
