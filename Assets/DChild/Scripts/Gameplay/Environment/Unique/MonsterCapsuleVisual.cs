using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class MonsterCapsuleVisual : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private CapsuleMonsterVisualHandle m_monsterVisuals;
        [SerializeField]
        private CapsuleHighlightHandle m_highlightVisuals;
#endif

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Destroy(this);
            }
        }
    }
}