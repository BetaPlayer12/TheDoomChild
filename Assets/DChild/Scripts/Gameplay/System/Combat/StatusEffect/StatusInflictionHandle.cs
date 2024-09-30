using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class StatusInflictionHandle : SerializedMonoBehaviour
    {
        [OdinSerialize, HideReferenceObjectPicker, OnValueChanged("Validate")]
        private Dictionary<StatusEffectType, StatusEffectData> m_list = new Dictionary<StatusEffectType, StatusEffectData>();

        [Button]
        public void Inflict(StatusEffectReciever reciever, StatusEffectType statusEffect)
        {
            if (reciever.IsInflictedWith(statusEffect) == false)
            {
                var hasRecieverResistedStatusEffect = reciever.resistance == null ? false : IsRNGSuccess(reciever.resistance.GetResistance(statusEffect));
                if (hasRecieverResistedStatusEffect == false)
                {
                    reciever.RecieveStatusEffect(m_list[statusEffect].CreateHandle());
                }
            }
        }

        public void Inflict(StatusEffectReciever reciever, params StatusEffectChance[] statusEffectChance)
        {
            for (int i = 0; i < statusEffectChance.Length; i++)
            {
                if (IsRNGSuccess(statusEffectChance[i].chance))
                {
                    Inflict(reciever, statusEffectChance[i].type);
                }
            }
        }

        private bool IsRNGSuccess(int compareWith)
        {
            if (compareWith >= 100)
            {
                return true;
            }
            else if (compareWith <= 0)
            {
                return false;
            }
            else
            {
                return Random.Range(0, 100) <= compareWith;
            }
        }

        private void Validate()
        {
            var keys = m_list.Keys;
            foreach (var key in keys)
            {
                if (m_list[key].type != key)
                {
                    m_list.Remove(key);
                }
            }
        }
    }
}