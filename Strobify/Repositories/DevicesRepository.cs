using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Converters;
using Strobify.Model;

namespace Strobify.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        public GameController Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(GameController entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(GameController entity)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<GameController> GetControllers()
        {
            var p = new List<GameController>()
            {
                new GameController() { Name = "Steering Wheel", DeviceGuid = new Guid("13757675-0365-49DD-972D-D5954E7E7FD3") },
                new GameController() { Name = "Joystick", DeviceGuid = new Guid("13757675-0365-49DD-972D-D5954E7E7FD3")},
                new GameController() { Name = "Direct Drive", DeviceGuid = new Guid("13757675-0365-49DD-972D-D5954E7E7FD3")
                }
            };
            return p;
        }
    }
}
