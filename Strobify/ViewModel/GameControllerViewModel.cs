using Strobify.Model;
using Strobify.Helpers;
using System.Collections.ObjectModel;

namespace Strobify.ViewModel
{
    class GameControllerViewModel : MainViewModel
    {
        public ObservableCollection<GameController> GameControllers { get; set; }

        public RelayCommand GetGameControllers { get; set; }

        public string GameControllerContent
        {
            get
            {
                return "Get game controllers";
            }
        }
    }
}
