using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyCompositionData", menuName = "DChild/Gameplay/Army/Army Composition Data")]
    public class ArmyCompositionData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private ArmyComposition m_composition = new ArmyComposition();

        public ArmyComposition GenerateArmyCompositionInstance() => new ArmyComposition(m_composition);
    }
}