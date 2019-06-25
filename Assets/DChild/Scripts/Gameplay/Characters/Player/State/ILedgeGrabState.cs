﻿namespace DChild.Gameplay.Characters.Players.State
{
    public interface ILedgeGrabState
    {
        bool isLedging { get; set; }
        bool waitForBehaviour { get; set; }
    }
}
