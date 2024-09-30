using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Holysoft.Event;

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

        public event EventAction<EventActionArgs> OnFightEnd;

        public Vector3 GetPlayerBattalionPosition() => m_player.centerPosition;
        public Vector3 GetEnemyBattalionPosition() => m_enemy.centerPosition;


        [Button]
        public void Initialize(Army playerArmy, Army enemyArmy)
        {
            var temporaryType = new DamageType[] { DamageType.Melee, DamageType.Range, DamageType.Magic };

            m_player.GenerateArmy(temporaryType);
            m_enemy.GenerateArmy(temporaryType);

            m_deathHandle.Initialize(playerArmy, m_player, enemyArmy, m_enemy, m_fightDuration);
        }

        [Button]
        public void VisualizeCombat(ArmyBattleCombatResult result)
        {
            StopAllCoroutines();
            StartCoroutine(FightRoutine(result));
            StartCoroutine(m_deathHandle.DyingInBattleRoutine(result));
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

            OnFightEnd?.Invoke(this, EventActionArgs.Empty);
        }
    }
}