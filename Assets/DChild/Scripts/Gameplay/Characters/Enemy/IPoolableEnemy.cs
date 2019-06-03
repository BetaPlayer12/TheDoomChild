using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface IPoolableEnemy
    {
        event EventAction<EventActionArgs> Death;

    }

}