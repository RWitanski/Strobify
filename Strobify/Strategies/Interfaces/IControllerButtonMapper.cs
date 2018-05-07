namespace Strobify.Strategies.Interfaces
{
    using Strobify.Model;
    using System;

    public interface IControllerButtonMapper
    {
        void AssignControllerButtonId(GameController gameController);
        Boolean IsMapperMode { get; set; }
    }
}