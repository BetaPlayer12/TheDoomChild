using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems.Serialization;
using UnityEngine;

namespace DChild.Gameplay.Systems
{

    public class LocationSwitcher : MonoBehaviour
    {
       
        [SerializeField]
        private LocationData m_destination;
        [SerializeField]
        private TravelDirection m_entranceDirection;

        public void GoToDestination(Character agent)
        {
            GameplaySystem.MovePlayerToLocation(agent, m_destination, m_entranceDirection);


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerControlledObject = collision.GetComponentInParent<PlayerControlledObject>();
            if(playerControlledObject != null)
            {
                GoToDestination(playerControlledObject.GetComponent<Character>());
            }
        }
    }
}
