using DChild.Temp;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class StoreNavigator : SerializedMonoBehaviour
    {
        [SerializeField]
        private StorePage m_currentPage;
        [SerializeField]
        private Dictionary<StorePage, string> m_eventStrings;

        public void SetPage(StorePage page) => m_currentPage = page;
        public void SetPage(int page) => m_currentPage = (StorePage)page;

        public void OpenPage()
        {
            GameEventMessage.SendEvent(m_eventStrings[m_currentPage]);
        }
    }
}