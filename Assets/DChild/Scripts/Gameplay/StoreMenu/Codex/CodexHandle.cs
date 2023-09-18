using Holysoft.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Menu.Codex
{
    public abstract class CodexHandle<DatabaseAssetType> : MonoBehaviour where DatabaseAssetType : DatabaseAsset
    {
        [SerializeReference]
        protected CodexIndexHandle<DatabaseAssetType> m_indexHandle;

        private DatabaseAssetType m_selectedData;
        private bool m_lockOnSelectedData;

        public abstract void Select(CodexIndexButton<DatabaseAssetType> indexButton);

        public void LockOnSelectedData()
        {
            m_lockOnSelectedData = true;
        }

        public void UnlockOnSelectedData()
        {
            m_lockOnSelectedData = false;
        }

        public void HighlightSelectedData()
        {
            if (m_lockOnSelectedData)
            {
                bool hasSelectedButton = false;
                for (int i = 0; i < m_indexHandle.buttonCount; i++)
                {
                    var button = m_indexHandle.GetButton(i);
                    if (button.data == m_selectedData)
                    {
                        Debug.Log("Selecting Codex Entry of " + button.data);
                        button.SetIsOn(true);
                        StopAllCoroutines();
                        StartCoroutine(DelayedGameObjectSelect(button.gameObject));
                        hasSelectedButton = true;
                    }
                    else
                    {
                        button.SetIsOn(false);
                    }
                }

                if (hasSelectedButton == false)
                {
                    StopAllCoroutines();
                }
            }
        }

        protected IEnumerator DelayedGameObjectSelect(GameObject selectedGameObject)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(selectedGameObject);
        }

        private void OnPageChange(object sender, EventActionArgs eventArgs)
        {
            HighlightSelectedData();
        }

        private void Awake()
        {
            m_indexHandle.PageChange += OnPageChange;
        }
    }


    public abstract class CodexHandle<DatabaseAssetType, IndexInfoType> : CodexHandle<DatabaseAssetType> where DatabaseAssetType : DatabaseAsset, IndexInfoType
                                                                                                         where IndexInfoType : class
    {
        [SerializeReference]
        private CodexInfoUI<IndexInfoType> m_infoPage;

        private DatabaseAssetType m_selectedData;
        private bool m_lockOnSelectedData;

        public override void Select(CodexIndexButton<DatabaseAssetType> indexButton)
        {
            if (m_lockOnSelectedData == false)
            {
                if (indexButton.isAvailable)
                {
                    m_selectedData = indexButton.data;
                    m_infoPage.ShowInfo(m_selectedData);
                    indexButton.SetIsOn(true);
                    StopAllCoroutines();
                    StartCoroutine(DelayedGameObjectSelect(indexButton.gameObject));
                }
                else
                {
                    m_infoPage.ShowInfo(null);
                }
            }
        }
    }
}