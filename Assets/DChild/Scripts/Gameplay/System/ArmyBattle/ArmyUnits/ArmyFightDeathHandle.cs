using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.ArmyBattle.Units;
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

        [System.Serializable]
        private class ArmyReference
        {
            private Army m_army;
            private ArmyBattalionManager m_battalion;

            public ArmyReference(Army army, ArmyBattalionManager battalion)
            {
                m_army = army;
                m_battalion = battalion;
                troopPerUnit = m_army.troopCount / m_battalion.GetTotalUnitCount();
            }

            public ArmyBattalionManager battalion => m_battalion;
            public int troopPerUnit { get; private set; }
            public bool HasAvailableGroup(DamageType damageType) => m_army.HasAvailableGroup(damageType);


        }

        [SerializeField, MinMaxSlider(0, 1, true)]
        private Vector2 m_deathInFightOccurancePercent;
        [SerializeField, MinMaxSlider(1, 100, true)]
        private Vector2Int m_deathInFightInstanceRange;


        private ArmyReference m_player;
        private ArmyReference m_enemy;

        private List<DeathOrderInfo> m_deathOrder;

        private Vector2 m_deathInFightOccuranceRange;

        private int m_nextPlayerUnitTypeToKillIndex;
        private int m_nextEnemyUnitTypeToKillIndex;

        private float deathInFightDuration => m_deathInFightOccuranceRange.y - m_deathInFightOccuranceRange.x;

        public void Initialize(Army playerArmy, ArmyBattalionManager playerBattalion, Army enemyArmy, ArmyBattalionManager enemyBattalion, float fightDuration)
        {
            m_player = new ArmyReference(playerArmy, playerBattalion);
            m_enemy = new ArmyReference(enemyArmy, enemyBattalion);

            m_deathOrder = new List<DeathOrderInfo>();
            m_deathInFightOccuranceRange = new Vector2(fightDuration * m_deathInFightOccurancePercent.x, fightDuration * m_deathInFightOccurancePercent.y);

            m_nextPlayerUnitTypeToKillIndex = Random.Range(0, (int)DamageType._COUNT);
            m_nextEnemyUnitTypeToKillIndex = Random.Range(0, (int)DamageType._COUNT);
        }

        public IEnumerator DyingInBattleRoutine(ArmyBattleCombatResult result)
        {
            m_deathOrder.Clear();
            var playerUnitsToKillOff = CalculateUnitsToKillOff(m_player.battalion, result.player.remainingTroopCount, m_player.troopPerUnit);
            ScheduleDeaths(playerUnitsToKillOff, true);
            var enemyUnitsToKillOff = CalculateUnitsToKillOff(m_enemy.battalion, result.enemy.remainingTroopCount, m_enemy.troopPerUnit);
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
                    KillOffUnit(m_player, ref m_nextPlayerUnitTypeToKillIndex);
                }
                else
                {
                    KillOffUnit(m_enemy, ref m_nextEnemyUnitTypeToKillIndex);
                }
            }

        }

        private void KillOffUnit(ArmyReference reference, ref int index)
        {
            bool hasViableUnitToKillOff = false;
            ArmyUnitsHandle unitHandle = null;
            var maxAttempts = 3;
            for (int attempts = 1; attempts <= maxAttempts; attempts++)
            {
                var damageType = (DamageType)index;
                unitHandle = reference.battalion.GetUnitHandle(damageType);
                if (unitHandle.GetUnitCount() == 0)
                {
                    NextIndex(ref index);
                    continue;
                }

                if (unitHandle.GetUnitCount() == 1 && reference.HasAvailableGroup(damageType))
                {
                    NextIndex(ref index);
                    continue;
                }

                hasViableUnitToKillOff = true;
                break;
            }

            if (hasViableUnitToKillOff)
            {
                unitHandle.KillOffUnits(1);
                NextIndex(ref index);
            }

            void NextIndex(ref int index)
            {
                index += 1;
                index = (int)Mathf.Repeat(index, ((int)DamageType._COUNT));
            }
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