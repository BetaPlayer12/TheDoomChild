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
            m_unitIcon.sprite = characterData != null ? characterData.icon : null;
            m_unitIcon.enabled = m_unitIcon.sprite != null;
            m_unitBackground.enabled = m_unitIcon.enabled;
        }
    }
}