using System;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Projectiles;
using Holysoft.Pooling;

namespace DChild.Gameplay.Pooling
{
    public class ProjectilePool : ObjectPool<Projectile, Type>
    {
        protected override int FindAvailableItemIndex(Projectile component)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i].projectileName == component.projectileName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
