using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class MonsterCapsuleVisual : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private CapsuleMonsterVisualHandle m_monsterVisuals;
        [SerializeField]
        private CapsuleHighlightHandle m_highlightVisuals;
#endif
    }
}