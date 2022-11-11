using UnityEngine;

namespace DChild
{
    public class CursorConfigurator : MonoBehaviour
    {
        [SerializeField]
        private bool m_makeCursorVisisbleOnStart;

        public void SetCursorVisibility(bool isVisible)
        {

            GameSystem.SetCursorVisibility(isVisible);
        }

        private void Start()
        {
            GameSystem.SetCursorVisibility(m_makeCursorVisisbleOnStart);
        }
    }

}