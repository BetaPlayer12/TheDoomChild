using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillVisuals
    {
        Transform transform { get; }
        void Play(int turnCount);

        GameObject gameObject { get; }
    }
}

