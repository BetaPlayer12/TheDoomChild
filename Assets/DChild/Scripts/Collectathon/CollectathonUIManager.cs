using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using DChild.Gameplay.NavigationMap;
using DChild.Gameplay.Environment;
using Location = DChild.Gameplay.Environment.Location;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor;

namespace DChild.UI
{
    public class CollectathonUIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_currentMapName;
        [SerializeField]
        private TextMeshProUGUI m_seedsCount;
        [SerializeField]
        private TextMeshProUGUI m_treasureChestCount;
        [SerializeField]
        private TextMeshProUGUI m_soulShardCount;

        [SerializeField]
        private NavMapInstantiator m_currentLocation;

        private CollectathonValues m_collectathonValues;

        private void SetCollectathonDetails(Location currentLocation)
        {
            //CreateVariableName(currentLocation, EnumType, VarType);
            //CreateVariableName(currentLocation, EnumType, VarType)

            switch (currentLocation)
            {
                case Location.City_Of_The_Dead:
                    {

                        //m_currentLocationSeedsCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
                        //m_currentLocationSeedsTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

                        //m_currentLocationTreasureChestCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
                        //m_currentLocationTreasureChestCTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

                        //m_currentLocationSoulShardCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
                        //m_currentLocationSoulSharTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;
                        break;
                    }
                case Location.Graveyard:
                    {

                        break;
                    }
                case Location.Unholy_Forest:
                    {

                        break;
                    }
                case Location.Garden:
                    {

                        break;
                    }
                case Location.Prison:
                    {

                        break;
                    }
                case Location.Laboratory:
                    {

                        break;
                    }
                case Location.Library:
                    {

                        break;
                    }
                case Location.Throne_Room:
                    {

                        break;
                    }
                case Location.Realm_Of_Nightmare:
                    {

                        break;
                    }
                case Location.Temple_Of_The_One:
                    {

                        break;
                    }
            }
        }

        [Button]
        private void AddSeedsCOTDValue()
        {
            int seeds = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            int seedSum = seeds += 1;
            DialogueLua.SetVariable("SeedsOfTheOne_Count_COTD", seedSum);
        }

        [Button]
        public void ShowCollectathonDetails()
        {
            int seedCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            int seedTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            int treasureChestCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            int treasureChestTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            int soulShardCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            int soulShardTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            m_currentMapName.SetText(m_currentLocation.currentMap.ToString().Replace('_', ' '));
            m_seedsCount.SetText($"{seedCount} / {seedTotal}");
            m_treasureChestCount.SetText($"{treasureChestCount} / {treasureChestTotal}");
            m_soulShardCount.SetText(soulShardCount + " / " + soulShardTotal);
        }
    }

}
