using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [CreateAssetMenu(fileName = "PlayerStatsData", menuName = "DChild/Gameplay/Character/Player Stats")]
    public class PlayerBaseStatsData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private PlayerStatsInfo m_info;

        public PlayerStatsInfo info => m_info;
    }
}


