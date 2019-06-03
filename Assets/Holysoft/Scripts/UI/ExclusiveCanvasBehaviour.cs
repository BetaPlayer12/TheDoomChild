using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{
    public class ExclusiveCanvasBehaviour : UICanvasComponent
    {
        [SerializeField]
        [ReadOnly]
        private UIBehaviour[] m_behaviours;

        protected override void OnHide(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_behaviours.Length; i++)
            {
                m_behaviours[i].Disable();
            }
        }

        protected override void OnShow(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_behaviours.Length; i++)
            {
                m_behaviours[i].Enable();
            }
        }

#if UNITY_EDITOR
        public bool HasBehaviour(UIBehaviour uICanvasBehaviour)
        {
            if (m_behaviours == null)
            {
                RecordExclusiveUICanvasBehaviour();
            }

            for (int i = 0; i < m_behaviours.Length; i++)
            {
                if (m_behaviours[i] == uICanvasBehaviour)
                {
                    return true;
                }
            }
            return false;
        }

        [Button("Record Exclusive Behaviours")]
        private void RecordExclusiveUICanvasBehaviour()
        {
            List<UIBehaviour> behaviours = new List<UIBehaviour>(GetComponentsInChildren<UIBehaviour>());
            var childCanvas = GetComponentsInChildren<ExclusiveCanvasBehaviour>();
            if (childCanvas == null)
            {
                m_behaviours = behaviours.ToArray();
            }
            else
            {
                for (int i = behaviours.Count - 1; i >= 0; i--)
                {
                    var toCheck = behaviours[i];
                    for (int j = 0; j < childCanvas.Length; j++)
                    {
                        if (childCanvas[j].HasBehaviour(toCheck))
                        {
                            behaviours.RemoveAt(i);
                            break;
                        }
                    }
                }

                m_behaviours = behaviours.ToArray();
            }
        }

        [Button("Clear Exclusive Behaviours")]
        private void ClearExclusiveUICanvasBehaviour()
        {
            m_behaviours = null;
        }
#endif

        private void OnValidate()
        {
#if UNITY_EDITOR
            RecordExclusiveUICanvasBehaviour();
#endif
        }

    }
}