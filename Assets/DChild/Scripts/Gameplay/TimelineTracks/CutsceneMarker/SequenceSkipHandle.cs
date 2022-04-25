using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChildDebug.Cutscene
{
    public class SequenceSkipHandle : MonoBehaviour
    {
        public static event Action SkipExecute;

        [Button]
        private void SkipSequence()
        {
            SkipExecute?.Invoke();
        }
    }
}
