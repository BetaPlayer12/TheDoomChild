using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [CreateAssetMenu(fileName = "ArmyGroupData", menuName = "DChild/Gameplay/Army/Group")]
    public class ArmyGroupTemplateData : SerializedScriptableObject
    {
        [SerializeField]
        private ArmyCharacterGroup m_armyCharacterGroup;
        [SerializeField]
        private SpecialSkill m_specialSkill;

        #region Attack Fields
        [SerializeField, ToggleGroup("m_canAttack", "Can Attack")]
        private bool m_canAttack;
        [SerializeField, DisableInInlineEditors, ToggleGroup("m_canAttack"), Indent]
        private UnitType m_unitType;
        [SerializeField, ToggleGroup("m_canAttack"), Indent]
        private bool m_useCharactersForPower;
        [SerializeField, MinValue(1), ToggleGroup("m_canAttack"), ShowIf("@m_canAttack && !m_useCharactersForPower"), Indent(2)]
        private Holysoft.Collections.RangeInt m_power = new Holysoft.Collections.RangeInt(1, 1);
        #endregion

        #region Ability Fields
        [SerializeField, ToggleGroup("m_hasAbility", "Has Ability")]
        private bool m_hasAbility;
        [SerializeField, TextArea, ToggleGroup("m_hasAbility"), Indent]
        private string m_abilityDescription;
        [SerializeField, ToggleGroup("m_hasAbility"), Indent]
        private IArmyAbilityEffect[] m_effects = new IArmyAbilityEffect[0];
        #endregion

        public ArmyCharacterGroup armyCharacterGroup => m_armyCharacterGroup;
        public SpecialSkill specialSkill => m_specialSkill;

        #region Attack Properties
        public bool canAttack => m_canAttack;
        public bool isUsingCharactersForPower => m_useCharactersForPower;
        public UnitType attackType => m_canAttack ? m_unitType : UnitType.None;
        #endregion

        #region Ability Properties
        public bool hasAbility => m_hasAbility;
        public string abilityDescription => m_abilityDescription;
        #endregion

        #region Attack Functions
        public int GetTotalAttackPower()
        {
            if (m_canAttack)
            {
                if (m_useCharactersForPower)
                {
                    var power = 0;
                    return power;
                }
                else
                {
                    return m_power.GenerateRandomValue();
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        public void UseAbility(Army owner, Army oppponent)
        {
            for (int i = 0; i < m_effects.Length; i++)
            {
                m_effects[i].ApplyEffect(owner, oppponent);
            }
        }
    }
}