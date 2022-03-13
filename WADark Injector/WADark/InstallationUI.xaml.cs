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
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace WADark
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class InstallationUI : Window
    {
        public string ThemeName;
        public string ThemeDataJson;
        public string WAVersion;

        public class VersionData
        {
            public string waversion { get; set; }
        }

        public InstallationUI()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(() => AssignData()));
        }

        private void AssignData()
        {
            var ver = _serialized_json_data<VersionData>(url);
            WAVersion = ver.waversion;

            int systemstatus = CheckSystem();
            if (systemstatus == 201)
            {
                NameString.Content = "This version is currently not supported by WADark.";
                InstallBtn.IsEnabled = false;
            } 
            else NameString.Content = $"WhatsApp Desktop Version: {WAVersion}";
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
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
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
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        StatusTxt.Content = "Error: Cannot extract asar file.";
                    });
                    return;
                }
                if (backupstatus == 302)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
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
                int installstatus = Installation("https://dl.exploitox.de/whatsapp-dark/themes/1.json");
                if (installstatus == 101)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        StatusTxt.Content = "Error: Cannot patch file.";
                    });
                    return;
                }
                if (installstatus == 102)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        StatusTxt.Content = "Error: Cannot modify style.";
                    });
                    return;
                }
                if (installstatus == 103)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CloseBtn.IsEnabled = true;
                        CloseBtn.Content = "Close";
                        CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                        InstallBtn.Visibility = Visibility.Hidden;
                        ProgressB.IsIndeterminate = false;
                        ProgressB.Value = 100;
                        StatusTxt.Content = "Error: Cannot create file.";
                    });
                    return;
                }
                if (installstatus == 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusTxt.Visibility = Visibility.Visible;
                        StatusTxt.Content = "Status: Copy to AppData ...";
                    });

                    // Install URL Protocol
                    var ApplicationPath = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "Exploitox", "WADark");
                    var dApplicationPath = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "Exploitox", "WADark").Replace(@"\", @"\\");
                    
                    // Copy application to AppData
                    try
                    {
                        Directory.CreateDirectory(ApplicationPath);
                        File.Copy(Process.GetCurrentProcess().MainModule.FileName, Path.Combine(ApplicationPath, "WADark.exe"), true);
                    }
                    catch
                    {
                        Dispatcher.Invoke(() =>
                        {
                            StatusTxt.Visibility = Visibility.Visible;
                            StatusTxt.Content = "Error: Cannot copy to AppData.";
                            CloseBtn.IsEnabled = true;
                            CloseBtn.Content = "Close";
                            CloseBtn.Margin = new Thickness(356, 139, 0, 0);
                            InstallBtn.Visibility = Visibility.Hidden;
                            ProgressB.IsIndeterminate = false;
                            ProgressB.Value = 100;
                            return;
                        });
                    }

                    Dispatcher.Invoke(() =>
                    {
                        StatusTxt.Visibility = Visibility.Visible;
                        StatusTxt.Content = "Status: Register URL Protocol ...";
                    });
                    RegistryKey key = Registry.ClassesRoot.OpenSubKey("wadark"); 
                    if (key == null)  // if the protocol is not registered yet... we register it
                    {
                        key = Registry.ClassesRoot.CreateSubKey("wadark");
                        key.SetValue(string.Empty, "URL:wadark Protocol");
                        key.SetValue("URL Protocol", string.Empty);

                        key = key.CreateSubKey(@"shell\open\command");
                        key.SetValue(string.Empty, dApplicationPath + "\\WADark.exe" + " /InstallTheme " + "%1");
                    }
                    key.Close();

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

        private static T _serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }
    }
}
