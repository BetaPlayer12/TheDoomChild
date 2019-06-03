using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IPlayerCombat : IDefenseStats
    {
        Vector2 position { get; }
        void Displace(Vector2 force);
        void EnableHitboxes();
        void DisableHitboxes();
        void EnableController();
        void DisableController();
        void BecomeInvulnerable(bool value);
    }
}