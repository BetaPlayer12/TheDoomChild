using Sirenix.OdinInspector;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    public class SkipCutscene : MonoBehaviour
    {

        [Button]
        private void skipscene(PlayableDirector m_currentDirector, float m_timeToSkipTo)
        {
            m_currentDirector.time = m_timeToSkipTo;
        }
    }
}
