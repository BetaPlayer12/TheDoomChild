/******************************
 * 
 * Manages FX that are instantiated rather than played;
 * 
 ******************************/
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class FXPool : ObjectPool<FX, string>
    {
        protected override int FindIndex(string request)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i].fxName == request)
                {
                    return i;
                }
            }

            return -1;
        }
    }

}