using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyGroup 
    {
        [SerializeField]
        private ArmyCharacterGroup m_members;
        [SerializeField]
        private DamageType m_type;

        public ArmyGroup (ArmyCharacterGroup members, DamageType type)
        {
            m_members = members;
            m_type = type;
        }
    }
}

