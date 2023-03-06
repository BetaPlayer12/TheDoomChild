using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using UnityEngine;
using System.Collections;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LootPicker : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;
        private SoulGathererEffectsHandler effect;
        private IPlayer m_owner;
        private Coroutine m_glowRoutine;

        public void Glow()
        {

            m_animator.SetTrigger("Glow");
            if (effect)
            {
                effect.StopEffect();
            }
        }

        private void Start()
        {
            m_owner = GetComponentInParent<PlayerControlledObject>().owner;
           
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var loot = collision.GetComponentInParent<Loot>();
            if (loot)
            {
                effect = null;
                 effect = FindObjectOfType<SoulGathererEffectsHandler>();
                if (effect)
                {
                    effect.PlayEffect();
                }
                loot.PickUp(m_owner);
            }
        }
    }
}