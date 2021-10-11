using System;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public struct DamageCalculator
    {
        [Flags]
        public enum Operations
        {
            CriticalDamage = 1<<0,
            DamageResistance = 1 << 0,
            BlockDamage = 1 << 0,

            All = CriticalDamage|DamageResistance| BlockDamage
        }

        [SerializeField]
        private CriticalDamageHandle m_critHandle;
        private DamageResistanceHandle m_resistanceHandle;
        private DamageBlockHandle m_blockHandle;

        public AttackSummaryInfo CalculateDamage(AttackDamageInfo attackInfo, IAttackResistance targetResistance, bool targetCanBlock, Operations operationFlags)
        {
            var summaryInfo = new AttackSummaryInfo(attackInfo.damage);

            if (operationFlags.HasFlag(Operations.BlockDamage))
            {
                summaryInfo = HandleDamageBlock(attackInfo.ignoresBlock, targetCanBlock, summaryInfo);
            }

            if (summaryInfo.wasBlocked == false)
            {
                if (operationFlags.HasFlag(Operations.CriticalDamage))
                {
                    summaryInfo = HandleCritCalculation(attackInfo, summaryInfo);
                }

                if (operationFlags.HasFlag(Operations.DamageResistance) && targetResistance != null)
                {
                    summaryInfo = HandleDamageResistance(targetResistance, summaryInfo);
                }
            }

            return summaryInfo;
        }

        private AttackSummaryInfo HandleCritCalculation(AttackDamageInfo attackInfo, AttackSummaryInfo summaryInfo)
        {
            var criticalDamageInfo = attackInfo.criticalDamageInfo;
            if (m_critHandle.CheckForCrit(attackInfo.criticalDamageInfo))
            {
                summaryInfo.isCrit = true;
                summaryInfo.SetDamageInfo(m_critHandle.CalculateCriticalDamage(summaryInfo.damageInfo.damage, criticalDamageInfo));
            }
            else
            {
                summaryInfo.isCrit = false;
            }

            return summaryInfo;
        }

        private AttackSummaryInfo HandleDamageResistance(IAttackResistance resistance, AttackSummaryInfo result)
        {
            var resistedDamage = m_resistanceHandle.CalculatateResistedDamage(result.damageInfo.damage, resistance);
            result.SetDamageInfo(resistedDamage);
            return result;
        }

        private AttackSummaryInfo HandleDamageBlock(bool attackIgnoresBlock, bool targetCanBlock, AttackSummaryInfo result)
        {
            if (targetCanBlock && attackIgnoresBlock == false)
            {
                var damage = m_blockHandle.CalculateBlockedDamage(result.damageInfo.damage);
                result.SetDamageInfo(damage);
                result.wasBlocked = true;
            }
            else
            {
                result.wasBlocked = false;
            }
            return result;
        }
    }
}