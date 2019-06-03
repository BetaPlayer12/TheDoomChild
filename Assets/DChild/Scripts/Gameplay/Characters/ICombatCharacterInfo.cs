using UnityEngine;
using System;
using Holysoft.Event;

namespace DChild.Gameplay.Characters
{
    public interface ICombatCharacterInfo
    {
        Vector2 position { get; }
        bool isAlive { get; }
        Type GetType();
    }
}
