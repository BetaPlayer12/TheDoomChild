using DChild.Serialization;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class AnimatedDoor : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isOpen)
            {
                this.m_isOpen = isOpen;
            }

            [SerializeField]
            private bool m_isOpen;

            public bool isOpen => m_isOpen;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isOpen);
        }

        [System.Serializable]
        public class DoorPanel
        {
            [SerializeField]
            private SkeletonAnimation m_doorPanel;
            [SerializeField, HorizontalGroup("Open"), ShowIf("m_doorPanel"),Spine.Unity.SpineAnimation(dataField = "m_doorPanel.skeletonDataAsset")]
            private string m_openAnimation;
            [SerializeField, HorizontalGroup("Close"), ShowIf("m_doorPanel"), Spine.Unity.SpineAnimation(dataField = "m_doorPanel.skeletonDataAsset")]
            private string m_closeAnimation;

            public void SetAs(bool isOpen)
            {
                var chosenAnimation = isOpen ? m_openAnimation : m_closeAnimation;
                m_doorPanel.state.SetAnimation(0, chosenAnimation, false);
            }
        }

        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private DoorPanel[] m_panels;

        [SerializeField, OnValueChanged("OnStateChange")]
        private bool m_isOpen;

        private Collider2DGroup m_collider2DGroup;

        public virtual void Load(ISaveData data) => SetAsOpen(((SaveData)data).isOpen);

        public ISaveData Save() => new SaveData(m_isOpen);

        public void Initialize()
        {
            m_isOpen = false;
            SetAsOpen(m_isOpen);
        }

        [Button, HideIf("m_isOpen"), HideInEditorMode]
        public void Open()
        {
            if (m_isOpen == false)
            {
                m_isOpen = true;
                for (int i = 0; i < m_panels.Length; i++)
                {
                    m_panels[i].SetAs(true);
                }
            }
        }

        [Button, ShowIf("m_isOpen"), HideInEditorMode]
        public void Close()
        {
            if (m_isOpen == true)
            {
                m_isOpen = false;

                for (int i = 0; i < m_panels.Length; i++)
                {
                    m_panels[i].SetAs(false);
                }
                m_collider2DGroup?.EnableColliders();
            }
        }

        public void ToggleState()
        {
            if (m_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void SetAsOpen(bool open)
        {
            if (m_collider2DGroup == null)
            {
                m_collider2DGroup = GetComponent<Collider2DGroup>();
            }
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].SetAs(open);
            }
            if (open)
            {
                m_collider2DGroup?.DisableColliders();
            }
            else
            {
                m_collider2DGroup?.EnableColliders();
            }
            m_isOpen = open;
        }

        private void Awake()
        {
            SetAsOpen(m_isOpen);
        }

#if UNITY_EDITOR
        private void OnStateChange()
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].SetAs(m_isOpen);
            }

            if (m_collider2DGroup == null)
            {
                m_collider2DGroup = GetComponent<Collider2DGroup>();
            }
            if (m_isOpen)
            {
                m_collider2DGroup?.DisableColliders();
            }
            else
            {
                m_collider2DGroup?.EnableColliders();
            }
        }
#endif
    }

}