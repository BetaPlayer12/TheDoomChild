using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class PlayerArmyController : ArmyController
    {
        [SerializeField, ValueDropdown("GetAllAttackingGroups"), HideInEditorMode]
        private IAttackingGroup m_chosenAttack;

        public void UseThisTurn(IAttackingGroup chosenAttack)
        {
            m_chosenAttack = chosenAttack;
        }

        public bool HasViableTurnOptions()
        {
            for (int i = 0; i < (int)DamageType._COUNT; i++)
            {
                if (m_controlledArmy.HasAvailableGroup((DamageType)i))
                {
                    return true;
                }
            }


            return false;
        }

        public override ArmyTurnAction GetTurnAction(int turnNumber)
        {
            if (m_chosenAttack == null)
                return new ArmyTurnAction
                {
                    troopCount = m_controlledArmy.troopCount,
                    willAttack = false
                };

            return new ArmyTurnAction()
            {
                troopCount = m_controlledArmy.troopCount,
                attack = new ArmyDamage(m_chosenAttack.GetDamageType(), m_chosenAttack.GetAttackPower()),
                willAttack = true
            };
        }
        public override void CleanUpForNextTurn()
        {
            if (m_chosenAttack == null)
                return;

            m_controlledArmy.SetAttackingGroupAvailability(m_chosenAttack, false);
            m_chosenAttack = null;
        }


        private ValueDropdownList<IAttackingGroup> GetAllAttackingGroups()
        {
            ValueDropdownList<IAttackingGroup> list = new ValueDropdownList<IAttackingGroup>();

            List<IAttackingGroup> attackingGroups = new List<IAttackingGroup>();

            for (int i = 0; i < (int)DamageType._COUNT; i++)
            {
                attackingGroups.AddRange(m_controlledArmy.GetAvailableGroups((DamageType)i));
            }

            for (int i = 0; i < attackingGroups.Count; i++)
            {
                var group = attackingGroups[i];
                list.Add(group.GetCharacterGroup().name, group);
            }

            return list;
        }


    }
}