using DChild.UI;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChildDebug.Cutscene
{
    public class SequenceSkipHandle : MonoBehaviour
    {
        [SerializeField]
        private DialogueSkipProgressionUI m_ui;
        [SerializeField]
        private float m_holdButtonDuration;

        private UIContainer m_view;
        private float m_holdButtonDurationTimer;

        public static event Action SkipExecute;

        private void Awake()
        {
            m_view = GetComponent<UIContainer>();
        }

        private void OnEnable()
        {
            m_holdButtonDurationTimer = 0;
            m_ui.SetProgression(0);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_holdButtonDurationTimer += Time.unscaledDeltaTime;
                m_ui.SetProgression(m_holdButtonDurationTimer / m_holdButtonDuration);
                if (m_holdButtonDurationTimer >= m_holdButtonDuration)
                {
                    SkipSequence();
                    enabled = false;
                }
            }
            else
            {
                m_holdButtonDurationTimer = 0;
                m_ui.SetProgression(0);
            }
        }

        [Button]
        private void SkipSequence()
        {
            m_view.Hide();
            SkipExecute?.Invoke();
        }
    }
}
