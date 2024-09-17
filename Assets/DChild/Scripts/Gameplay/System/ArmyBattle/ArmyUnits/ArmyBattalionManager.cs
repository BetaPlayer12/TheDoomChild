using DChild.Gameplay.ArmyBattle.Units;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    public class ArmyBattalionManager : SerializedMonoBehaviour, IArmyBattalion
    {
        [SerializeField]
        private ArmyBattalionGenerator m_unitGenerator;
        [SerializeField, TabGroup("Units", "Melee"), HideLabel]
        private ArmyUnitsHandle m_meleeUnitsHandle;
        [SerializeField, TabGroup("Units", "Range"), HideLabel]
        private ArmyUnitsHandle m_rangeUnitsHandle;
        [SerializeField, TabGroup("Units", "Magic"), HideLabel]
        private ArmyUnitsHandle m_magicUnitsHandle;

        public Vector2 centerPosition => transform.position;

        public int GetTotalUnitCount()
        {
            var totalUnits = 0;
            for (int i = 0; i < (int)DamageType._COUNT; i++)
            {
                totalUnits += GetUnitHandle((DamageType)i).GetUnitCount();
            }
            return totalUnits;
        }

        public void KillOff(DamageType unitType, int count)
        {
            GetUnitHandle(unitType).KillOffUnits(count);
        }

        [Button]
        public void GenerateArmy(DamageType[] typesToGenerate)
        {
            var armyUnits = m_unitGenerator.GenerateArmy(typesToGenerate);
            m_meleeUnitsHandle.SetUnits(armyUnits.Where(x => x.type == DamageType.Melee).ToArray());
            m_rangeUnitsHandle.SetUnits(armyUnits.Where(x => x.type == DamageType.Range).ToArray());
            m_magicUnitsHandle.SetUnits(armyUnits.Where(x => x.type == DamageType.Magic).ToArray());

            AllIdle();
        }

        [Button]
        public void Attack(DamageType type, IArmyBattalion target)
        {
            GetUnitHandle(type).Attack(target);
        }

        [Button]
        public void StopAttack()
        {
            m_meleeUnitsHandle.StopAttack();
            m_rangeUnitsHandle.Idle();
            m_magicUnitsHandle.Idle();
        }

        private void AllIdle()
        {
            m_meleeUnitsHandle.Idle();
            m_rangeUnitsHandle.Idle();
            m_magicUnitsHandle.Idle();
        }

        public ArmyUnitsHandle GetUnitHandle(DamageType type)
        {
            switch (type)
            {
                case DamageType.Melee:
                    return m_meleeUnitsHandle;
                case DamageType.Range:
                    return m_rangeUnitsHandle;
                case DamageType.Magic:
                    return m_magicUnitsHandle;
                default:
                    return null;
            }
        }
    }
}