using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryHandle : MonoBehaviour
    {
        [SerializeField]
        private BestiaryInfoUI m_infoPage;

        public void Select(BestiaryIndexButton indexButton)
        {
            m_infoPage.ShowInfo(indexButton.data);
        }
    }
}