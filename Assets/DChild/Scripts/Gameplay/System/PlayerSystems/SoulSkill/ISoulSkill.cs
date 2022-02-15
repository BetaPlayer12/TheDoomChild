using DChild.Gameplay.Characters.Players;
using System.Collections;

namespace DChild.Gameplay.SoulSkills
{
    public interface ISoulSkill : ISoulSkillInfo
    {
        void ApplyEffectTo(IPlayer player);
        void RemoveEffectFrom(IPlayer player);
    }
}
