using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
        [SerializeField, Min(1)]
        private int m_value;

        protected override void ApplyPickUp(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
            CallPoolRequest();
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.tag != "Sensor" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //    {
        //        m_pickedBy.inventory.AddSoulEssence(m_value);
        //        CallPoolRequest();
        //    }
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    if (collision.tag != "Sensor" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //    {
        //        m_pickedBy.inventory.AddSoulEssence(m_value);
        //        CallPoolRequest();
        //    }
        //}
    }
}