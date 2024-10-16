using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public struct ArmyBattleCombatResult
    {
        [System.Serializable]
        public struct Record
        {
            [SerializeField]
            private int m_initialTroopCount;
            [SerializeField]
            private int m_remainingTroopCount;
            [SerializeField]
            private DamageType m_attackType;
            [SerializeField]
            private DamageType m_damageTypeRecieved;

            public Record(int initialTroopCount, int remainingTroopCount, DamageType attackType, DamageType damageTypeRecieved)
            {
                m_initialTroopCount = initialTroopCount;
                m_remainingTroopCount = remainingTroopCount;
                m_attackType = attackType;
                m_damageTypeRecieved = damageTypeRecieved;
            }

            public int initialTroopCount => m_initialTroopCount;
            public int remainingTroopCount => m_remainingTroopCount;
            public DamageType attackType => m_attackType;

            public DamageType damageTypeRecieved => m_damageTypeRecieved;
            public int damageReceived => Mathf.Max(0, m_initialTroopCount - m_remainingTroopCount);
            public int healingReceived => Mathf.Max(0, m_remainingTroopCount - m_initialTroopCount);
            public bool hasReceivedHealing => healingReceived > 0;

            public override string ToString()
            {
                return $"Initial Troop Count: {m_initialTroopCount} \n" +
                       $"Attacked With: {m_attackType} \n" +
                       $"Received Damage: {m_damageTypeRecieved} {damageReceived} \n" +
                       $"Healed: {healingReceived}\n" + 
                       $"Remaining Troop Count: {m_remainingTroopCount}\n";
            }
        }

        [SerializeField]
        private Record m_player;
        [SerializeField]
        private Record m_enemy;

        public ArmyBattleCombatResult(Record player, Record enemy)
        {
            m_player = player;
            m_enemy = enemy;
        }

        public Record player => m_player;
        public Record enemy => m_enemy;

        public override string ToString()
        {
            return $"::::Army Combat Result::::\n" +
                   $"====Player====\n" +
                   $"{m_player}\n" +
                   $"====Enemy====\n" +
                   $"{m_enemy}";
        }
    }
}