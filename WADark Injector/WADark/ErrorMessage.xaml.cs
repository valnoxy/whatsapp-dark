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
using System.Windows;

namespace WADark
{
    /// <summary>
    /// Interaktionslogik für ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        public ErrorMessage(string message)
        {
            InitializeComponent();
            ErrorString.Text = message;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
