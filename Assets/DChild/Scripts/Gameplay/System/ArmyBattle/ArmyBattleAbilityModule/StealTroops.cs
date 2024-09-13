using UnityEngine;
using DChild.Gameplay.ArmyBattle;


[System.Serializable]
public class StealTroops :IArmyAbilityEffect
{

    [SerializeField]
    private float m_TroopsPercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;
    //get 3% from enemy health percentage then add to the player
    public void ApplyEffect(Army owner, Army opponent)
    {
        var convertedTroopPercentage = PercentageConverter.ConvertPercentage(m_TroopsPercentage);
        if (m_appliedEffectToOpponent)
        {
            var troopsConverted = PercentageConverter.ConvertTroopsPercentage(convertedTroopPercentage, opponent);
            opponent.SubtractTroopCount(Mathf.FloorToInt(troopsConverted));
            owner.AddTroopCount(Mathf.FloorToInt(troopsConverted));
            Debug.Log(opponent.troopCount + " Opponent");
        }
        else
        {
            var troopsConverted = PercentageConverter.ConvertTroopsPercentage(convertedTroopPercentage, opponent);
            opponent.AddTroopCount(Mathf.FloorToInt(troopsConverted));
            owner.SubtractTroopCount(Mathf.FloorToInt(troopsConverted));
            Debug.Log(owner.troopCount + " Player");
        }
    }










}
