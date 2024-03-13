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
using System.Linq;
using System.Xml.Schema;
using Holysoft.Collections;

namespace DChild.UI
{
    public class CollectathonUIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_currentMapName;
        [SerializeField]
        private TextMeshProUGUI m_seedsCountText;
        [SerializeField]
        private TextMeshProUGUI m_normalChestCountText;
        [SerializeField]
        private TextMeshProUGUI m_soulSkillChestCountText;
        [SerializeField]
        private TextMeshProUGUI m_soulShardCountText;

        [SerializeField]
        private Location m_currentLocation;

        public void SetCollectathonDetails(Location currentLocation)
        {
            //Set current seeds, normal chest, treasure chest, and soul shard count and location name
            m_currentLocation = currentLocation;
        }

        [Button]
        public void ShowCollectathonDetails()
        {
            m_currentMapName.SetText(m_currentLocation.ToString().Replace('_', ' '));

            m_seedsCountText.SetText($"{DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentCountVariableName(CollectathonTypes.SeedsOfTheOne, m_currentLocation)).asInt}" +
                $" / {DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentTotalVariableName(CollectathonTypes.SeedsOfTheOne, m_currentLocation)).AsInt}");
            m_normalChestCountText.SetText($"{DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentCountVariableName(CollectathonTypes.NormalChest, m_currentLocation)).AsInt} /" +
                $" {DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentTotalVariableName(CollectathonTypes.NormalChest, m_currentLocation)).AsInt}");
            m_soulSkillChestCountText.SetText($"{DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentCountVariableName(CollectathonTypes.SoulSkillChest, m_currentLocation)).asInt}" +
                $" / {DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentTotalVariableName(CollectathonTypes.SoulSkillChest, m_currentLocation)).AsInt}");
            m_soulShardCountText.SetText($"{DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentCountVariableName(CollectathonTypes.ShardChest, m_currentLocation)).AsInt} /" +
                $" {DialogueLua.GetVariable(CollecathonUtility.GenerateCurrentTotalVariableName(CollectathonTypes.ShardChest, m_currentLocation)).AsInt}");
        }
    }

}
