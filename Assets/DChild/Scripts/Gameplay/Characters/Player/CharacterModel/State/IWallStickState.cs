﻿namespace DChild.Gameplay.Characters.Players.State
{
    public interface IWallStickState
    {
        bool isStickingToWall { get; set; }
        bool canWallCrawl { get; set; }
    }
}