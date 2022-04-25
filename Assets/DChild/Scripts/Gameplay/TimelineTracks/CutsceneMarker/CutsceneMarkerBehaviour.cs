using System;
using Unity.Collections;
using UnityEngine.Playables;

namespace DChildDebug.Cutscene
{
    [Serializable]
    public class CutsceneMarkerBehaviour : PlayableBehaviour
    {
        [ReadOnly]
        public double clipStart;
        [ReadOnly]
        public double clipEnd;
    }
}
