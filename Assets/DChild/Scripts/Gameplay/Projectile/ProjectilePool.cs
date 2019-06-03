using System;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Pooling
{
    public class ProjectilePool : ObjectPool<Projectile, Type>
    {
        public Projectile RetrieveFromPool(string request)
        {
            if (m_poolCount > 0)
            {
                var index = FindIndex(request);
                if (index >= 0)
                {
                    var item = RemoveFromPool(index);
                    return item;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected int FindIndex(string request)
        {

            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i].projectileName == request)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override int FindIndex(Type request)
        {
            if (request.IsAbstract)
            {
                for (int i = 0; i < m_items.Count; i++)
                {
                    if (m_items[i] != null && m_items[i].GetType().IsSubclassOf(request))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_items.Count; i++)
                {
                    if (m_items[i] != null && m_items[i].GetType() == request)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
