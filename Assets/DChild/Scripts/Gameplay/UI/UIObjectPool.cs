using DChild.UI;
using System;

namespace DChild.Gameplay.Pooling
{
    public class UIObjectPool : ObjectPool<UIObject, Type>
    {
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