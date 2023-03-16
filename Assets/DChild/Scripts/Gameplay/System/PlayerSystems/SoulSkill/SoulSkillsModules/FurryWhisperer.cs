using Holysoft.Event;
using PixelCrushers.DialogueSystem;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct FurryWhisperer : ISoulSkillModule
    {
        public struct StateChangeEvent : IEventActionArgs
        {
            public StateChangeEvent(bool isactive) : this()
            {
                this.isactive = isactive;
            }

            public bool isactive { get; }
        }
        public static event EventAction<FurryWhisperer.StateChangeEvent> Onstatechange;
        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            DialogueLua.SetVariable("whisper", true);
            Onstatechange?.Invoke(this, new StateChangeEvent(true));
            
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            DialogueLua.SetVariable("whisper", false);
            Onstatechange?.Invoke(this, new StateChangeEvent(false));
        }
    }
}