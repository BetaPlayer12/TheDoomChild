using UnityEngine;
using UnityEngine.UIElements;

namespace DChild.Gameplay.UI.Map
{

    public class MapController : MonoBehaviour
    {
        [SerializeField]
        private MapMover m_mover;
        [SerializeField]
        private MapScaler m_scaler;

        private void LateUpdate()
        {
            m_mover.Update();
            m_scaler.Update();
        }
    }
}