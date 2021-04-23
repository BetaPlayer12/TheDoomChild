using DChild.Gameplay;
using DChild.Gameplay.Inventories;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleInfiniteItem : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_quickItem;
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