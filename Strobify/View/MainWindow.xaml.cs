namespace Strobify.View
{
    using GalaSoft.MvvmLight.Ioc;
    using MahApps.Metro.Controls;
    using Strobify.Services.Interfaces;
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

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var _configurationService = SimpleIoc.Default.GetInstance<IConfigurationService>();
            _configurationService.SaveConfiguration();
        }
    }
}