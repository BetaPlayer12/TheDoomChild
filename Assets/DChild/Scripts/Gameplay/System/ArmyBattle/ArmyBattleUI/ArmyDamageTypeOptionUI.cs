using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyDamageTypeOptionUI : MonoBehaviour
    {
        [SerializeField]
        private DamageType m_damageType;

        public DamageType damageType => m_damageType;
        //Put in Details of UI

        public void SetType(DamageType damageType)
        {
            m_damageType = damageType;
        }
    }
}