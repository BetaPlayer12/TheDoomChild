using DChild.Gameplay.Combat.StatusInfliction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class StatusResistance : SerializedMonoBehaviour, IStatusResistance
    {
        [OdinSerialize, HideReferenceObjectPicker, ReadOnly, PropertyOrder(2)]
        protected Dictionary<StatusEffectType, StatusResistanceType> m_resistanceInfo = new Dictionary<StatusEffectType, StatusResistanceType>();

        public StatusResistanceType GetResistance(StatusEffectType type) => m_resistanceInfo.ContainsKey(type) ? m_resistanceInfo[type] : StatusResistanceType.None;

        public void SetResistance(StatusEffectType type, StatusResistanceType resistanceType)
        {
            if (m_resistanceInfo.ContainsKey(type))
            {
                m_resistanceInfo[type] = resistanceType;
            }
            else
            {
                m_resistanceInfo.Add(type, resistanceType);
            }
        }
    }
}