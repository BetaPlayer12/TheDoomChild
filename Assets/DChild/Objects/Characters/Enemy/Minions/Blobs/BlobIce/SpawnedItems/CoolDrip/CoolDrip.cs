using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Projectiles
{
    public class CoolDrip : PoolableObject
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            DestroyInstance();
        }
    }

}
