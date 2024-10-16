using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills
{
    public class ArmyBattleSpecialSkillDebugger : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleSpecialSkillHandle m_handle;
        [SerializeField]
        private ArmyController m_owner;

        [Button]
        private void UseSkill(ArmyGroupTemplateData armyGroup)
        {
            m_handle.Activate(armyGroup.specialSkill, m_owner);
        }
    }
}