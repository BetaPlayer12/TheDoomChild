using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace DChildDebug.Cutscene
{
    [Serializable]
    public class CutsceneMarkerBehaviour : PlayableBehaviour
    {
        [HideInInspector]
        public double clipStart;
        [HideInInspector]
        public double clipEnd;
    }
}
