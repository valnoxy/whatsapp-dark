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

        public class ThemeList
        {
            // hard coded version - there is surely a better way...
            // supports only up to 10 themes
            public string theme1 { get; set; }
            public string theme2 { get; set; }
            public string theme3 { get; set; }
            public string theme4 { get; set; }
            public string theme5 { get; set; }
            public string theme6 { get; set; }
            public string theme7 { get; set; }
            public string theme8 { get; set; }
            public string theme9 { get; set; }
            public string theme10 { get; set; }
            public string theme1json { get; set; }
            public string theme2json { get; set; }
            public string theme3json { get; set; }
            public string theme4json { get; set; }
            public string theme5json { get; set; }
            public string theme6json { get; set; }
            public string theme7json { get; set; }
            public string theme8json { get; set; }
            public string theme9json { get; set; }
            public string theme10json { get; set; }
        }

        public class ThemeData
        {
            public string bgcol { get; set; }
            public string bgcol2 { get; set; }
            public string bgcol_hover { get; set; }
            public string titlebar { get; set; }
            public string textcol { get; set; }
            public string accent { get; set; }
            public string accent_hover { get; set; }
            public string accent_pale { get; set; }
            public string col_red { get; set; }
            public string col_green { get; set; }
            public string col_blue { get; set; }
            public string msgout { get; set; }
            public string msgout_deeper { get; set; }
            public string msgin { get; set; }
            public string msgin_deeper { get; set; }
            public string msgbg { get; set; }
            public string rich_textbg { get; set; }
            public string msginfo { get; set; }
        }

        public static string url = "https://dl.exploitox.de/whatsapp-dark/version_v2.json";
        public static string themeurl = "https://dl.exploitox.de/whatsapp-dark/themes.json";

        public static string themeid_url;

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
                var ver = _serialized_json_data<VersionData>(url);

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
                    ThemeSelection();
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

        private static void ThemeSelection()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n ██╗    ██╗ █████╗ ██████╗  █████╗ ██████╗ ██╗  ██╗\n" +
                " ██║    ██║██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝\n" +
                " ██║ █╗ ██║███████║██║  ██║███████║██████╔╝█████╔╝ \n" +
                " ██║███╗██║██╔══██║██║  ██║██╔══██║██╔══██╗██╔═██╗ \n" +
                " ╚███╔███╔╝██║  ██║██████╔╝██║  ██║██║  ██║██║  ██╗\n" +
                "  ╚══╝╚══╝ ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝");

            var theme = _serialized_json_data<ThemeList>(themeurl);

            try
            {
                var ver = _serialized_json_data<VersionData>(url);
                Console.WriteLine("WADark [Version: {0} (Beta Version)]\n", ver.waversion);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose a theme:");
                if (File.Exists("theme.json"))
                    Console.WriteLine("   0) Local theme (WADark\\theme.json)");
                if (!String.IsNullOrEmpty(theme.theme1)) 
                    Console.WriteLine("   1) {0}", theme.theme1);
                if (!String.IsNullOrEmpty(theme.theme2))
                    Console.WriteLine("   2) {0}", theme.theme2);
                if (!String.IsNullOrEmpty(theme.theme3))
                    Console.WriteLine("   3) {0}", theme.theme3);
                if (!String.IsNullOrEmpty(theme.theme4))
                    Console.WriteLine("   4) {0}", theme.theme4);
                if (!String.IsNullOrEmpty(theme.theme5))
                    Console.WriteLine("   5) {0}", theme.theme5);
                if (!String.IsNullOrEmpty(theme.theme6))
                    Console.WriteLine("   6) {0}", theme.theme6);
                if (!String.IsNullOrEmpty(theme.theme7))
                    Console.WriteLine("   7) {0}", theme.theme7);
                if (!String.IsNullOrEmpty(theme.theme8))
                    Console.WriteLine("   8) {0}", theme.theme8);
                if (!String.IsNullOrEmpty(theme.theme9))
                    Console.WriteLine("   9) {0}", theme.theme9);
                if (!String.IsNullOrEmpty(theme.theme10))
                    Console.WriteLine("  10) {0}", theme.theme10);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: " + ex);
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(1);
            }

            bool confirmed = false;

            while (!confirmed)
            {
                Console.Write("\nSelect a theme: ");
                string option = Console.ReadLine();

                if (option == "1")
                {
                    if (!String.IsNullOrEmpty(theme.theme1))
                    {
                        themeid_url = theme.theme1json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "2")
                {
                    if (!String.IsNullOrEmpty(theme.theme2))
                    {
                        themeid_url = theme.theme2json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "3")
                {
                    if (!String.IsNullOrEmpty(theme.theme3))
                    {
                        themeid_url = theme.theme3json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "4")
                {
                    if (!String.IsNullOrEmpty(theme.theme4))
                    {
                        themeid_url = theme.theme4json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "5")
                {
                    if (!String.IsNullOrEmpty(theme.theme5))
                    {
                        themeid_url = theme.theme5json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "6")
                {
                    if (!String.IsNullOrEmpty(theme.theme6))
                    {
                        themeid_url = theme.theme6json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "7")
                {
                    if (!String.IsNullOrEmpty(theme.theme7))
                    {
                        themeid_url = theme.theme7json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "8")
                {
                    if (!String.IsNullOrEmpty(theme.theme8))
                    {
                        themeid_url = theme.theme8json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "9")
                {
                    if (!String.IsNullOrEmpty(theme.theme9))
                    {
                        themeid_url = theme.theme9json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
                if (option == "10")
                {
                    if (!String.IsNullOrEmpty(theme.theme10))
                    {
                        themeid_url = theme.theme10json;
                        confirmed = true;
                    }
                    else confirmed = false;
                }
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
                var ver = _serialized_json_data<VersionData>(url);
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
                var ver = _serialized_json_data<VersionData>(url);

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

        private static void CheckSys()
        {
            var tempNodeJS = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "nodejs.msi");
            var tempHomebrew = "/Users/" + Environment.UserName + "/Library/Preferences/Exploitox/WADark/Homebrew.sh";

            // Check WhatsApp version
            var ver = _serialized_json_data<VersionData>(url);
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
                    p.StartInfo.Arguments = "/i " + tempNodeJS + " /quiet /qn";
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
            var ver = _serialized_json_data<VersionData>(url);
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
            var ver = _serialized_json_data<VersionData>(url);
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

            // Switch values to selected theme
            try
            {
                var themejson = _serialized_json_data<ThemeData>(themeid_url);
                string style = File.ReadAllText(tempCSS1);
                style = style.Replace("--bgcol: #000000", "--bgcol: " + themejson.bgcol);
                style = style.Replace("--bgcol2: #000000", "--bgcol2: " + themejson.bgcol2);
                style = style.Replace("--bgcol_hover: #000000", "--bgcol_hover: " + themejson.bgcol_hover);
                style = style.Replace("--titlebar: #000000", "--titlebar: " + themejson.titlebar);
                style = style.Replace("--textcol: #000000", "--textcol: " + themejson.textcol);
                style = style.Replace("--accent: #000000", "--accent: " + themejson.accent);
                style = style.Replace("--accent_hover: #000000", "--accent_hover: " + themejson.accent_hover);
                style = style.Replace("--accent_pale: #000000", "--accent_pale: " + themejson.accent_pale);
                style = style.Replace("--col_red: #000000", "--col_red: " + themejson.col_red);
                style = style.Replace("--col_green: #000000", "--col_green: " + themejson.col_green);
                style = style.Replace("--col_blue: #000000", "--col_blue: " + themejson.col_blue);
                style = style.Replace("--msgout: #000000", "--msgout: " + themejson.msgout);
                style = style.Replace("--msgout_deeper: #000000", "--msgout_deeper: " + themejson.msgout_deeper);
                style = style.Replace("--msgin: #000000", "--msgin: " + themejson.msgin);
                style = style.Replace("--msgin_deeper: #000000", "--msgin_deeper: " + themejson.msgin_deeper);
                style = style.Replace("--msgbg: #000000", "--msgbg: " + themejson.msgbg);
                style = style.Replace("--rich_textbg: #000000", "--rich_textbg: " + themejson.rich_textbg);
                style = style.Replace("--msginfo: #000000", "--msginfo: " + themejson.msginfo);
                File.WriteAllText(tempCSS1, style);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot modify style file: " + ex);
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
            var ver = _serialized_json_data<VersionData>(url);
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
            var ver = _serialized_json_data<VersionData>(url);
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
