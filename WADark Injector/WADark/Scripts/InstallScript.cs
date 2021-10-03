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

using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WADark
{
    class InstallScript
    {
        public static string url = "https://dl.exploitox.de/whatsapp-dark/version_v3.json";
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

        public class VersionData
        {
            public string waversion { get; set; }
            public string node_win { get; set; }
            public string css_filename1 { get; set; }
            public string css_dl1 { get; set; }
        }

        public static int Installation(string JsonOrUrl)
        {
            var ver = _serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var tempDir = "";
            var waAsar = "";
            var tempCSS1 = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
                tempCSS1 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename1);

            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
                tempDir = "/tmp/Exploitox/WADark/asar_extract/";
                tempCSS1 = tempDir + ver.css_filename1;
            }

            // Patch extracted files
            try
            {
                // Delete old files
                if (File.Exists(tempCSS1)) { File.Delete(tempCSS1); }

                // Download patched files
                using (var w = new WebClient())
                {
                    w.DownloadFile(ver.css_dl1, tempCSS1);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot patch asar file: " + ex);
                System.Threading.Thread.Sleep(3000);
                return 101;
            }

            // Switch values to selected theme
            try
            {
                var themejson = _serialized_json_data<ThemeData>(JsonOrUrl);
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
                return 102;
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
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot create new asar file.");
                System.Threading.Thread.Sleep(3000);
                return 103;
            }
            return 0;
        }

        public static int ThemeInstallation(string JsonOrUrl)
        {
            var ver = _serialized_json_data<VersionData>(url);
            string waVer = "app-" + ver.waversion;
            var tempDir = "";
            var waAsar = "";
            var tempCSS1 = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                tempDir = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract");
                waAsar = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "WhatsApp", waVer, "resources", "app.asar");
                tempCSS1 = Path.Combine(Path.GetTempPath(), "Exploitox", "WADark", "asar_extract", ver.css_filename1);

            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                waAsar = "/Applications/WhatsApp.app/Contents/Resources/app.asar";
                tempDir = "/tmp/Exploitox/WADark/asar_extract/";
                tempCSS1 = tempDir + ver.css_filename1;
            }

            // Patch extracted files
            try
            {
                // Delete old files
                if (File.Exists(tempCSS1)) { File.Delete(tempCSS1); }

                // Download patched files
                using (var w = new WebClient())
                {
                    w.DownloadFile(ver.css_dl1, tempCSS1);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot patch asar file: " + ex);
                System.Threading.Thread.Sleep(3000);
                return 101;
            }

            // Switch values to selected theme
            try
            {
                var themejson = _serialized_json_data<ThemeData>(JsonOrUrl);
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
                return 102;
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
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!] Error: Cannot create new asar file.");
                System.Threading.Thread.Sleep(3000);
                return 103;
            }
            return 0;
        }

        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||  //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]")))    //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    // Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) // some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static T _serialized_json_data<T>(string url) where T : new()
        {
            if (IsValidJson(url))
            {
                var json_data = string.Empty;
                try
                {
                    json_data = url;
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
