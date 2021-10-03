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

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WADark.Scripts
{
    internal class CheckSys
    {
        public static int CheckSystem()
        {
            var tempNodeJS = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "nodejs.msi");
            var tempHomebrew = "/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark/Homebrew.sh";

            // Check WhatsApp version
            var ver = _serialized_json_data<InstallScript.VersionData>(InstallScript.url);
            string waVer = "app-" + ver.waversion;

            var waDir = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                waDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waDir = "/Applications/WhatsApp.app/Contents/Resources";
            }

            if (!Directory.Exists(waDir))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    --> WhatsApp Version {0} not found!\n        " +
                    "WADark cannot continue patching. Please wait until a new version of WADark was published.", ver.waversion);
                System.Threading.Thread.Sleep(3000);
                return 201;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    --> WhatsApp Version {0} found.", ver.waversion);
            }

            // Check if node.js is installed
            // If platform is Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Create temp directory
                string tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
                Directory.CreateDirectory(tempDir);

                if (!File.Exists(Path.GetPathRoot(Environment.SystemDirectory) + "Program Files\\nodejs\\npx.cmd"))
                {
                    Console.WriteLine("    --> node.js (npm) not found. Downloading ...");
                    using (var w = new WebClient())
                    {
                        w.DownloadFile(ver.node_win, tempNodeJS);
                    }
                    Console.WriteLine("    --> Installing node.js ...");
                    Process p = new Process();
                    p.StartInfo.FileName = "msiexec.exe";
                    p.StartInfo.Arguments = "/i " + tempNodeJS + " /quiet /qn";
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    p.WaitForExit();
                }
            }

            // If platform is OSX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // Create temp directory
                string tempDir = "/tmp/Exploitox/WADark/asar_extract";
                Directory.CreateDirectory(tempDir);

                Directory.CreateDirectory("/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark");
                if (!File.Exists("/usr/local/bin/npm"))
                {
                    Console.WriteLine("    --> node.js (npm) not found. Downloading ...");
                    if (!File.Exists("/usr/local/homebrew"))
                    {
                        Console.WriteLine("    --> Installing Homebrew ...");
                        using (var w = new WebClient())
                        {
                            w.DownloadFile("https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh", tempHomebrew);
                        }
                        Process pHB = new Process();
                        pHB.StartInfo.FileName = "/bin/bash";
                        pHB.StartInfo.Arguments = tempHomebrew;
                        pHB.Start();
                        pHB.WaitForExit();
                    }
                    Console.WriteLine("    --> Installing node.js ...");
                    Process p = new Process();
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = "-c \"brew install node\"";
                    p.Start();
                    p.WaitForExit();
                }
            }
            return 0;
        }
        private static T _serialized_json_data<T>(string url) where T : new()
        {
            if (url.Equals("theme.json"))
            {
                var json_data = string.Empty;
                try
                {
                    json_data = File.ReadAllText("theme.json");
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
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
