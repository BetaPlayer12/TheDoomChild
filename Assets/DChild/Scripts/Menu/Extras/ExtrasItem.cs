using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Extras
{
    public struct ItemSelectedEventArgs : IEventActionArgs
    {
        public ItemSelectedEventArgs(int itemIndex) : this()
        {
            this.itemIndex = itemIndex;
        }

        public int itemIndex { get; }
    }

    public class ExtrasItem : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, MinValue(0)]
#endif
        private int m_itemIndex;

        public event EventAction<ItemSelectedEventArgs> Selected;

        public int itemIndex => m_itemIndex;

        public void SetIndex(int index) => m_itemIndex = index;

        public void Select() => Selected?.Invoke(this, new ItemSelectedEventArgs(m_itemIndex));
    }
}