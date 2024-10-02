using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBannerUI : MonoBehaviour
    {
        public void Display(ArmyOverviewData overviewData)
        {
            Debug.Log($"ArmyBattle: Displaying {overviewData.name} in Banner");
        }
    }
}