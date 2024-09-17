using UnityEngine;
using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAIData", menuName = "DChild/Gameplay/Army/Army AI Data")]
    public class ArmyAIData : SerializedScriptableObject
    {
        [SerializeField]
        private ArmyData m_armyData;
        [SerializeField]
        private IArmyAIAction[] m_aiAction;
        [SerializeField]
        private RandomizedArmyAttackHandle m_randomAttack;

        public ArmyData armyData => m_armyData;

        public ArmyGroupTemplateData ChooseAttack(int round)
        {
            var actionIndex = round - 1;
            if (m_aiAction.Length >= round)
            {
                if (m_aiAction[actionIndex].isRandomizedAction == false)
                {
                    Debug.Log("Not random");
                    return m_aiAction[actionIndex].GetAction();

                }
                else
                {
                    Debug.Log("random");
                    return m_randomAttack.Randomized();
                }
            }
            else
            {
                return m_randomAttack.Randomized();
            }
        }

    }


}