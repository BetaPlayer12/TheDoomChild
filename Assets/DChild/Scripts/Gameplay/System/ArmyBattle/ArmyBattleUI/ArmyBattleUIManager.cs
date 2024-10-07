using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattleUIManager : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattlePlayerOption m_playerOption;
        [SerializeField]
        private ArmyParticipantDetailsUI m_participantDetails;


        public void Initialize(PlayerArmyController player, ArmyController enemy)
        {
            m_playerOption.Initialize(player);
            m_participantDetails.Display(player, enemy);
        }

        public void UpdatePlayerOptions()
        {
            m_playerOption.UpdateOptions();
        }
    }
}