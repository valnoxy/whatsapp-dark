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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace WADark
{
    class Program
    {
        public class VersionData
        {
            public string waversion { get; set; }
            public string node_win { get; set; }
            public string css_filename1 { get; set; }
            public string css_dl1 { get; set; }
            public string css_filename2 { get; set; }
            public string css_dl2 { get; set; }
            public string index_filename { get; set; }
            public string index_dl { get; set; }
        }
        public static string url = "https://dl.exploitox.de/whatsapp-dark/version.json";

        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            Environment.Exit(1);
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n ██╗    ██╗ █████╗ ██████╗  █████╗ ██████╗ ██╗  ██╗\n" +
                " ██║    ██║██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝\n" +
                " ██║ █╗ ██║███████║██║  ██║███████║██████╔╝█████╔╝ \n" +
                " ██║███╗██║██╔══██║██║  ██║██╔══██║██╔══██╗██╔═██╗ \n" +
                " ╚███╔███╔╝██║  ██║██████╔╝██║  ██║██║  ██║██║  ██╗\n" +
                "  ╚══╝╚══╝ ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝");

            try
            {
                var ver = _download_serialized_json_data<VersionData>(url);

                Console.WriteLine("WADark [Version: {0} (Beta Version)]", ver.waversion);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            Console.WriteLine("(c) 2018 - 2021 Exploitox - Unleash your exploits!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose an option:");
            Console.WriteLine("   1) Install WADark");
            Console.WriteLine("   2) Remove WADark and restore backup");
            Console.WriteLine("   ----------------------------------- ");
            Console.WriteLine("   3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Install();
                    return false;

                case "2":
                    Uninstall();
                    return false;

                case "3":
                    Environment.Exit(0);
                    return false;

                default:
                    return true;
            }
        }

        static void Install()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n ██╗    ██╗ █████╗ ██████╗  █████╗ ██████╗ ██╗  ██╗\n" +
                " ██║    ██║██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝\n" +
                " ██║ █╗ ██║███████║██║  ██║███████║██████╔╝█████╔╝ \n" +
                " ██║███╗██║██╔══██║██║  ██║██╔══██║██╔══██╗██╔═██╗ \n" +
                " ╚███╔███╔╝██║  ██║██████╔╝██║  ██║██║  ██║██║  ██╗\n" +
                "  ╚══╝╚══╝ ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝");

            try
            {
                var ver = _download_serialized_json_data<VersionData>(url);
                Console.WriteLine("WADark [Version: {0} (Beta Version)]\n", ver.waversion);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            // System check
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine("[i] Check system ...");
            CheckSys();

            // Kill WhatsApp process if running
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Terminate WhatsApp process if running ...");
            foreach (var process in Process.GetProcessesByName("WhatsApp"))
            {
                process.Kill();
            }
            System.Threading.Thread.Sleep(3000);

            // Backup old app.asar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Create backup ...");
            BackupAsar();

            // Apply new app.asar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Patch app.asar ...");
            PatchAsar();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] WADark was successfully installed!");
            System.Threading.Thread.Sleep(5000);
            Environment.Exit(0);
        }

        static void Uninstall()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n ██╗    ██╗ █████╗ ██████╗  █████╗ ██████╗ ██╗  ██╗\n" +
                " ██║    ██║██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝\n" +
                " ██║ █╗ ██║███████║██║  ██║███████║██████╔╝█████╔╝ \n" +
                " ██║███╗██║██╔══██║██║  ██║██╔══██║██╔══██╗██╔═██╗ \n" +
                " ╚███╔███╔╝██║  ██║██████╔╝██║  ██║██║  ██║██║  ██╗\n" +
                "  ╚══╝╚══╝ ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝");

            try
            {
                var ver = _download_serialized_json_data<VersionData>(url);

                Console.WriteLine("WADark [Version: {0} (Beta Version)]\n", ver.waversion);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            // System check
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Check system ...");
            CheckSys();

            // Kill WhatsApp process if running
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Terminate WhatsApp process if running ...");
            foreach (var process in Process.GetProcessesByName("WhatsApp"))
            {
                process.Kill();
            }
            System.Threading.Thread.Sleep(3000);

            // Remove new app.asar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Create backup ...");
            RemoveAsar();

            // Apply old app.asar
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[i] Apply new app.asar ...");
            RestoreAsar();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[OK] WADark was successfully installed!");
            System.Threading.Thread.Sleep(5000);
            Environment.Exit(0);
        }

        private static T _download_serialized_json_data<T>(string url) where T : new()
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

        private static void CheckSys()
        {
            var tempNodeJS = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "nodejs.msi");
            var tempHomebrew = "/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark/Homebrew.sh";

            // Check WhatsApp version
            var ver = _download_serialized_json_data<VersionData>(url);
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
                Environment.Exit(1);
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
                    p.StartInfo.Arguments = "/qn /l* " + tempDir + "\\nodejs_install.log /i " + tempNodeJS;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.UseShellExecute = true;
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
        }

        private static void BackupAsar()
        {
            var ver = _download_serialized_json_data<VersionData>(url);
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
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot extract asar file.");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
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
                Environment.Exit(1);
            }
        }

        private static void PatchAsar()
        {
            var ver = _download_serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var tempDir = "";
            var waAsar = "";
            var tempCSS1 = "";
            var tempCSS2 = "";
            var tempIndex = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
                tempCSS1 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename1);
                tempCSS2 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename2);
                tempIndex = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.index_filename);

            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
                tempDir = "/tmp/Exploitox/WADark/asar_extract/";

                tempCSS1 = tempDir + ver.css_filename1;
                tempCSS2 = tempDir + ver.css_filename2;
                tempIndex = tempDir + ver.index_filename;
            }

            // Patch extracted files
            try
            {
                // Delete old files
                if (File.Exists(tempCSS1)) { File.Delete(tempCSS1); }
                if (File.Exists(tempCSS2)) { File.Delete(tempCSS2); }
                if (File.Exists(tempIndex)) { File.Delete(tempIndex); }

                // Download patched files
                using (var w = new WebClient())
                {
                    w.DownloadFile(ver.css_dl1, tempCSS1);
                    w.DownloadFile(ver.css_dl2, tempCSS2);
                    w.DownloadFile(ver.index_dl, tempIndex);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot patch asar file: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            // Create new asar file with npx
            try
            {
                Process p = new Process();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    p.StartInfo.FileName = Path.GetPathRoot(Environment.SystemDirectory) + "Program Files\\nodejs\\npx.cmd";
                    p.StartInfo.Arguments = "npx asar pack " + tempDir + " " + waAsar;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    p.StartInfo.FileName = "/bin/bash"; 
                    p.StartInfo.Arguments = "-c \"npx asar pack " + tempDir + " " + waAsar + "\"";
                }
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot create new asar file.");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }

        private static void RemoveAsar()
        {
            var ver = _download_serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var waAsar = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
            }
            try
            {
                File.Delete(waAsar);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot remove new asar file.");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }

        private static void RestoreAsar()
        {
            var ver = _download_serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var waAsar = "";
            var waAsarBkg = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
                waAsarBkg = Path.Combine(Environment.GetFolderPath(
                     Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar.bkg");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
                waAsarBkg = "/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark/app.asar.bkg";
            }
            try
            {
                File.Move(waAsarBkg, waAsar);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot restore old asar file.");
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }
    }
}
