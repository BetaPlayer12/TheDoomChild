using DChild.Serialization;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CompositeToggableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_currentState;

            public SaveData(bool currentState)
            {
                m_currentState = currentState;
            }

            public bool currentState => m_currentState;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_currentState);
        }

        [SerializeField, HideInPlayMode]
        private bool m_startAs;
        [ShowInInspector, HideInEditorMode, OnValueChanged("ToggleState")]
        private bool m_currentState;
        [SerializeField,TabGroup("Inline")]
        private ToggableObject[] m_toggables;
        [SerializeField, TabGroup("Inverted")]
        private ToggableObject[] m_invertedToggables;

        private bool m_hasBeenInitialize;

        [Button, HideInEditorMode]
        public void ToggleState()
        {
            SetToggleState(!m_currentState);
        }

        public void Load(ISaveData data)
        {
            SetToggleState(((SaveData)data).currentState);
            m_hasBeenInitialize = true;
        }

        public ISaveData Save() => new SaveData(m_currentState);

        public void SetToggleState(bool value)
        {
            m_currentState = value;
            SetToggablesStateTo(m_toggables, value);
            SetToggablesStateTo(m_invertedToggables, !value);
            m_hasBeenInitialize = true;
        }

        private void SetToggablesStateTo(ToggableObject[] toggables,bool value)
        {
            for (int i = 0; i < toggables.Length; i++)
            {
                try
                {
                    toggables[i].SetToggleState(value);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message, toggables[i]);
                }
            }
        }

        private void Awake()
        {
            if (m_hasBeenInitialize == false)
            {
                SetToggleState(m_startAs);
                m_hasBeenInitialize = true;
            }
        }
        private void OnDrawGizmosSelected()
        {
            var onColor = Color.green;
            var offColor = Color.red;

            var currentState = Application.isPlaying ? m_currentState : m_startAs;

            var currentPosition = transform.position;
            DrawGizmos(m_toggables, currentState ? onColor : offColor);
            DrawGizmos(m_invertedToggables, !currentState ? onColor : offColor);

            void DrawGizmos(ToggableObject[] toggables, Color color)
            {
                var cubeSize = new Vector3(2, 2, 1);
                Gizmos.color = color;
                for (int i = 0; i < toggables.Length; i++)
                {
                    var position = toggables[i].transform.position;
                    Gizmos.DrawLine(currentPosition, position);
                    Gizmos.DrawCube(position, cubeSize);
                }
            }
        }
    }
}