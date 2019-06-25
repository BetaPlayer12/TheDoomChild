using DChild.Gameplay.Characters;

namespace DChild.Gameplay.Combat
{
    public interface IFlinch
    {
        void Flinch(RelativeDirection damageSource, AttackType damageTypeRecieved);
    }
}