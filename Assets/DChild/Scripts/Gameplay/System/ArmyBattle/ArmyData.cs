using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyData", menuName = "DChild/Gameplay/Army/Army Data")]
    public class ArmyData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private ArmyInfo m_info;

        public ArmyInfo info => m_info;
    }
}

