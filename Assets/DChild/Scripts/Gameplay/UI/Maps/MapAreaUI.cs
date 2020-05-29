using DChild.Serialization;
using UnityEngine;

namespace DChild.Gameplay.UI.Map
{

    public class MapAreaUI : MonoBehaviour
    {
        [SerializeField]
        private SerializeID m_mapID;
        [SerializeField]
        private Canvas m_area;

        public void Update(DynamicSerializableData data)
        {
            m_area.enabled = data.GetData<MapRevealator.SaveData>().IsRevealed(m_mapID);
        }
    }
}