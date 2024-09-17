using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.ArmyBattle.Units;
using Doozy.Runtime.Common.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{
    public class ArmyMeleeAttack : ArmyAttackVisualizer
    {
        private List<ArmyUnit> m_targets;
        private List<ArmyUnitMelee> m_units;
        private List<Vector3> m_unitStartingPosition;

        private static Dictionary<ArmyUnit, ArmyUnit> m_unitTargetPair;

        private bool m_isAttacking;

        private const float SPEED_RUNNING = 200f;

        public override void Attack(List<ArmyUnit> units, IArmyBattalion target)
        {
            if (m_isAttacking)
                return;

            m_isAttacking = true;

            m_targets.Clear();
            var liveUnits = units.Where(x => x.isAlive).ToList();

            var meleeTargets = target.GetUnitHandle(DamageType.Melee);
            if (meleeTargets.HasUnits())
            {
                DecideTargetsPerUnits(liveUnits, meleeTargets);
            }
            else
            {
                var rangeTargets = target.GetUnitHandle(DamageType.Range);
                if (rangeTargets.HasUnits())
                {
                    DecideTargetsPerUnits(liveUnits, rangeTargets);
                }
                else
                {
                    var magicTargets = target.GetUnitHandle(DamageType.Magic);
                    if (magicTargets.HasUnits())
                    {
                        DecideTargetsPerUnits(liveUnits, magicTargets);
                    }
                }
            }

            SetAsMeleeUnits(units);

            StopAllCoroutines();
            StartCoroutine(AttackTargetsRoutine(m_units, m_targets));
        }

        private void DecideTargetsPerUnits(List<ArmyUnit> liveUnits, ArmyUnitsHandle meleeTargets)
        {
            var shuffledLiveMeleeTargets = meleeTargets.GetUnits().Where(x => x.isAlive).ToList().Shuffle();

            for (int i = 0; i < liveUnits.Count; i++)
            {
                var unit = liveUnits[i];
                if (m_unitTargetPair.ContainsKey(unit) == false)
                {
                    var targetIndex = (int)Mathf.Repeat(i, shuffledLiveMeleeTargets.Count - 1);
                    var meleeTarget = shuffledLiveMeleeTargets[targetIndex];
                    m_targets.Add(meleeTarget);

                    m_unitTargetPair.Add(unit, meleeTarget);
                    if (m_unitTargetPair.ContainsKey(meleeTarget) == false)
                    {
                        m_unitTargetPair.Add(meleeTarget, unit);
                    }
                }
                else
                {
                    m_targets.Add(m_unitTargetPair[unit]);
                }
            }
        }

        private void SetAsMeleeUnits(List<ArmyUnit> units)
        {
            m_units.Clear();
            m_unitStartingPosition.Clear();
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                m_units.Add((ArmyUnitMelee)unit);
                m_unitStartingPosition.Add(unit.transform.position);
            }
        }

        public override void StopAttack(List<ArmyUnit> units)
        {
            m_isAttacking = false;
            StopAllCoroutines();
            StartCoroutine(ReturnToStartingPositionRoutine(m_units));
        }

        private IEnumerator AttackTargetsRoutine(List<ArmyUnitMelee> units, List<ArmyUnit> targets)
        {
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (unit.isAlive)
                {
                    var target = targets[i];
                    var directionTowardsTarget = (target.transform.position - unit.transform.position).normalized;
                    unit.Move(directionTowardsTarget);
                }
            }

            bool allUnitsAreNearTheirTarget = false;
            var meleeRange = 45f;
            do
            {
                allUnitsAreNearTheirTarget = true;
                for (int i = 0; i < units.Count; i++)
                {
                    var unit = units[i];
                    if (unit.isAlive)
                    {
                        var target = targets[i];
                        if (Mathf.Abs(unit.transform.position.x - target.transform.position.x) > meleeRange)
                        {
                            var directionTowardsTarget = (target.transform.position - unit.transform.position).normalized;
                            unit.transform.position += directionTowardsTarget * SPEED_RUNNING * GameplaySystem.time.deltaTime;
                            allUnitsAreNearTheirTarget = false;
                        }
                        else
                        {
                            unit.Attack();
                        }
                    }
                }
                yield return null;
            } while (allUnitsAreNearTheirTarget == false);
        }

        private IEnumerator ReturnToStartingPositionRoutine(List<ArmyUnitMelee> units)
        {
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                if (unit.isAlive)
                {
                    var directionTowardsTarget = (m_unitStartingPosition[i] - unit.transform.position).normalized;
                    unit.Move(directionTowardsTarget);
                }
            }

            bool allUnitsAreNearTheirTarget = false;
            do
            {
                allUnitsAreNearTheirTarget = true;
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    var unit = units[i];
                    if (unit.isAlive)
                    {
                        var target = m_targets[i];
                        var startingPoint = m_unitStartingPosition[i];
                        if (Mathf.Abs(unit.transform.position.x - startingPoint.x) > 5f)
                        {
                            var directionTowardsTarget = (startingPoint - unit.transform.position).normalized;
                            unit.transform.position += directionTowardsTarget * SPEED_RUNNING * GameplaySystem.time.deltaTime;
                            allUnitsAreNearTheirTarget = false;
                        }
                        else
                        {
                            unit.transform.position = m_unitStartingPosition[i];
                            var scale = unit.transform.localScale;
                            scale.x *= -1;
                            unit.transform.localScale = scale;
                            unit.Idle();

                            m_unitStartingPosition.RemoveAt(i);
                            units.RemoveAt(i);
                        }
                    }
                }
                yield return null;
            } while (allUnitsAreNearTheirTarget == false);
        }

        private void Awake()
        {
            m_targets = new List<ArmyUnit>();
            m_units = new List<ArmyUnitMelee>();
            m_unitStartingPosition = new List<Vector3>();

            if (m_unitTargetPair == null)
            {
                m_unitTargetPair = new Dictionary<ArmyUnit, ArmyUnit>();
            }
        }
    }
}