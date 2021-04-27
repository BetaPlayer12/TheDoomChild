using DChild.Gameplay;
using DChild.Gameplay.Inventories;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleInfiniteItem : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_quickItem;

        public bool value => m_quickItem.removeItemCountOnConsume;

        [Button]
        public void ToggleOn()
        {
            m_quickItem.removeItemCountOnConsume = false;
        }
        [Button]
        public void ToggleOff()
        {
            m_quickItem.removeItemCountOnConsume = true;
        }
    }
}