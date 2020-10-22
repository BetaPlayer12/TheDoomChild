using UnityEngine;

namespace Holysoft.Gameplay.UI
{
    public class CappedStatUIGroup : CappedStatUI
    {
        [SerializeField]
        private CappedStatUI[] m_children;

        public override float maxValue
        {
            set
            {
                for (int i = 0; i < m_children.Length; i++)
                {
                    m_children[i].maxValue = value;
                }
            }
        }
        public override float currentValue
        {
            set
            {
                for (int i = 0; i < m_children.Length; i++)
                {
                    m_children[i].currentValue = value;
                }
            }
        }
    }

}