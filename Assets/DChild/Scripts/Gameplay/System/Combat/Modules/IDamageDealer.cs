﻿/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/

using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IDamageDealer
    {
        Vector2 position { get; }
        void Damage(TargetInfo target, BodyDefense targetDefense);
    }
}
