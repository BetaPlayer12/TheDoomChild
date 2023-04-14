using System;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Projectiles;
using Holysoft.Pooling;
using UnityEngine.SceneManagement;

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
                SceneManager.MoveGameObjectToScene(instance.gameObject, SceneManager.GetActiveScene());
                instance.ResetState();
            }
            return instance;
        }
    }
}
