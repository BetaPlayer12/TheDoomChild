using DChild.Gameplay.Systems.Serialization;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class LocationSwitcher : MonoBehaviour
    {
        [SerializeField]
        private LocationData m_destination;

        public void GoToDestination(Character agent)
        {
            if (GameSystem.IsCurrentZone(m_destination.scene) == false)
            {
                GameSystem.LoadZone(m_destination.scene, true);
            }
            agent.transform.position = m_destination.position;
        }
    }
}
