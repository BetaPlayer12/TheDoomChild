using DChild.Gameplay.Combat.StatusAilment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class ResetStatusEffectDurationModule : IStatusEffectModule
    {

        [SerializeField]
       private StatusEffectType m_type;

        public void Start(Character character)
        {
            character.GetComponent<StatusEffectReciever>().ResetDuration(m_type);

        }

        public void Stop(Character character)
        {
          
        }

    }
}

