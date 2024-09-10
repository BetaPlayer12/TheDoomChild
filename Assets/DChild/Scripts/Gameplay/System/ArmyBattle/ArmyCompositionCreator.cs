using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyCompositionCreator : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_maxCharacterPerType = 1;

        private ArmyComposition m_composition;

        public void LoadComposition(ArmyComposition armyComposition)
        {

        }

        public void AddCharacter(ArmyCharacterData character)
        {
            //if(m_composition.GetNumberOfCharacter(character.unitType) < m_maxCharacterPerType)
            //{
            //   // m_composition.AddCharacter(character);
            //}
        }

        public void RemoveCharacter(ArmyCharacterData character)
        {

        }
    }
}