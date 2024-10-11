using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyCharacterUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_unitIcon;
        [SerializeField]
        private Image m_unitBackground;

        [Button]
        public void Display(ArmyCharacterData characterData)
        {
            if (characterData != null)
            {
                m_unitIcon.sprite = characterData.icon;
                return;
            }
            m_unitIcon.enabled = false;
            m_unitBackground.enabled = false;
        }
    }
}