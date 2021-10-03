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
    public class Backup
    {
        public static int BackupAsar(bool OverwriteBkg)
        {
            var ver = _serialized_json_data<InstallScript.VersionData>(InstallScript.url);
            string waVer = "app-" + ver.waversion;

            var waAsar = "";
            var waAsarBkg = "";
            var tempDir = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
                waAsarBkg = Path.Combine(Environment.GetFolderPath(
                     Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar.bkg");
                tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
                Directory.CreateDirectory(tempDir);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
                waAsarBkg = "/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark/app.asar.bkg";
                tempDir = "/tmp/Exploitox/WADark/asar_extract";
            }

            // Check if backup already exist
            if (File.Exists(waAsarBkg))
            {
                bool overwrite = false;
                bool confirmed = false;

                if (OverwriteBkg)
                {
                    overwrite = true;
                    confirmed = true;
                }

                while (!confirmed)
                {
                    Console.Write("[!] A backup file already exist. Overwrite? [Y/N]: ");
                    string option = Console.ReadLine();

                    if (option == "Y" || option == "y")
                    {
                        overwrite = true;
                        confirmed = true;
                    }
                    if (option == "N" || option == "n")
                    {
                        overwrite = false;
                        confirmed = true;
                    }
                }

                if (overwrite)
                    File.Delete(waAsarBkg);
                if (!overwrite)
                    Environment.Exit(1);
            }

            // Extract asar file with npx
            try
            {
                Process p = new Process();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    p.StartInfo.FileName = Path.GetPathRoot(Environment.SystemDirectory) + "Program Files\\nodejs\\npx.cmd";
                    p.StartInfo.Arguments = "npx asar extract " + waAsar + " " + tempDir;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = "-c \"npx asar extract " + waAsar + " " + tempDir + "\"";
                }
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot extract asar file.");
                System.Threading.Thread.Sleep(3000);
                return 301;
            }

            try
            {
                File.Move(waAsar, waAsarBkg);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot create backup.");
                System.Threading.Thread.Sleep(3000);
                return 302;
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
