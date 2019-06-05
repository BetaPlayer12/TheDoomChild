using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSting : AttackProjectile
{
    protected override void Collide()
    {
        CallPoolRequest();
    }
}
