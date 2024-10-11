using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class AttackingGroupPowerUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_armyPower;

        public void Display(IAttackingGroup group)
        {
            m_armyPower.text = $"{group.GetAttackPower()}";
        }
    }
}