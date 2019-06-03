using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Quick Fix for an issue with unity's Canvas Group in 2018.3
/// </summary>
namespace Holysoft.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [ExecuteAlways]
    public class CanvasGroupFadeHandler : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_mainGroup;
        [SerializeField]
        private CanvasGroup[] m_exclusiveCanvasGroups;
        private float m_prevAlpha;

        private void UpdateAlphas()
        {
            m_prevAlpha = m_mainGroup.alpha;
            for (int i = 0; i < (m_exclusiveCanvasGroups?.Length ?? 0); i++)
            {
                m_exclusiveCanvasGroups[i].alpha = m_prevAlpha;
            }
        }

        private void Start()
        {
            UpdateAlphas();
        }
     
        private void Update()
        {
            if (m_mainGroup.alpha != m_prevAlpha)
            {
                UpdateAlphas();
            }
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_mainGroup);
            var canvasGroups = new List<CanvasGroup>(GetComponentsInChildren<CanvasGroup>());
            canvasGroups.Remove(m_mainGroup);
            canvasGroups.RemoveAll(x => x.ignoreParentGroups);
            m_exclusiveCanvasGroups = canvasGroups.ToArray();
        }
    }

}