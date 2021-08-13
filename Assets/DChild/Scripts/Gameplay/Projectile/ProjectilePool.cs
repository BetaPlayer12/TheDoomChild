using System;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Projectiles;
using Holysoft.Pooling;

namespace DChild.Gameplay.Pooling
{
    public class ProjectilePool : ObjectPool<Projectile>
    {
        protected override Projectile RetrieveFromPool(Projectile component)
        {
            var instance =  base.RetrieveFromPool(component);
            if (instance != null)
            {
                instance.transform.parent = null;
                instance.ResetState();
            }
            return instance;
        }
    }
}
