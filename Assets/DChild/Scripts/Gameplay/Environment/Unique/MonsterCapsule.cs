using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class MonsterCapsule : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private CapsuleMonsterVisualHandle m_monsterVisuals;
        [SerializeField]
        private CapsuleHighlightHandle m_highlightVisuals;
#endif
        [SerializeField]
        private SpineAnimation m_spineAnimation;
    }
}