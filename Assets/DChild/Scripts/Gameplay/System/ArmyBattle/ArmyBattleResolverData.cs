using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyBattleResolverData",menuName = "DChild/Gameplay/Army/Battle Resolver")]
    public class ArmyBattleResolverData : ScriptableObject
    {
        [SerializeField, MinValue(0)]
        private float m_drawModifier;
        [SerializeField, MinValue(0)]
        private float m_winModifier;
        [SerializeField, MinValue(0)]
        private float m_loseModifier;

        public float drawModifier => m_drawModifier;
        public float winModifier => m_winModifier;
        public float loseModifier => m_loseModifier;
    }
}