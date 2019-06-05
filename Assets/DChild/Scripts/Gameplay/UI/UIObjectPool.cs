using DChild.UI;
using Holysoft.Pooling;
using System;

namespace DChild.Gameplay.Pooling
{
    public class UIObjectPool : ObjectPool<UIObject, Type>
    {
        protected override int FindAvailableItemIndex(UIObject component)
        {
            var request = component.GetType();

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