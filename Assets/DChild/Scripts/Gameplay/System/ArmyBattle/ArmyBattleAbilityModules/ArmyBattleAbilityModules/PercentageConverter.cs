using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;


[System.Serializable]
public static class PercentageConverter  
{

    public static float ConvertPercentage(float percentage)
    {
        var maxValue = 100;
        percentage /= maxValue;
        return percentage;
    }

    public static float ConvertTroopsPercentage(float troopPercentage, Army ownerOrEnemy)
    {
        var currentValueHp = ownerOrEnemy.troopCount;
        var valueOftroops = troopPercentage * currentValueHp;
        return valueOftroops;
    }
}
