﻿namespace Strobify.View
{
    using MahApps.Metro.Controls;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StringValidation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
            KeyboardButtonTxtBox.SelectAll();
        }
    }
}