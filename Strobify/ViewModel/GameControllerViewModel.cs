﻿using System;
using Strobify.Model;
using Strobify.Helpers;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Strobify.ViewModel
{
    public class GameControllerViewModel : ViewModelBase
    {
        public ObservableCollection<GameController> GameControllers { get; set; }

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

        public string DevicesContent
        {
            get
            {
                return "Get devices";
            }
        }
        public GameControllerViewModel()
        {
            
            GameControllers = new ObservableCollection<GameController>
            {
                    new GameController { Name="Kierownica" , DeviceGuid= new Guid("13757675-0365-49DD-972D-D5954E7E7FD3") },
                    new GameController { Name="Joystick" , DeviceGuid= new Guid("852836DE-2A86-4192-AE8A-013BB11EA07F") },
                    new GameController { Name="Joystick" , DeviceGuid= new Guid("DE33E027-8978-4E46-870C-A59FE105359C") },
                    new GameController { Name="Fanatec" , DeviceGuid= new Guid("AE7A1C01-4545-4EDA-9F2B-3122B474F918") }
            };
        }
    }
}
