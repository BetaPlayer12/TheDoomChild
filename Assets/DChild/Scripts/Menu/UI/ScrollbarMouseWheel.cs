using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DChild.Menu
{
    /// <summary>
    /// Forwards wheel input from a UI region to a scrollbar without a ScrollRect.
    /// Attach to the scrollbar or a shared parent of its raycastable content.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("DChild/UI/Scrollbar Mouse Wheel")]
    public class ScrollbarMouseWheel : MonoBehaviour, IScrollHandler, IPointerExitHandler
    {
        [SerializeField, Tooltip("Assign a Doozy UIScrollbar or a Unity UI Scrollbar.")]
        private Selectable m_scrollbar;

        [SerializeField, Min(0.01f), Tooltip("Scroll event units per step. Use 6 for a typical Windows wheel with Input System 1.4; use 1 for legacy UI input.")]
        private float m_scrollUnitsPerStep = 1f;

        [SerializeField, Range(0.001f, 1f), Tooltip("Normalized movement per step when the scrollbar has no discrete steps.")]
        private float m_continuousStep = 0.1f;

        [SerializeField, Tooltip("Reverse the wheel direction relative to the scrollbar's direction.")]
        private bool m_invertDirection;

        [SerializeField, Min(0), Tooltip("0 uses the scrollbar's step count. 1 disables scrolling. A higher count overrides wheel stepping for paged content.")]
        private int m_stepCountOverride;

        private float m_scrollRemainder;
        private float m_lastValue;

        public void SetScrollbar(Selectable scrollbar)
        {
            m_scrollbar = scrollbar;
            m_scrollRemainder = 0f;
        }

        public void SetStepCount(int stepCount)
        {
            m_stepCountOverride = Mathf.Max(0, stepCount);
            m_scrollRemainder = 0f;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!isActiveAndEnabled || eventData.used || m_scrollbar == null ||
                !m_scrollbar.IsActive() || !m_scrollbar.IsInteractable())
            {
                m_scrollRemainder = 0f;
                return;
            }

            var doozyScrollbar = m_scrollbar as UIScrollbar;
            var unityScrollbar = m_scrollbar as Scrollbar;
            if (doozyScrollbar == null && unityScrollbar == null)
                return;

            float value = doozyScrollbar != null ? doozyScrollbar.value : unityScrollbar.value;
            float size = doozyScrollbar != null ? doozyScrollbar.size : unityScrollbar.size;
            int steps = m_stepCountOverride > 0 ? m_stepCountOverride :
                (doozyScrollbar != null ? doozyScrollbar.numberOfSteps : unityScrollbar.numberOfSteps);

            if (steps == 1 || size >= 1f)
            {
                m_scrollRemainder = 0f;
                return;
            }

            bool increasesDown = doozyScrollbar != null
                ? doozyScrollbar.direction == SlideDirection.TopToBottom || doozyScrollbar.direction == SlideDirection.LeftToRight
                : unityScrollbar.direction == Scrollbar.Direction.TopToBottom || unityScrollbar.direction == Scrollbar.Direction.LeftToRight;

            // Treat horizontal trackpad movement to the right like a downward wheel movement.
            float delta = Mathf.Abs(eventData.scrollDelta.y) >= Mathf.Abs(eventData.scrollDelta.x)
                ? eventData.scrollDelta.y : -eventData.scrollDelta.x;
            delta *= increasesDown ? -1f : 1f;
            if (m_invertDirection)
                delta = -delta;
            if (Mathf.Approximately(delta, 0f))
                return;

            eventData.Use();
            if (!Mathf.Approximately(value, m_lastValue) || m_scrollRemainder * delta < 0f)
                m_scrollRemainder = 0f;
            m_lastValue = value;

            // Do not accumulate overscroll at either end; reversing must respond immediately.
            if ((value <= 0f && delta < 0f) || (value >= 1f && delta > 0f))
            {
                m_scrollRemainder = 0f;
                return;
            }

            float movement = delta / Mathf.Max(0.01f, m_scrollUnitsPerStep);
            if (steps > 1)
            {
                m_scrollRemainder += movement;
                int wholeSteps = (int)m_scrollRemainder;
                if (wholeSteps == 0)
                    return;
                m_scrollRemainder -= wholeSteps;
                int currentStep = Mathf.RoundToInt(value * (steps - 1));
                int nextStep = Mathf.Clamp(currentStep + wholeSteps, 0, steps - 1);
                value = nextStep / (float)(steps - 1);
            }
            else
            {
                value = Mathf.Clamp01(value + movement * m_continuousStep);
            }

            if (doozyScrollbar != null)
                doozyScrollbar.value = value;
            else
                unityScrollbar.value = value;

            m_lastValue = doozyScrollbar != null ? doozyScrollbar.value : unityScrollbar.value;
            if (m_lastValue <= 0f || m_lastValue >= 1f)
                m_scrollRemainder = 0f;
        }

        public void OnPointerExit(PointerEventData eventData) => m_scrollRemainder = 0f;

        private void OnDisable() => m_scrollRemainder = 0f;

        private void Reset()
        {
            m_scrollbar = GetComponent<UIScrollbar>();
            if (m_scrollbar == null)
                m_scrollbar = GetComponent<Scrollbar>();
        }
    }
}
