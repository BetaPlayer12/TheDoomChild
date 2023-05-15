namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyUnitModifier
    {
        float GetModifier(UnitType unitType);
        void SetModifier(UnitType unitType, float value);
        void AddModifier(UnitType unitType, float value);
        void ResetModifiers();
    }
}