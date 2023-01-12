using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class Army : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private ArmyCompositionData m_initialComposition;
#endif
        [SerializeField]
        private Health m_troopCount;
        [SerializeField, HideInEditorMode]
        private ArmyComposition m_composition;

        public Health troopCount => m_troopCount;

        public int GetPower(UnitType unitType) => m_composition.GetTotalUnitPower(unitType);

        public ArmyCharacter RemoveRandomCharacter(UnitType unitType) => m_composition.RemoveCharacter(unitType, Random.Range(0, m_composition.GetNumberOfCharacter(unitType)));

        public void SetArmyComposition(ArmyComposition armyComposition)
        {
            m_composition = armyComposition;
            SetTroopCount(m_composition.troopCount);
        }

        public void RecordArmyCompositionTo(ref ArmyComposition armyComposition)
        {
            armyComposition.CopyComposition(m_composition);
        }

        public void SetTroopCount(int troopCount)
        {
            m_troopCount.SetMaxValue(troopCount);
            m_troopCount.ResetValueToMax();
        }

        public void Initialize()
        {
            SetTroopCount(m_composition.troopCount);
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (m_initialComposition != null)
                m_composition = m_initialComposition.GenerateArmyCompositionInstance();
#endif
        }
    }
}