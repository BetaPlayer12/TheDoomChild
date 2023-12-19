using System;
using System.Collections;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Items;
using Doozy.Runtime.Nody;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

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
            public IStoredItem currentSlot { get; private set; }
            public SelectionType selectionType { get; private set; }


            public void Initialize(int currentIndex, IStoredItem currentSlot, SelectionType selectionType)
            {
                this.currentIndex = currentIndex;
                this.currentSlot = currentSlot;
                this.selectionType = selectionType;
            }
        }

        [SerializeField]
        private IPlayer m_player;
        [SerializeField]
        private IInventory m_inventory;
        [SerializeField]
        private QuickItemSelections m_selections;
        [SerializeField]
        private bool m_wrapped;


        [ShowInInspector, ReadOnly]
        private int m_currentIndex;
        [SerializeField]
        private FlowController m_graph;
        [SerializeField]
        private QuickItemCooldown m_cooldown;
        [SerializeField]
        private bool m_removeItemCountOnConsume;
        private IStoredItem m_currentItem;
        private ConsumableItemData m_currentItemData;
        private QuickItemCountRemover m_itemCountRemover;

        private bool m_hideUI;
        public event EventAction<SelectionEventArgs> SelectedItem;
        public event EventAction<SelectionEventArgs> Update;

        #region PRE_ALPHA
        public event Action<string> ItemUsed;
        #endregion


        public bool isWrapped => m_wrapped;
        public int currentIndex => m_currentIndex;
        public bool removeItemCountOnConsume { get => m_removeItemCountOnConsume; set => m_removeItemCountOnConsume = value; }
        public bool hideUI => m_hideUI;

        public bool CanUseCurrentItem() => m_currentItemData.CanBeUse(m_player);

        public void UseCurrentItem()
        {
            if (m_cooldown.isOver && CanUseCurrentItem())
            {
                if (m_player != null)
                {
                    m_currentItemData.Use(m_player);
                    m_cooldown.StartCooldown();
                    ItemUsed?.Invoke(m_currentItemData.itemName);
                }
                if (m_removeItemCountOnConsume)
                {
                    m_itemCountRemover.Remove(m_currentItem.data, 1);
                }
            }
        }

        [Button, HorizontalGroup("Split"), HideInEditorMode]
        public void Previous()
        {
            if (m_currentIndex == 0)
            {
                if (m_wrapped)
                {
                    m_currentIndex = m_selections.itemCount - 1;
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
            if (m_currentIndex == m_selections.itemCount - 1)
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
            m_currentItem = m_selections.GetItem(m_currentIndex);
            if (m_currentItem == null)
            {
                m_currentIndex--;
                if (m_currentIndex < 0)
                {
                    m_currentIndex = 0;
                }
                m_currentItem = m_selections.GetItem(m_currentIndex);

            }
            if (m_currentItem != null)
            {
                m_currentItemData = (ConsumableItemData)m_currentItem.data;

            }
            else
            {
                m_currentItem = null;
            }

            InvokeSelectedItemEvent(selectionType);
        }

        private void InvokeSelectedItemEvent(SelectionEventArgs.SelectionType selectionType)
        {
            using (Cache<SelectionEventArgs> cacheEventArgs = Cache<SelectionEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_currentIndex, m_currentItem, selectionType);
                SelectedItem?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }


        private void OnSelectionUpdate(object sender, EventActionArgs eventArgs)
        {
            if (m_selections.HasItems())
            {
                //if (m_hideUI)
                //{
                //    GameplaySystem.gamplayUIHandle.ShowQuickItem(true);
                //    m_hideUI = false;
                //}

                if (m_selections.IsInSelections(m_currentItemData))
                {
                    UpdateCurrentItemIndex();
                    StoreSelectedItem(SelectionEventArgs.SelectionType.None);
                }
                else
                {
                    StoreSelectedItem(SelectionEventArgs.SelectionType.Previous);
                }
            }
            else
            {
                m_currentItem = null;
                m_currentItemData = null;
                m_currentIndex = 0;
                StoreSelectedItem(SelectionEventArgs.SelectionType.None);
                //if (m_hideUI == false)
                //{
                //    GameplaySystem.gamplayUIHandle.ShowQuickItem(false);
                //    m_hideUI = true;
                //}
            }
        }

        private void OnSelectionDetailsUpdate(object sender, EventActionArgs eventArgs)
        {
            StoreSelectedItem(SelectionEventArgs.SelectionType.None);
        }

        private void UpdateCurrentItemIndex()
        {
            if (m_currentItemData == null)
            {
                m_currentIndex = 0;
            }
            else
            {
                m_currentIndex = m_selections.FindIndexOf(m_currentItem);
            }
        }

        private IEnumerator DelayedInitialiationRoutine()
        {
            do
            {
                yield return null;
            } while (m_graph.initialized == false);
            yield return null;
            var hasQuickSlot = m_selections.HasItems();
            //m_hideUI = hasQuickSlot == false;
            //GameplaySystem.gamplayUIHandle.ShowQuickItem(hasQuickSlot);
        }

        private void Awake()
        {
            m_currentIndex = 0;
            if (m_currentItem != null)
            {
                m_currentItemData = (ConsumableItemData)m_currentItem.data;
            }
            m_selections.SelectionUpdate += OnSelectionUpdate;
            m_selections.SelectionDetailsUpdate += OnSelectionDetailsUpdate;
            if (m_selections.HasItems())
            {
                m_currentItem = m_selections.GetItem(m_currentIndex);
            }
            //m_removeItemCountOnConsume = true;
        }

        private void Start()
        {
            //This waits for the Nody Graph to initialize itself
            StartCoroutine(DelayedInitialiationRoutine());
            m_itemCountRemover = new QuickItemCountRemover(m_player, m_inventory);
            m_cooldown.ResetCooldown();
        }
    }
}
