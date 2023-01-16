using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyBattleResolverData", menuName = "DChild/Gameplay/Army/Battle Resolver")]
    public class ArmyBattleResolverData : ScriptableObject
    {
        [SerializeField, MinValue(1)]
        private int m_baseDamageValue;
        [SerializeField, MinValue(0), BoxGroup("Battle Result Modifiers")]
        private float m_drawModifier = 1;
        [SerializeField, MinValue(0), BoxGroup("Battle Result Modifiers")]
        private float m_winModifier = 1;
        [SerializeField, MinValue(0), BoxGroup("Battle Result Modifiers")]
        private float m_loseModifier = 1;
        [SerializeField, Range(0f, 1f)]
        private float m_chanceToLoseACharacter;


        public int baseDamageValue => m_baseDamageValue;
        public float drawModifier => m_drawModifier;
        public float winModifier => m_winModifier;
        public float loseModifier => m_loseModifier;
        public float chanceToLoseACharacter => m_chanceToLoseACharacter;
    }
}