using PixelCrushers.DialogueSystem;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct FurryWhisperer : ISoulSkillModule
    {
        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            DialogueLua.SetVariable("whisper", true);
          
            
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            DialogueLua.SetVariable("whisper", false);
        }
    }
}