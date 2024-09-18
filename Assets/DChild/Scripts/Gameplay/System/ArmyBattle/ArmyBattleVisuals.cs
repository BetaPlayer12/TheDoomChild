using System.Collections;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleVisuals : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_event;
        [SerializeField]
        private TextMeshProUGUI m_battleResult;

        public void InitializeArmyVisuals(ArmyController player, ArmyController enemy)
        {
            m_event.text = "";
            m_battleResult.text = "";
        }

        public IEnumerator StartBattleVisuals(ArmyController player, ArmyController enemy)
        {
            //m_event.text = $"Player Picked {player.currentAttack.type}({player.currentAttack.value}) \n Against \n Enemy {enemy.currentAttack.type}({enemy.currentAttack.value})";
            //Code to Simulate Fighting between 2 Armies
            yield return null;
        }

        public void SetArmyAnimationToCelebrate(Army player, string temporaryMessage)
        {
            m_battleResult.text = temporaryMessage;
        }
    }
}