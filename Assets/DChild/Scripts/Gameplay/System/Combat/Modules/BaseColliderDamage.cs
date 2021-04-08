/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using Holysoft;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Base Collider Damage")]
    public class BaseColliderDamage : ColliderDamage
    {
        protected override bool IsValidToHit(Collider2D collision) => true;
    }
}
