/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Strobify"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Strobify.Repositories;
using Strobify.Repositories.Interfaces;
using Strobify.Services;
using Strobify.Services.Interfaces;
using Strobify.Strategies;
using Strobify.Strategies.Interfaces;

namespace Strobify.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IDeviceRepository, DeviceRepository>();
            SimpleIoc.Default.Register<IDeviceService, DeviceService>();
            SimpleIoc.Default.Register<IButtonMapperStrategy, ButtonMapperStrategy>();
            SimpleIoc.Default.Register<IControllerButtonMapper, ControllerButtonMapper>();
            SimpleIoc.Default.Register<IKeyboardButtonMapper, KeyboardButtonMapper>();
            SimpleIoc.Default.Register<ILightService, LightService>();

            SimpleIoc.Default.Register<GameControllerViewModel>();
        }

        public GameControllerViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameControllerViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}