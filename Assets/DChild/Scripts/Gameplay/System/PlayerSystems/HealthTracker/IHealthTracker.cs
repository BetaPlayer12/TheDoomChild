using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Systems
{
    public interface IHealthTracker
    {
        void TrackHealth(Damageable damageable);
        void RemoveTracker(Damageable damageable);
    }
}