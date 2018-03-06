using System;
using System.ComponentModel;

namespace Strobify.Model
{
    public class GameController : INotifyPropertyChanged
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        Guid _deviceGuid;
        public Guid DeviceGuid
        {
            get { return _deviceGuid; }
            set
            {
                if (_deviceGuid != value)
                {
                    _deviceGuid = value;
                    RaisePropertyChanged("DeviceGuid");
                }
            }
        } 

        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}