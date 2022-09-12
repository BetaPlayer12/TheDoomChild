namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandStopConversation : SequencerCommand
    {
        private bool hasExecuted;

        private void Execute()
        {
            DialogueManager.StopConversation();
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
