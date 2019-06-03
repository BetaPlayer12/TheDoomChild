using DChild.Gameplay.Characters.Players.Equipments;

namespace DChild.Gameplay.Characters.Players
{
    public interface IEquipment
    {
        IArmor armor { get; }
        IWeapon weapon { get; }
    }
}