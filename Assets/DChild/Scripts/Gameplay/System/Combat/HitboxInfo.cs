using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct HitboxInfo
    {
        public int targetID { get; }
        public Vector2 position { get; }
        public bool isInvulnerable { get; }

        public HitboxInfo(Hitbox hitbox)
        {
            targetID = hitbox.GetInstanceID();
            position = hitbox.transform.position;
            isInvulnerable = hitbox.defense.isInvulnerable;
        }
    }
}