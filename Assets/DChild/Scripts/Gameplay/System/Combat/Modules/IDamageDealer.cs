/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/

namespace DChild.Gameplay.Combat
{
    public interface IDamageDealer
    {
        void Damage(TargetInfo target, BodyDefense targetDefense);
    }
}
