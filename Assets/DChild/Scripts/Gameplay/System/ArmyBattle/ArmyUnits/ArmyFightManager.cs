using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{
    public class ArmyFightManager : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattalionManager m_player;
        [SerializeField]
        private ArmyBattalionManager m_enemy;

        [SerializeField, MinValue(0)]
        private float m_fightDuration;
        [SerializeField]
        private ArmyFightDeathHandle m_deathHandle;
        [SerializeField, MinValue(0)]
        private float m_postAttackDuration;

        [Button]
        public void Initialize(DamageType[] playerUnitTypes, DamageType[] enemyUnitTypes)
        {
            m_player.GenerateArmy(playerUnitTypes);
            m_enemy.GenerateArmy(enemyUnitTypes);

            m_deathHandle.Initialize(m_fightDuration);
        }

        [Button]
        public void VisualizeCombat(ArmyBattleCombatResult result)
        {
            StopAllCoroutines();
            StartCoroutine(FightRoutine(result));
            StartCoroutine(m_deathHandle.DyingInBattleRoutine(m_player, m_enemy, result));
        }

        private IEnumerator FightRoutine(ArmyBattleCombatResult result)
        {
            m_player.StopAttack();
            m_enemy.StopAttack();

            m_player.Attack(result.player.attackType, m_enemy);
            m_enemy.Attack(result.enemy.attackType, m_player);

            yield return new WaitForSeconds(m_fightDuration);

            m_player.StopAttack();
            m_enemy.StopAttack();

            yield return new WaitForSeconds(m_postAttackDuration);
        }
    }
}