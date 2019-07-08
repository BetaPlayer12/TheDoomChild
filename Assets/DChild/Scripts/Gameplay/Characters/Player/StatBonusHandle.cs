using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "StatBonusHandle", menuName = "DChild/Player/ Stat Bonus Handle")]
    public class StatBonusHandle : SerializedScriptableObject
    {
        [OdinSerialize]
        private Dictionary<PlayerStat, BonusConversion> m_conversions;

        public bool HasBonus(PlayerStat stat) => m_conversions.ContainsKey(stat);

        public int GetBonus(PlayerStat stat, IAttributes attributes)
        {
            return m_conversions[stat].GetBonus(attributes);
        }
    }
}