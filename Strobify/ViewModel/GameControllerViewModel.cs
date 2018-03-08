﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Strobify.Helpers;
using Strobify.Model;
using Strobify.Repositories.Interfaces;
using System.Collections.ObjectModel;

namespace Strobify.ViewModel
{
    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; } = new ObservableCollection<GameController>();
        readonly IDeviceRepository _deviceRepository = SimpleIoc.Default.GetInstance<IDeviceRepository>();

        object _selectedDevice;
        public object SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                Set(ref _selectedDevice, value);
            }
        }

        public RelayCommand GetDevicesCommand { get; set; }

        public string DevicesContent => "Get devices";

        public void InitCommands()
        {
            this.GetDevicesCommand = new RelayCommand((parameter) =>
                { InitGameControllerList(); }
            );
        }

        private void InitGameControllerList()
        {
            GameControllers.Clear();
            
            foreach (var device in _deviceRepository.GetControllers())
            {
                GameControllers.Add(device);
            }
        }

        public GameControllerViewModel()
        {
            InitCommands();
        }
    }
}