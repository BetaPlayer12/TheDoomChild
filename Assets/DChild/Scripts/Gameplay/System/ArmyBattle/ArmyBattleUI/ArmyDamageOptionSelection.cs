using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyDamageOptionSelection : MonoBehaviour
    {
        [SerializeField]
        private ArmyDamageTypeOptionUI[] m_options;
        private Army m_reference;

        public void Initialize(Army army)
        {
            m_reference = army;
        }

        public void UpdateSelectionAvailability()
        {
            if(m_reference != null)
            {
                for (int i = 0; i < m_options.Length; i++)
                {
                    var option = m_options[i];
                    option.SetInteractability(m_reference.HasAvailableGroup(option.damageType));
                }
            }
        }
    }
}