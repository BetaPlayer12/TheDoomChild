using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpitProjectile : AttackProjectile
{
   
    protected override void Collide()
    {
        PoolObject();
    }
}
