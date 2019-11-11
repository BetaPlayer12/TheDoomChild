using PixelCrushers.DialogueSystem;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct FurryWhisperer : ISoulSkillModule
    {
        public void AttachTo(IPlayer player)
        {
            DialogueLua.SetVariable("FurryWhispererEquipped", true);
        }

        public void DetachFrom(IPlayer player)
        {
            DialogueLua.SetVariable("FurryWhispererEquipped", false);
        }
    }
}