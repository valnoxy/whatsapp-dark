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

using System;
using System.IO;
using System.Windows;
using static WADark.InstallScript;
using static WADark.Scripts.CheckSys;
using static WADark.Scripts.Backup;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace WADark
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static string ThemeUrl = "https://dl.exploitox.de/whatsapp-dark/themes/1.json";
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Check if Node.js (npm) exist 
            // if (!File.Exists(Path.GetPathRoot(Environment.SystemDirectory) + "Program Files\\nodejs\\npx.cmd"))
            // {
            //     ErrorMessage eM = new ErrorMessage("node.js (npm) is not installed. Please install it first before running WADark!");
            //     eM.Show();
            // }

            // Check if switch is defined
            if (e.Args.Length > 0)
            {
                if (e.Args[0] == "/InstallTheme")
                {
                    ThemeInstall themeInstall = new ThemeInstall(e.Args[1]);
                    themeInstall.Show();
                }
                if (e.Args[0] == "/silent")
                {
                    // Silent Installation (mainly for winget)

                    // System check
                    int systemstatus = CheckSystem();
                    if (systemstatus == 201)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("A newer WhatsApp Desktop Version was detected, but it is currently not supported by WADark.");
                            eM.Show();
                        });
                        Environment.Exit(201);
                    }

                    // Kill WhatsApp processes
                    foreach (var process in Process.GetProcessesByName("WhatsApp"))
                    {
                        process.Kill();
                    }
                    Thread.Sleep(3000);

                    // Backup
                    int backupstatus = BackupAsar(true);
                    if (backupstatus == 301)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("WADark cannot extract the asar file. Please try again.");
                            eM.Show();
                        });
                        Environment.Exit(301);
                    }
                    if (backupstatus == 302)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("WADark cannot create a backup file. Please try again or (re)move all backups in \"%localappdata%\\WhatsApp\\app-X.XXXX.XX\\resources\".");
                            eM.Show();
                        });
                        Environment.Exit(302);
                    }

                    // Installation
                    int installstatus = ThemeInstallation(ThemeUrl);
                    if (installstatus == 101)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("WADark cannot patch the asar file. Please try again.");
                            eM.Show();
                        });
                        Environment.Exit(101);
                    }
                    if (installstatus == 102)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("WADark cannot modify the style files. Please try again.");
                            eM.Show();
                        });
                        Environment.Exit(102);
                    }
                    if (installstatus == 103)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ErrorMessage eM = new ErrorMessage("WADark cannot create a new asar file. Please try again.");
                            eM.Show();
                        });
                        Environment.Exit(103);
                    }
                    if (installstatus == 0)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                // Create Installation window
                Installation mainWindow = new Installation();
                mainWindow.Show();
            }
        }
    }
}
