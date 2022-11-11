using DChild.UI;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChildDebug.Cutscene
{
    public class SequenceSkipHandle : MonoBehaviour
    {
        [SerializeField]
        private DialogueSkipProgressionUI m_ui;
        [SerializeField]
        private InputActionReference m_input;
        [SerializeField]
        private float m_holdButtonDuration;

        private UIContainer m_view;
        private float m_holdButtonDurationTimer;

        public static event Action SkipExecute;

        public void Reset()
        {
            m_holdButtonDurationTimer = 0;
            m_ui.SetProgression(0);
        }

        public void SubscribeToInput()
        {
            m_input.action.started += OnSkipHoldStart;
            m_input.action.canceled += OnSkipHoldCanceled;
        }

        public void UnsubscribeToInput()
        {
            m_input.action.started -= OnSkipHoldStart;
            m_input.action.canceled -= OnSkipHoldCanceled;
        }

        private void OnSkipHoldStart(InputAction.CallbackContext obj)
        {
            StopAllCoroutines();
            StartCoroutine(SkipRoutine());
        }

        private void OnSkipHoldCanceled(InputAction.CallbackContext obj)
        {
            Debug.Log("Skip Routine Canceled");
            StopAllCoroutines();
        }

        private IEnumerator SkipRoutine()
        {
            Debug.Log("Skip Routine Start");
            Reset();
            yield return null;
            do
            {
                m_holdButtonDurationTimer += Time.unscaledDeltaTime;
                m_ui.SetProgression(m_holdButtonDurationTimer / m_holdButtonDuration);
                yield return null;
            } while (m_holdButtonDurationTimer < m_holdButtonDuration);
            SkipSequence();
        }

        [Button]
        private void SkipSequence()
        {
            m_view.Hide();
            SkipExecute?.Invoke();
        }

        private void Awake()
        {
            m_view = GetComponent<UIContainer>();
        }
    }
}
