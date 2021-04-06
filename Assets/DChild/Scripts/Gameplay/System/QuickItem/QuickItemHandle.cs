using System;
using System.Collections;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using Doozy.Engine;
using Doozy.Engine.Nody;
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
        private IItemContainer m_container;
        [SerializeField]
        private bool m_wrapped;

        [ShowInInspector, ReadOnly]
        private int m_currentIndex;
        [SerializeField]
        private GraphController m_graph;
        private ItemSlot m_currentSlot;
        private ConsumableItemData m_currentItem;
        private QuickItemCountRemover m_itemCountRemover;

        private bool m_hideUI;
        public event EventAction<SelectionEventArgs> SelectedItem;
        public event EventAction<SelectionEventArgs> Update;

        public bool isWrapped => m_wrapped;
        public int currentIndex => m_currentIndex;
        public IItemContainer container => m_container;
        public bool hideUI => m_hideUI;

        public bool CanUseCurrentItem() => m_currentItem.CanBeUse(m_player);

        public void UseCurrentItem()
        {
            if (CanUseCurrentItem())
            {
                if (m_player != null)
                {
                    m_currentItem.Use(m_player);
                }
                m_itemCountRemover.Remove(m_currentSlot.item, 1);
            }
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
            if (m_currentSlot == null)
            {
                m_currentIndex--;
                m_currentSlot = m_container.GetSlot(m_currentIndex);
            }

            m_currentItem = (ConsumableItemData)m_currentSlot.item;
            using (Cache<SelectionEventArgs> cacheEventArgs = Cache<SelectionEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_currentIndex, m_currentSlot, selectionType);
                SelectedItem?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (HasItemsInQuickSlot())
            {
                if (m_hideUI)
                {
                    GameplaySystem.gamplayUIHandle.ShowQuickItem(true);
                    m_hideUI = false;
                }

                if (eventArgs.count == 0 && eventArgs.data == m_currentSlot.item)
                {
                    StoreSelectedItem(SelectionEventArgs.SelectionType.Previous);
                }
                else
                {
                    StoreSelectedItem(SelectionEventArgs.SelectionType.None);
                }

            }
            else
            {
                if (m_hideUI == false)
                {
                    GameplaySystem.gamplayUIHandle.ShowQuickItem(false);
                    m_hideUI = true;
                }
            }
        }

        private bool HasItemsInQuickSlot()
        {
            return m_container.HasItemCategory(ItemCategory.Consumable) || m_container.HasItemCategory(ItemCategory.Throwable);
        }

        private IEnumerator DelayedInitialiationRoutine()
        {
            do
            {
                yield return null;
            } while (m_graph.Initialized == false);
            yield return null;
            var hasQuickSlot = HasItemsInQuickSlot();
            m_hideUI = hasQuickSlot == false;
            GameplaySystem.gamplayUIHandle.ShowQuickItem(hasQuickSlot);
        }

        private void Awake()
        {
            m_currentIndex = 0;
            m_currentSlot = m_container.GetSlot(m_currentIndex);
            if (m_currentSlot != null)
            {
                m_currentItem = (ConsumableItemData)m_currentSlot.item;
                m_container.ItemUpdate += OnItemUpdate;
            }
        }

        private void Start()
        {
            m_hideUI = true;
            //This waits for the Nody Graph to initialize itself
            StartCoroutine(DelayedInitialiationRoutine());
            m_itemCountRemover = new QuickItemCountRemover(m_player, m_container);
        }
    }
}
