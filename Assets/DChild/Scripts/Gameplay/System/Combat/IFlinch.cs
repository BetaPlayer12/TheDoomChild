using DChild.Gameplay.Characters;

namespace DChild.Gameplay.Combat
{
    public interface IFlinch
    {
        bool isAlive { get; }
        void Flinch(RelativeDirection damageSource, AttackType damageTypeRecieved);
    }
}