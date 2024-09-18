using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyAbilityTest : IArmyAbilityEffect
{
    public void ApplyEffect(Army owner, Army opponent)
    {
        Debug.Log("Ability Used");
    }

}
