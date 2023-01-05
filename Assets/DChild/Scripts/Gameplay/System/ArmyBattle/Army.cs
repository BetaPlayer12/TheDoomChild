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

        [SerializeField, HideInEditorMode]
        private ArmyComposition m_composition;

        public void SetArmyComposition(ArmyComposition armyComposition) => m_composition = armyComposition;

        private void Awake()
        {
#if UNITY_EDITOR
            if (m_initialComposition != null)
                m_composition = m_initialComposition.GenerateArmyCompositionInstance();
#endif
        }
    }
}