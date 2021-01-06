using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "ThrowableItemData", menuName = "DChild/Database/Throwable Item Data")]
    public class ThrowableItemData : ConsumableItemData
    {
        [SerializeField]
        private ProjectileInfo m_projectile;
        private static bool m_isOnCooldown;
        private static bool m_isInUse;

        public override bool CanBeUse(IPlayer player)
        {
            return m_isOnCooldown == false && player.state.isAttacking == false;
        }

        public override void Use(IPlayer player)
        {
            if (m_isInUse == false)
            {
                var handle = player.character.GetComponent<SkullThrow>();
                handle.SetProjectileInfo(m_projectile);
                if (player.state.isGrounded)
                {
                    handle.StartAim();
                    handle.Execute();
                    player.character.StartCoroutine(TrackInputRoutine(player, handle));
                }
                else
                {
                    handle.Execute();
                    handle.StartThrow();
                }
            }

            /*Start Throw via Controller
             Controller decides when to spawn the projectile
             this only starts the process*/
        }

        private IEnumerator TrackInputRoutine(IPlayer player, SkullThrow handle)
        {
            bool keepInLoop = true;
            while (keepInLoop)
            {
                yield return null;
                if (player.state.isGrounded)
                {
                    if (true) //KeyReleased
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
