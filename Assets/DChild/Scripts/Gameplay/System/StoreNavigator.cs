using DChild.Temp;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class StoreNavigator : SerializedMonoBehaviour
    {
        [SerializeField]
        private SignalSender m_openStoreSignal;
        [SerializeField]
        private StorePage m_currentPage;
        [SerializeField]
        private Dictionary<StorePage, UIToggle> m_pageToggleButtons;

        public void SetPage(StorePage page) => m_currentPage = page;
        public void SetPage(int page) => m_currentPage = (StorePage)page;

        public void OpenStore()
        {
            m_openStoreSignal.SendSignal();
        }

        public void OpenPage()
        {
            m_pageToggleButtons[m_currentPage].SetIsOn(true);
        }
    }
}