using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Items;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemHandle : SerializedMonoBehaviour
    {
        public class SelectionEventArgs : IEventActionArgs
        {
            public enum SelectionType
            {
                Next,
                Previous,
                None
            }

            public int currentIndex { get; private set; }
            public ItemSlot currentSlot { get; private set; }
            public SelectionType selectionType { get; private set; }

            public void Initialize(int currentIndex, ItemSlot currentSlot, SelectionType selectionType)
            {
                this.currentIndex = currentIndex;
                this.currentSlot = currentSlot;
                this.selectionType = selectionType;
            }
        }

        [SerializeField]
        private IPlayer m_player;
        [SerializeField]
        private bool m_wrapped;
        [SerializeField]
        private IItemContainer m_container;

        [ShowInInspector, ReadOnly]
        private int m_currentIndex;
        private ItemSlot m_currentSlot;
        public event EventAction<SelectionEventArgs> SelectedItem;
        public event EventAction<SelectionEventArgs> Update;

        public bool isWrapped => m_wrapped;
        public int currentIndex => m_currentIndex;
        public IItemContainer container => m_container;

        public void UseCurrentItem()
        {
            m_currentSlot = m_container.GetSlot(m_currentIndex);
            if (m_player != null)
            {
                ((UsableItemData)m_currentSlot.item).Use(m_player);
            }
            m_container.AddItem(m_currentSlot.item, -1);
        }

        [Button, HorizontalGroup("Split"), HideInEditorMode]
        public void Previous()
        {
            if (m_currentIndex == 0)
            {
                if (m_wrapped)
                {
                    m_currentIndex = m_container.Count - 1;
                    StoreSelectedItem(SelectionEventArgs.SelectionType.Previous);
                }
                else
                {
                    StoreSelectedItem(SelectionEventArgs.SelectionType.None);
                }
            }
            else
            {
                m_currentIndex--;
                StoreSelectedItem(SelectionEventArgs.SelectionType.Previous);
            }
        }

        [Button, HorizontalGroup("Split"), HideInEditorMode]
        public void Next()
        {
            if (m_currentIndex == m_container.Count - 1)
            {
                if (m_wrapped)
                {
                    m_currentIndex = 0;
                    StoreSelectedItem(SelectionEventArgs.SelectionType.Next);
                }
                else
                {
                    StoreSelectedItem(SelectionEventArgs.SelectionType.None);
                }
            }
            else
            {
                m_currentIndex++;
                StoreSelectedItem(SelectionEventArgs.SelectionType.Next);
            }
        }

        private void StoreSelectedItem(SelectionEventArgs.SelectionType selectionType)
        {
            m_currentSlot = m_container.GetSlot(m_currentIndex);
            using (Cache<SelectionEventArgs> cacheEventArgs = Cache<SelectionEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_currentIndex, m_currentSlot, selectionType);
                SelectedItem?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.count == 0 && eventArgs.data == m_currentSlot.item)
            {
                Previous();
            }
        }

        private void Awake()
        {
            m_currentIndex = 0;
            m_currentSlot = m_container.GetSlot(m_currentIndex);
            m_container.ItemUpdate += OnItemUpdate;
        }
    }
}
