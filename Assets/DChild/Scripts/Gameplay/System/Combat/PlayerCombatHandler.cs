using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class PlayerCombatHandler : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float m_lowestDamageReceivePercentage = 0.3f;

        [SerializeField]
        private float m_knockBackPower;
        [SerializeField, BoxGroup("IFrame")]
        private float m_invulnerabilityDuration;
        [SerializeField, BoxGroup("IFrame")]
        private float m_inputDisableDuration;

        private WaitForWorldSeconds m_hitboxDisableWait;
        private WaitForWorldSeconds m_inputDisableWait;

        public void ResolveDamageRecieved(IPlayerCombat player, Vector2 damageSource)
        {
            if (((IPlayer)player).characterState.canFlinch)
            {
                if (damageSource.x > player.position.x)
                {
                    player.Displace(new Vector2(-1, 1) * m_knockBackPower);
                }
                else
                {
                    player.Displace(Vector2.one * m_knockBackPower);
                }
                StartCoroutine(DisableInputTemporarily(player));
            }
            StartCoroutine(TemporaryInvulnerability(player));
        }

        public void FactorDefense(IPlayerCombat player, ref DamageInfo info)
        {
            if (AttackDamage.IsMagicAttack(info.damageType))
            {
                info.damage -= player.magicDefense;
            }
            else
            {
                info.damage -= player.defense;
            }
        }

        private int ReduceDamageByDefense(int damage, int defense)
        {
            var result = damage - defense;
            var lowestDamage = Mathf.CeilToInt(damage * m_lowestDamageReceivePercentage);
            return result < lowestDamage ? lowestDamage : result;
        }

        private int CalculateMagicDamageReduction(IPlayerCombat player, System.Collections.Generic.List<AttackType> attackerDamageType)
        {
            int magicDefense = 0;
            var numberOfMagicAttackType = (attackerDamageType.Contains(AttackType.Physical) ? attackerDamageType.Count - 1 : attackerDamageType.Count);
            if (numberOfMagicAttackType > 0)
            {
                magicDefense = (int)(player.magicDefense / numberOfMagicAttackType);
            }

            return magicDefense;
        }

        private IEnumerator TemporaryInvulnerability(IPlayerCombat player)
        {
            player.BecomeInvulnerable(true);
            yield return m_hitboxDisableWait;
            player.BecomeInvulnerable(false);
        }

        private IEnumerator DisableInputTemporarily(IPlayerCombat player)
        {
            player.DisableController();
            yield return m_inputDisableWait;
            player.EnableController();
        }

        private void Awake()
        {
            m_hitboxDisableWait = new WaitForWorldSeconds(m_invulnerabilityDuration);
            m_inputDisableWait = new WaitForWorldSeconds(m_inputDisableDuration);
        }

#if UNITY_EDITOR
        public void Initialize(float knockbackPower, float hitboxDisableDuration, float inputDisableDuration)
        {
            m_knockBackPower = knockbackPower;
            m_invulnerabilityDuration = hitboxDisableDuration;
            m_inputDisableDuration = inputDisableDuration;
        }
#endif
    }
}