using UnityEngine;

namespace DChild
{
    public class CursorConfigurator : MonoBehaviour
    {
        [SerializeField]
        private bool m_makeCursorVisisble;

        public void SetCursorVisibility(bool isVisible)
        {
            GameSystem.SetCursorVisibility(m_makeCursorVisisble);
        }

        private void Start()
        {
            GameSystem.SetCursorVisibility(m_makeCursorVisisble);
        }
    }

}