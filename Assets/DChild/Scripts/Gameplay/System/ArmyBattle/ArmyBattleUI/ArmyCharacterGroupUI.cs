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
                    }
                    else
                    {
                        //In case the number of Member UIs exceeds armyCharacterMember Count
                        m_memberUIs[i].Display(null);
                    }
                }

            }
            else
            {
                for (int i = 0; i < m_memberUIs.Length; i++)
                {
                    m_memberUIs[i].Display(null);
                }
            }
        }
    }
}