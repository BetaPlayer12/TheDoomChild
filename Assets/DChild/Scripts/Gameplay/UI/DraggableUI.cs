using UnityEngine;

namespace DChild.UI
{
    public class DraggableUI : MonoBehaviour
    {
        private Vector2 m_origin;

        public void FollowMousePosition()
        {
            transform.position = Input.mousePosition;
        }

        public void ReturnToOrigin()
        {
            transform.localPosition = m_origin;
        }

        public void SetOrigin(Vector2 origin)
        {
            m_origin = origin;
        }

        private void Awake()
        {
            m_origin = transform.localPosition;
        }
    } 
}
