using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class Door : MonoBehaviour, ISerializableComponent, ILerpHandling
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
            private Transform m_doorPanel;
            [SerializeField, HorizontalGroup("Open"), ShowIf("m_doorPanel")]
            private Vector2 m_openPosition;
            [SerializeField, HorizontalGroup("Close"), ShowIf("m_doorPanel")]
            private Vector2 m_closePosition;

            private Vector2 m_start;
            private Vector2 m_destination;

            public void SetLerpAs(bool open)
            {
                if (open)
                {
                    SetMoveValues(m_doorPanel.localPosition, m_openPosition);
                }
                else
                {
                    SetMoveValues(m_doorPanel.localPosition, m_closePosition);
                }
            }

            private void SetMoveValues(Vector2 start, Vector2 destination)
            {
                m_start = start;
                m_destination = destination;
            }

            public void Lerp(float lerpValue)
            {
                m_doorPanel.localPosition = Vector2.Lerp(m_start, m_destination, lerpValue);
            }

            public void SetAs(bool isOpen)
            {
                m_doorPanel.localPosition = isOpen ? m_openPosition : m_closePosition;
            }

#if UNITY_EDITOR


            [ResponsiveButtonGroup("Open/Button"), Button("Use Current"), ShowIf("m_doorPanel")]
            private void UseCurrentForOpenPosition()
            {
                m_openPosition = m_doorPanel.localPosition;
            }

            [ResponsiveButtonGroup("Close/Button"), Button("Use Current"), ShowIf("m_doorPanel")]
            private void UseCurrentForClosePosition()
            {
                m_closePosition = m_doorPanel.localPosition;
            }
#endif
        }

        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private DoorPanel[] m_panels;
        [SerializeField]
        private AnimationCurve m_speed;
        [SerializeField, OnValueChanged("OnStateChange")]
        private bool m_isOpen;

        private Collider2DGroup m_collider2DGroup;
        private Animator m_animator;
        private float m_animationTime;

        [Button, HideIf("m_isOpen"), HideInEditorMode]
        public void Open()
        {
            if(m_isOpen == false)
            {
                m_isOpen = true;
                enabled = false;
                m_animator.SetTrigger("Shake");
                for (int i = 0; i < m_panels.Length; i++)
                {
                    m_panels[i].SetLerpAs(true);
                }
            }
        }

        [Button, ShowIf("m_isOpen"), HideInEditorMode]
        public void Close()
        {
            if(m_isOpen == true)
            {
                m_isOpen = false;
                enabled = false;
                m_animator.SetTrigger("Shake");
                for (int i = 0; i < m_panels.Length; i++)
                {
                    m_panels[i].SetLerpAs(false);
                }
                m_collider2DGroup.EnableColliders();
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
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
                m_collider2DGroup = GetComponent<Collider2DGroup>();
            }
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].SetAs(open);
            }
            if (open)
            {
                m_collider2DGroup.DisableColliders();
            }
            else
            {
                m_collider2DGroup.EnableColliders();
            }
            m_isOpen = open;
        }

        public void StartInterpolation()
        {
            enabled = true;
            m_animationTime = 0;
        }

        public void SetLerpValue(float lerpValue)
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].Lerp(lerpValue);
            }
        }


        public virtual void Load(ISaveData data) => SetAsOpen(((SaveData)data).isOpen);

        public ISaveData Save() => new SaveData(m_isOpen);

        private void Awake()
        {
            SetAsOpen(m_isOpen);
            enabled = false;
        }

        private void LateUpdate()
        {
            m_animationTime += GameplaySystem.time.deltaTime;
            var lerpValue = m_speed.Evaluate(m_animationTime);
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].Lerp(lerpValue);
            }
            if (lerpValue >= 1)
            {
                if (m_isOpen)
                {
                    m_collider2DGroup.DisableColliders();
                }
                enabled = false;
            }
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
                m_collider2DGroup.DisableColliders();
            }
            else
            {
                m_collider2DGroup.EnableColliders();
            }
        }


#endif
    }

}