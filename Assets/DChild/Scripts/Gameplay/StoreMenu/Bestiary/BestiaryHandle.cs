using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryHandle : MonoBehaviour
    {
        [SerializeField]
        private BestiaryIndexHandle m_indexHandle;
        [SerializeField]
        private BestiaryInfoUI m_infoPage;

        private BestiaryData m_selectedBestiaryData;
        private bool m_lockOnSelectedData;

        public void Select(BestiaryIndexButton indexButton)
        {
            if (m_lockOnSelectedData == false)
            {
                m_selectedBestiaryData = indexButton.data;
                m_infoPage.ShowInfo(m_selectedBestiaryData);
                indexButton.SetState(Doozy.Runtime.UIManager.UISelectionState.Pressed);
                StopAllCoroutines();
                StartCoroutine(DelayedGameObjectSelect(indexButton.gameObject));
            }
        }

        public void LockOnSelectedData()
        {
            m_lockOnSelectedData = true;
        }

        public void UnlockOnSelectedData()
        {
            m_lockOnSelectedData = false;
        }

        private IEnumerator DelayedGameObjectSelect(GameObject selectedGameObject)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(selectedGameObject);
        }

        private void Awake()
        {
            m_indexHandle.PageChange += OnPageChange;
        }

        private void OnPageChange(object sender, EventActionArgs eventArgs)
        {
            if (m_lockOnSelectedData)
            {
                for (int i = 0; i < m_indexHandle.buttonCount; i++)
                {
                    var button = m_indexHandle.GetButton(i);
                    if (button.data == m_selectedBestiaryData)
                    {
                        button.SetState(Doozy.Runtime.UIManager.UISelectionState.Pressed);
                        StopAllCoroutines();
                        StartCoroutine(DelayedGameObjectSelect(button.gameObject));
                    }
                    else
                    {
                        button.SetState(Doozy.Runtime.UIManager.UISelectionState.Normal);
                    }
                }
            }
        }
    }
}