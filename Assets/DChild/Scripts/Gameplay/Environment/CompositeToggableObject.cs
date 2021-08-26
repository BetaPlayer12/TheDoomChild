using DChild.Serialization;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CompositeToggableObject : ToggableObject
    {
        [SerializeField, TabGroup("Inline")]
        private ToggableObject[] m_toggables;
        [SerializeField, TabGroup("Inverted")]
        private ToggableObject[] m_invertedToggables;
        public override void SetToggleState(bool value)
        {
            base.SetToggleState(value);
            SetToggablesStateTo(m_toggables, value);
            SetToggablesStateTo(m_invertedToggables, !value);
        }

        private void SetToggablesStateTo(ToggableObject[] toggables, bool value)
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {

            var currentState = Application.isPlaying ? m_currentState : m_startAs;
            DrawGizmos(currentState);
        }

        public void DrawGizmos(bool currentToggleState)
        {
            var onColor = Color.green;
            var offColor = Color.red;

            var currentPosition = transform.position;
            DrawGizmos(m_toggables, currentToggleState ? onColor : offColor);
            DrawGizmos(m_invertedToggables, !currentToggleState ? onColor : offColor);

            void DrawGizmos(ToggableObject[] toggables, Color color)
            {
                var cubeSize = new Vector3(2, 2, 1);
                Gizmos.color = color;
                for (int i = 0; i < toggables.Length; i++)
                {
                    var toggable = toggables[i];
                    var position = toggable.transform.position;
                    Gizmos.DrawLine(currentPosition, position);
                    if (toggable.GetType() == this.GetType())
                    {
                        ((CompositeToggableObject)toggable).DrawGizmos(currentToggleState);
                        Gizmos.color = color;
                        Gizmos.DrawSphere(position, 2);
                    }
                    else
                    {

                        Gizmos.color = color;
                        Gizmos.DrawCube(position, cubeSize);
                    }
                }
            }
        }
#endif
    }
}