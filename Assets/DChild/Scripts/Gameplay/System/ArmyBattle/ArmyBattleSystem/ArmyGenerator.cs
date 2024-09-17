using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyGenerator : MonoBehaviour
    {

        public Army GenerateArmy(ArmyData armyData) => new Army(armyData.info);

    }
}