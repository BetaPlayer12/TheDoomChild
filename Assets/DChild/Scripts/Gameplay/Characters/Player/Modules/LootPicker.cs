using DChild.Gameplay.Pooling;
using DChild.Gameplay.SoulEssence;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LootPicker : MonoBehaviour, IComplexCharacterModule
    {
        private IPlayer m_owner;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_owner = info.character.GetComponent<PlayerControlledObject>().owner;
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