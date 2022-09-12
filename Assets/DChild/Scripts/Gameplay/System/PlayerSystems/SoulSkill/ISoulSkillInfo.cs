using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    public interface ISoulSkillInfo
    {
        int id { get; }
        int soulCapacity { get; }
        Sprite icon { get; }
        string description { get; }
    }
}
