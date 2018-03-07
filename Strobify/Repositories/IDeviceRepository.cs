﻿using System.Collections.Generic;
using Strobify.Model;

namespace Strobify.Repositories
{
    public interface IDeviceRepository :IRepository<GameController, string>
    {
        IEnumerable<GameController> GetControllers();
        //IEnumerable<GameController> Find(string text);
    }
}