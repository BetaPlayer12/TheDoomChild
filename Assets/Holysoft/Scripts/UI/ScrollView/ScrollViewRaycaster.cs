using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    [RequireComponent(typeof(ScrollViewNavigation))]
    public class ScrollViewRaycaster : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster m_raycaster;

        private void OnLerpStart(object sender, EventActionArgs eventArgs)
        {
            m_raycaster.enabled = false;
        }

        private void OnLerpEnd(object sender, EventActionArgs eventArgs)
        {
            m_raycaster.enabled = true;
        }

        private void Awake()
        {
            var navigation = GetComponent<ScrollViewNavigation>();
            navigation.LerpEnd += OnLerpEnd;
            navigation.LerpStart += OnLerpStart;
        }
    }
}