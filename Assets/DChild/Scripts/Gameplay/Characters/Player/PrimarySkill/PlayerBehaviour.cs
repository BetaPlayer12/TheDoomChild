using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [Flags]
    public enum PlayerBehaviour
    {
        Jump = 1<< 0,
        Attack = 1<< 1,
        Movement =  1<< 2,

        [HideInInspector]
        All = Jump | Attack | Movement,
        [HideInInspector]
        None = 0
    }
}
