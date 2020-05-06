using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.UI.Map;
using DChild.Menu;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class WorldMapHandler : MonoBehaviour
    {
        [SerializeField]
        private AreaTransferData m_transferData;
        [SerializeField]
        private AreaNode[] m_nodes;
        private LocationData m_from;

        public void SetFromLocation(LocationData from)
        {
            m_from = from;
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