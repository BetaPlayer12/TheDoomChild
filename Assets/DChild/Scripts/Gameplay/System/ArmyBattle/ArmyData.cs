using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyData
    {
        [SerializeField]
        private ArmyInfo m_info;

        public ArmyInfo info => m_info;
    }
}

