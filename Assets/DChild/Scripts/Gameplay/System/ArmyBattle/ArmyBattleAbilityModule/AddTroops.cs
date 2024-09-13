using UnityEngine;
using DChild.Gameplay.ArmyBattle;


[System.Serializable]
public class AddTroops : IArmyAbilityEffect
{

    [SerializeField]
    private float m_TroopsPercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;

    public void ApplyEffect(Army owner, Army opponent)
    {

        var convertedTroopPercentage = PercentageConverter.ConvertPercentage(m_TroopsPercentage);

        if (m_appliedEffectToOpponent)
        {
            var troopsConverted = PercentageConverter.ConvertTroopsPercentage(convertedTroopPercentage, opponent);
            opponent.AddTroopCount(Mathf.FloorToInt(troopsConverted));
            Debug.Log(opponent.troopCount + " Opponent");
        }
        else
        {
            
            var troopsConverted = PercentageConverter.ConvertTroopsPercentage(convertedTroopPercentage, owner);
            owner.AddTroopCount(Mathf.FloorToInt(troopsConverted));
            Debug.Log(owner.troopCount +" Player");
        }
    }

 
   

}
