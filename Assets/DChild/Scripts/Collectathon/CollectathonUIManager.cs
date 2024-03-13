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
        private TextMeshProUGUI m_normalChestCount;
        [SerializeField]
        private TextMeshProUGUI m_treasureChestCount;
        [SerializeField]
        private TextMeshProUGUI m_soulShardCount;

        [SerializeField]
        private Location m_currentLocation;
        [SerializeField]
        private CollectathonValues m_collectathonValues;

        public void SetCollectathonDetails(Location currentLocation)
        {
             //Set current seeds, normal chest, treasure chest, and soul shard count and location name
             m_currentLocation = currentLocation;


        }

        [Button]
        public void ShowCollectathonDetails()
        {
            //int seedCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            //int seedTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            //int treasureChestCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            //int treasureChestTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            //int soulShardCount = DialogueLua.GetVariable("SeedsOfTheOne_Count_COTD").asInt;
            //int soulShardTotal = DialogueLua.GetVariable("SeedsOfTheOne_Total_COTD").asInt;

            m_currentMapName.SetText(m_currentLocation.ToString().Replace('_', ' '));
            //m_seedsCount.SetText($"{seedCount} / {seedTotal}");
            //m_treasureChestCount.SetText($"{treasureChestCount} / {treasureChestTotal}");
            //m_soulShardCount.SetText(soulShardCount + " / " + soulShardTotal);
        }
    }

}
