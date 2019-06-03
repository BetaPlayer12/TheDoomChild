using UnityEngine;

namespace DChild
{
    public class LocationBackdropComponent : MonoBehaviour, IGameDataComponent
    {
        [SerializeField]
        private LocationBackdrop m_data;

        public IGameData data => m_data;
    }
}