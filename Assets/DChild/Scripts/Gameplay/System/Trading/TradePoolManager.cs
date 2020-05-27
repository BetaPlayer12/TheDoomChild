using DChild.Gameplay.Inventories;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class TradePoolManager : MonoBehaviour, IReferenceFactory, IReferenceFactoryData
    {
        [SerializeField]
        private RectTransform m_content;
        [SerializeField]
        private Transform m_tempParent;
        [SerializeField]
        private Transform m_UIContainer;
        [SerializeField]
        private UIScrollViewContentAdjustor m_adjustor;
        [SerializeField]
        private GameObject m_itemTemplate;
        [SerializeField, MinValue(1)]
        private int m_preloadedTemplateCount;

        public event EventAction<ReferenceInstanceEventArgs> InstanceCreated;
        private List<TradableItemUI> m_itemUIs;
        private bool m_sizeChange;

        public int instanceCount
        {
            get
            {
                if (m_itemUIs.Count > m_preloadedTemplateCount)
                {
                    return m_itemUIs.Count;
                }
                else
                {
                    int count = 0;
                    for (int i = 0; i < m_itemUIs.Count; i++)
                    {
                        if (m_itemUIs[i].gameObject.activeSelf)
                        {
                            count++;
                        }
                    }
                    return count;
                }
            }
        }

        public void OrganizeUIObjects(IItemContainer inventory)
        {
            var inventoryCount = inventory.Count;
            if (inventoryCount != m_itemUIs.Count)
            {
                if (inventoryCount > m_preloadedTemplateCount)
                {
                    //Create new ones when lacking or destroy excesses
                    if (inventoryCount < m_itemUIs.Count)
                    {
                        for (int i = m_itemUIs.Count - 1; i >= inventoryCount; i--)
                        {
                            Destroy(m_itemUIs[i].gameObject);
                        }
                    }
                    else
                    {
                        for (int i = m_itemUIs.Count; i < inventoryCount; i++)
                        {
                            m_itemUIs.Add(CreateItemUI(i));
                        }
                    }
                }
                else if (m_itemUIs.Count > m_preloadedTemplateCount)
                {
                    for (int i = m_itemUIs.Count - 1; i >= 10; i--)
                    {
                        Destroy(m_itemUIs[i].gameObject);
                    }
                }
                m_sizeChange = true;
            }

            EnableUIObjects(inventory);
            PopulateList(inventory);
        }

        private void EnableUIObjects(IItemContainer inventory)
        {
            AdjustContentSpaceSize();

            int index = 0;
            for (; index < inventory.Count; index++)
            {
                m_itemUIs[index].gameObject.SetActive(true);
            }
            for (; index < m_preloadedTemplateCount; index++)
            {
                m_itemUIs[index].gameObject.SetActive(false);
            }
        }

        private void AdjustContentSpaceSize()
        {
            m_UIContainer.gameObject.SetActive(false);
            m_UIContainer.transform.parent = m_tempParent;

            //TODO: Expand The Content Size based on the amount of stuff
            if (m_sizeChange)
            {
                m_adjustor.UpdateSize();
                m_sizeChange = false;
            }


            m_UIContainer.transform.parent = m_content;
            m_UIContainer.gameObject.SetActive(true);
        }

        private void PopulateList(IItemContainer inventory)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                m_itemUIs[i].Set(inventory.GetSlot(i));
            }
        }

        private TradableItemUI CreateItemUI(int index)
        {
            var instance = Instantiate(m_itemTemplate);
            instance.transform.parent = m_UIContainer;

            using (Cache<ReferenceInstanceEventArgs> cacheEvent = Cache<ReferenceInstanceEventArgs>.Claim())
            {
                cacheEvent.Value.SetValue(instance);
                cacheEvent.Value.referenceIndex = index;
                //Place them by UIContentOrganizer 
                InstanceCreated?.Invoke(this, cacheEvent);
                cacheEvent.Release();
            }
            return instance.GetComponent<TradableItemUI>();
        }

        private void Awake()
        {
            m_itemUIs = new List<TradableItemUI>();
            for (int i = 0; i < m_preloadedTemplateCount; i++)
            {
                m_itemUIs.Add(CreateItemUI(i));
            }
            AdjustContentSpaceSize();
        }
    }
}