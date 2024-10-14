using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class AddInventoryItems : ISpecialSkillModule, ISpecialSkillImplementor
    {
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            Debug.Log("It works now Apply the Effects");
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {

        }
    }
}

