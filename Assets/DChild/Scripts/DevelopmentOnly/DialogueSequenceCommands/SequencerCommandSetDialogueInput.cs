using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandSetDialogueInput : SequencerCommand
    {
        private bool hasExecuted;

        private void Execute()
        {
            var parameter = GetParameterAsBool(0);
            DialogueManager.SetDialogueSystemInput(parameter);
            hasExecuted = true;
        }

        public void Awake()
        {
            Execute();
            Stop();
        }

        public void OnDestroy()
        {
            if (hasExecuted == false)
            {
                Execute();
            }
            Stop();
        }
    }
}
