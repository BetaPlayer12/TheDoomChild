using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public struct CriticalDamageHandler
    {
        [SerializeField]
        [MinValue(1f)]
        private float m_critModifier;

        public void Execute(ref DamageInfo info, int chance, float modifier)
        {
            if (chance > 0 && Random.Range(0, 101) <= chance)
            {
                info.damage = Mathf.FloorToInt(info.damage * m_critModifier * modifier);
                info.isCrit = true;
            }
        }
    }
}