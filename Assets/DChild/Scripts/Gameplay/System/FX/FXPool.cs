/******************************
 * 
 * Manages FX that are instantiated rather than played;
 * 
 ******************************/
using DChild.Gameplay.Pooling;
using Holysoft.Pooling;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class FXPool : ObjectPool<FX, string>
    {
        protected override int FindAvailableItemIndex(FX component)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i].fxName == component.fxName)
                {
                    return i;
                }
            }

            return -1;
        }
    }

}