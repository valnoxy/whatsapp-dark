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
using Newtonsoft.Json;

namespace WADark
{
    class Program
    {
        public class VersionData
        {
            public string waversion { get; set; }
            public string node_win { get; set; }
            public string node_mac { get; set; }
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

                Console.WriteLine("WADark [Version: {0} (Alpha Version)]", ver.waversion);
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
                Console.WriteLine("WADark [Version: {0} (Alpha Version)]\n", ver.waversion);
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

                Console.WriteLine("WADark [Version: {0} (Alpha Version)]\n", ver.waversion);
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
            // Check WhatsApp version
            var ver = _download_serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var waDir = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer);

            // Check if node.js is installed
            var tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark");
            var tempNodeJS = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "nodejs.msi");
            Directory.CreateDirectory(tempDir);
            if (!File.Exists("C:\\Program Files\\nodejs\\npx.cmd"))
            {
                Console.WriteLine("    --> node.js not found. Downloading ...");
                using (var w = new WebClient())
                {
                    w.DownloadFile(ver.node_win, tempNodeJS);
                }
                Console.WriteLine("    --> Installing silent ...");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "msiexec.exe";
                p.StartInfo.Arguments = "/qn /l* " + tempDir + "\\nodejs_install.log /i " + tempNodeJS;
                p.StartInfo.Verb = "runas";
                p.StartInfo.UseShellExecute = true;
                p.Start();
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: An error has occurred: " + p.ExitCode);
                    System.Threading.Thread.Sleep(5000);
                    Environment.Exit(1);
                }
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
        }

        private static void BackupAsar()
        {
            var ver = _download_serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var waAsar = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
            var waAsarBkg = Path.Combine(Environment.GetFolderPath(
                 Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar.bkg");
            var tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");

            // Extract asar file with npx
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\Program Files\\nodejs\\npx.cmd";
                p.StartInfo.Arguments = "npx asar extract " + waAsar + " " + tempDir;
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: An error has occurred: " + p.ExitCode);
                    System.Threading.Thread.Sleep(5000);
                    Environment.Exit(1);
                }
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
            var tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
            var tempNodeJS = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "nodejs.msi");
            string waVer = "app-" + ver.waversion;
            var waAsar = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
            var waAsarBkg = Path.Combine(Environment.GetFolderPath(
                 Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar.bkg");
            var tempCSS1 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename1);
            var tempCSS2 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename2);
            var tempIndex = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.index_filename);

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
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\Program Files\\nodejs\\npx.cmd";
                p.StartInfo.Arguments = "npx asar pack " + tempDir + " " + waAsar;
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: An error has occurred: " + p.ExitCode);
                    System.Threading.Thread.Sleep(5000);
                    Environment.Exit(1);
                }
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
            var waAsar = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");

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
            var waAsar = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
            var waAsarBkg = Path.Combine(Environment.GetFolderPath(
                 Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar.bkg");

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
