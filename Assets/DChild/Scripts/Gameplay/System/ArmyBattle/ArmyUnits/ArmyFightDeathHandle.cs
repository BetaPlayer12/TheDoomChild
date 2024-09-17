using DChild.Gameplay.ArmyBattle.Battalion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{
    [System.Serializable]
    public class ArmyFightDeathHandle
    {
        [System.Serializable]
        private struct DeathOrderInfo
        {
            public float time;
            public bool isPlayerUnit;

            public DeathOrderInfo(float time, bool isPlayerUnit)
            {
                this.time = time;
                this.isPlayerUnit = isPlayerUnit;
            }
        }

        [SerializeField, MinMaxSlider(0, 1, true)]
        private Vector2 m_deathInFightOccurancePercent;
        [SerializeField, MinMaxSlider(1, 100, true)]
        private Vector2Int m_deathInFightInstanceRange;

        [SerializeField, MinValue(1)]
        private int m_playerTroopPerUnit;
        [SerializeField, MinValue(1)]
        private int m_enemyTroopPerUnit;

        private List<DeathOrderInfo> m_deathOrder;

        private Vector2 m_deathInFightOccuranceRange;

        private int m_nextPlayerUnitTypeToKillIndex;
        private int m_nextEnemyUnitTypeToKillIndex;

        private float deathInFightDuration => m_deathInFightOccuranceRange.y - m_deathInFightOccuranceRange.x;

        public void Initialize(float fightDuration)
        {
            m_deathOrder = new List<DeathOrderInfo>();
            m_deathInFightOccuranceRange = new Vector2(fightDuration * m_deathInFightOccurancePercent.x, fightDuration * m_deathInFightOccurancePercent.y);

            m_nextPlayerUnitTypeToKillIndex = Random.Range(0, (int)DamageType._COUNT);
            m_nextEnemyUnitTypeToKillIndex = Random.Range(0, (int)DamageType._COUNT);
        }

        public IEnumerator DyingInBattleRoutine(ArmyBattalionManager player, ArmyBattalionManager enemy, ArmyBattleCombatResult result)
        {
            m_deathOrder.Clear();
            var playerUnitsToKillOff = CalculateUnitsToKillOff(player, result.player.remainingTroopCount, m_playerTroopPerUnit);
            ScheduleDeaths(playerUnitsToKillOff, true);
            var enemyUnitsToKillOff = CalculateUnitsToKillOff(enemy, result.enemy.remainingTroopCount, m_enemyTroopPerUnit);
            ScheduleDeaths(enemyUnitsToKillOff, false);
            m_deathOrder = m_deathOrder.OrderBy(x => x.time).ToList();

            yield return new WaitForSeconds(m_deathInFightOccuranceRange.x); //Wait Untill units can die

            for (int i = 0; i < m_deathOrder.Count; i++)
            {
                var deathOrder = m_deathOrder[i];
                if (i == 0)
                {
                    yield return new WaitForSeconds(deathOrder.time);
                }
                else
                {
                    var nextDeathTimeWait = deathOrder.time - m_deathOrder[i - 1].time;
                    yield return new WaitForSeconds(nextDeathTimeWait);
                }

                if (deathOrder.isPlayerUnit)
                {
                    KillOffUnit(player, ref m_nextPlayerUnitTypeToKillIndex);
                }
                else
                {
                    KillOffUnit(enemy, ref m_nextEnemyUnitTypeToKillIndex);
                }
            }

        }

        private void KillOffUnit(ArmyBattalionManager battalion, ref int index)
        {
            var unitHandle = battalion.GetUnitHandle((DamageType)index);
            if (unitHandle.GetUnitCount() > 1)
            {
                unitHandle.KillOffUnits(1);
            }
            else
            {
                index += 1;
                index = (int)Mathf.Repeat(index, ((int)DamageType._COUNT));
                KillOffUnit(battalion, ref index);
            }

            index += 1;
            index = (int)Mathf.Repeat(index, ((int)DamageType._COUNT));
        }

        private void ScheduleDeaths(int unitsToKillOff, bool ofPlayerBattalion)
        {
            var unitDeathInterval = deathInFightDuration / unitsToKillOff;
            for (int i = 0; i < unitsToKillOff; i++)
            {
                var timeOfDeath = (unitDeathInterval * i) + Random.Range(0, unitDeathInterval);
                m_deathOrder.Add(new DeathOrderInfo(timeOfDeath, ofPlayerBattalion));
            }
        }

        private int CalculateUnitsToKillOff(ArmyBattalionManager battalion, int remainingTroopCount, int troopPerUnit)
        {
            var numberOfUnitsToLive = Mathf.CeilToInt(remainingTroopCount / troopPerUnit);
            var currentLiveUnits = battalion.GetTotalUnitCount();
            var unitsToKillOff = currentLiveUnits - numberOfUnitsToLive;
            return unitsToKillOff;
        }
    }
}