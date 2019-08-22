using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    [System.Serializable]
    public struct CriticalDamageHandle
    {
        [SerializeField]
        [MinValue(1f)]
        private float m_critModifier;

        public void Execute(ref AttackInfo info, int chance, float modifier)
        {
            if (chance > 0 && Random.Range(0, 101) <= chance)
            {
                info.isCrit = true;
                for (int i = 0; i < info.damageList.Count; i++)
                {
                    var damageInfo = info.damageList[i];
                    var damage = damageInfo.damage;
                    damage.value = Mathf.FloorToInt(damage.value * m_critModifier * modifier);
                    damageInfo.damage = damage;
                    info.damageList[i] = damageInfo;
                }
            }
        }
    }
}