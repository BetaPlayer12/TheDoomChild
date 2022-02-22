// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine.Playables;

namespace PixelCrushers.DialogueSystem
{
    [Serializable]
    public class PlaySequenceShortcutBehaviour : PlayableBehaviour
    {
        public enum SequenceShortcut
        {
        }

        public string m_reference;

        public void Execute()
        {

        }
    }
}
#endif
#endif
