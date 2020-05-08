﻿using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class PlayerCombatHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_hitFX;
        [SerializeField, BoxGroup("IFrame")]
        private float m_invulnerabilityDuration;
        [SerializeField, BoxGroup("IFrame")]
        private float m_inputDisableDuration;

        private HitStopHandle m_hitStop;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void ResolveDamageRecieved(IPlayer player)
        {
            if (player.state?.canFlinch ?? true)
            {
                if (m_hitFX)
                {
                    m_spawnHandle.InstantiateFX(m_hitFX, player.character.centerMass.position);
                    m_hitStop.Execute();
                }
                StartCoroutine(DisableInputTemporarily(player));
            }
            StartCoroutine(TemporaryInvulnerability(player));
        }

        //private int CalculateMagicDamageReduction(IPlayer player, System.Collections.Generic.List<AttackType> attackerDamageType)
        //{
        //    int magicDefense = 0;
        //    var numberOfMagicAttackType = (attackerDamageType.Contains(AttackType.Physical) ? attackerDamageType.Count - 1 : attackerDamageType.Count);
        //    if (numberOfMagicAttackType > 0)
        //    {
        //        magicDefense = (int)(player.magicDefense / numberOfMagicAttackType);
        //    }

        //    return magicDefense;
        //}

        private IEnumerator TemporaryInvulnerability(IPlayer player)
        {
            player.damageableModule.SetInvulnerability(true);
            yield return new WaitForWorldSeconds(m_invulnerabilityDuration);
            player.damageableModule.SetInvulnerability(false);
        }

        private IEnumerator DisableInputTemporarily(IPlayer player)
        {
            player.controller.Disable();
            yield return new WaitForWorldSeconds(m_inputDisableDuration);
            player.controller.Enable();
        }

        private void Awake()
        {
            m_hitStop = GetComponent<HitStopHandle>();
        }
    }
}