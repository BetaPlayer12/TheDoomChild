using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryEntity : MonoBehaviour
    {
        [SerializeField]
        private BestiaryData m_data;

        public int bestiaryID => m_data.id;
        public void SetData(BestiaryData data)
        {
            m_data = data;
        }
    }
}