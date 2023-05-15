using Holysoft.Event;

namespace DChild.Gameplay.Quests
{
    public static class DialogueVariableUtility
    {
        public static event EventAction<EventActionArgs> OnVariableChange;

        public static void SendVariableChangeEvent(object sender) => OnVariableChange?.Invoke(sender, EventActionArgs.Empty);
    }
}