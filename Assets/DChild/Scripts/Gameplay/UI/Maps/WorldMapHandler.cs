using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.UI.Map;
using DChild.Menu;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class WorldMapHandler : MonoBehaviour
    {
        [SerializeField]
        private AreaTransferData m_transferData;
        [ShowInInspector, OnValueChanged("SetFromLocation")]
        private LocationData m_from;
        [SerializeField]
        private AreaNode[] m_nodes;

        public void SetFromLocation(LocationData from)
        {
            m_from = from;
            if (from == null)
            {
                for (int i = 0; i < m_nodes.Length; i++)
                {
                    m_nodes[i].Show();
                }
            }
            else
            {
                var availableLocations = m_transferData.GetAvailableLocationFrom(from);
                for (int i = 0; i < m_nodes.Length; i++)
                {
                    if (availableLocations.Contains(m_nodes[i].location) || from.location == m_nodes[i].location)
                    {
                        m_nodes[i].Show();
                    }
                    else
                    {
                        m_nodes[i].Hide();
                    }
                }
            }
        }

        public void GoTo(AreaNode areaNode)
        {
            var destination = m_transferData.GetDestination(m_from, areaNode.location);
            if (destination)
            {
                Cache<LoadZoneFunctionHandle> cacheLoadZoneHandle = Cache<LoadZoneFunctionHandle>.Claim();
                var player = GameplaySystem.playerManager.player;
                cacheLoadZoneHandle.Value.Initialize(destination, player.character, cacheLoadZoneHandle);
                LoadingHandle.SetLoadType(LoadingHandle.LoadType.Smart);
                GameSystem.LoadZone(destination.scene, true, cacheLoadZoneHandle.Value.CallLocationArriveEvent);
                GameplaySystem.ClearCaches();
            }
            //Close UI After
        }
    }
}