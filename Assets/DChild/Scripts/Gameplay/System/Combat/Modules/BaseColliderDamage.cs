/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Base Collider Damage")]
    public class BaseColliderDamage : ColliderDamage
    {
        protected override bool IsValidColliderToHit(Collider2D collision) => true;
    }
}