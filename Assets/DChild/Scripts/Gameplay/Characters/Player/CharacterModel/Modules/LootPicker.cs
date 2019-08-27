using DChild.Gameplay.Pooling;
using DChild.Gameplay.SoulEssence;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LootPicker : MonoBehaviour
    {
        private IPlayer m_owner;

        private void Start()
        {
            m_owner = GetComponentInParent<PlayerControlledObject>().owner;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var loot = collision.GetComponentInParent<Loot>();
            if (loot)
            {
                loot.PickUp(m_owner);
            }
        }
    }

}