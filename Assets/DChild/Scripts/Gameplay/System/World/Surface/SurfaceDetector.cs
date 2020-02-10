using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class SurfaceDetector : MonoBehaviour
    {
        public event EventAction<SurfaceDetectedEventArgs> NewSurfaceDetected;
        private SurfaceDetectedEventArgs m_eventArgs;
        private Collider2D m_previousCollider;

        private void Awake()
        {
            m_eventArgs = new SurfaceDetectedEventArgs();
            m_previousCollider = null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("SolidPlatform") && collision.collider != m_previousCollider)
            {
                var surface = collision.collider.GetComponent<Surface>();
                m_eventArgs.Set(GameplaySystem.databaseManager.GetDatabase<SurfaceData>().GetFXGroup(surface.type));
                NewSurfaceDetected?.Invoke(this, m_eventArgs);
                m_previousCollider = collision.collider;
            }
        }
    }
}