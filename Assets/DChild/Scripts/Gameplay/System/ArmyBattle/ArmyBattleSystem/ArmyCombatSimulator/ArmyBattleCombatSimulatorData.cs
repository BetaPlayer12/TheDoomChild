using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyBattleCombatSimulatorData", menuName = "DChild/Gameplay/Army/System/Combat Simulator Data")]
    public class ArmyBattleCombatSimulatorData : SerializedScriptableObject
    {
        [System.Serializable]
        private struct DamagePair
        {
            [SerializeField]
            private DamageType m_attack;
            [SerializeField]
            private DamageType m_against;

            public DamageType attack => m_attack;
            [SerializeField]
            public DamageType against => m_against;

            public DamagePair(DamageType attack, DamageType against)
            {
                m_attack = attack;
                m_against = against;
            }
        }

        [SerializeField, MinValue(1)]
        private int m_troopCountConstant = 1;
        [SerializeField, MinValue(0)]
        private float m_randomizeAttackValueModifierRange;
        [SerializeField]
        private Dictionary<DamagePair, float> m_damageTypeModifierConfig = new Dictionary<DamagePair, float>();

        public int troopCountConstant => m_troopCountConstant;
        public float randomizeAttackValueModifierRange => m_randomizeAttackValueModifierRange;

        public float GetDamageTypeModifier(DamageType attack, DamageType against)
        {
            var key = new DamagePair(attack, against);
            if (m_damageTypeModifierConfig.TryGetValue(key, out float modifier))
            {
                return modifier;
            }

            return 1;
        }
    }
}