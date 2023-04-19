using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtUIManager : MonoBehaviour
    {
        [SerializeField]
        private BattleAbilities m_reference;

        private Dictionary<BattleAbility, CombatArtSelectButton> m_abilityButtonPair;
        private CombatArtSelectRequirements[] m_artRequirements;

        public void Initialize()
        {
            var combatArtCount = (int)BattleAbility._Count;
            for (int i = 0; i < combatArtCount; i++)
            {
                var ability = (BattleAbility)i;
                if (m_reference.IsAbilityActivated(ability))
                {
                    m_abilityButtonPair[ability].SetState(CombatArtSelectButton.State.Unlocked);
                }
                else
                {
                    m_abilityButtonPair[ability].SetState(CombatArtSelectButton.State.Locked);
                }

            }
            for (int i = 0; i < m_artRequirements.Length; i++)
            {
                m_artRequirements[i].ValidateButtonState();
            }
        }

        private void Awake()
        {
            m_abilityButtonPair = new Dictionary<BattleAbility, CombatArtSelectButton>();
            var buttons = GetComponentsInChildren<CombatArtSelectButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                m_abilityButtonPair.Add(button.skillUnlock, button);
            }
            m_artRequirements = GetComponentsInChildren<CombatArtSelectRequirements>();
        }
    }

}