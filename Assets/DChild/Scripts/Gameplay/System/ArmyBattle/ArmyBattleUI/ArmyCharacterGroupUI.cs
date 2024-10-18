using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyCharacterGroupUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyCharacterUI[] m_memberUIs;
        public void Display(ArmyCharacterGroup armyCharacterGroup)
        {
            if (armyCharacterGroup != null)
            {

                for (int i = 0; i < m_memberUIs.Length; i++)
                {
                    if (i < armyCharacterGroup.memberCount)
                    {
                        m_memberUIs[i].Display(armyCharacterGroup.GetCharacter(i));
                        continue;
                    }
                    //In case the number of Member UIs exceeds armyCharacterMember Count
                    m_memberUIs[i].Display(null);
                }

                return;
            }

            for (int i = 0; i < m_memberUIs.Length; i++)
            {
                m_memberUIs[i].Display(null);
            }
        }

        [Button]
        public void Display(ArmyGroupTemplateData armyGroupTemplateData)
        {
            Display(armyGroupTemplateData.armyCharacterGroup);
        }
    }
}