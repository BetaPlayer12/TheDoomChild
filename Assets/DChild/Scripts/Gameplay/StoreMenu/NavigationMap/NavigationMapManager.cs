using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap.MapLegend;
using DChild.Gameplay.UI;
using DChild.Gameplay.UI.Map;
using DChild.UI;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapManager : MonoBehaviour
    {
        [SerializeField]
        private NavMapInstantiator m_instantiator;
        [SerializeField]
        private NavMapTracker m_tracker;

        private RectTransform m_currentMap;
        private NavigationMapInstance m_mapInstance;
        [SerializeField]
        private bool m_mapNeedsCompleteUpdate = true;
        [SerializeField]
        private CollectathonUIManager m_collectathonManager;
        [SerializeField]
        private MapZoomHandler m_zoomHandler;

        [SerializeField, BoxGroup("Map Legend")] private UIContainerUIAnimator m_legendSection;
        [SerializeField, BoxGroup("Map Legend")] private SetTextToTextBox m_toggleIconsPrompt;
        [SerializeField, BoxGroup("Map Legend")] private MapKeyboardPan m_keyboardPan;

        private NavigationMapIconManager m_iconManager;

        public event EventAction<EventActionArgs> OnMapZoom;

        public void UpdateConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            if (m_instantiator.currentMap != location)
            {
                NavigationMapSceneHandle.changes.Clear();
                m_tracker.RemoveUIReferencesFromCurrentMap();
                m_currentMap = m_instantiator.LoadMapFor(location);
                m_zoomHandler.SetupZoom(m_currentMap);
                m_mapNeedsCompleteUpdate = true;
                m_mapInstance = m_currentMap.GetComponentInChildren<NavigationMapInstance>();
                m_iconManager = m_currentMap.GetComponentInChildren<NavigationMapIconManager>();
                m_collectathonManager.SetCollectathonDetails(location);
                m_zoomHandler.SetZoomConstraints(m_mapInstance.minZoom, m_mapInstance.maxZoom);

                m_legendSection.GetComponent<MapLegendUI>().SetLegendList(m_iconManager.legendIcons);
            }

            m_tracker.SetReferencePointPosition(m_currentMap, mapReferencePoint);
            m_tracker.SetInGameTrackReferencePoint(inGameReference);
            m_tracker.SetCalculationOffsets(calculationOffset);
        }

        public void ForceMapUpdateOnNextOpen()
        {
            m_mapNeedsCompleteUpdate = true;
        }

        public void ToggleLegendVisibility(bool visible)
        {
            if (visible)
                m_legendSection.Show();
            else
                m_legendSection.Hide();
        }

        public void CycleLegendPage()
        {
            m_legendSection.GetComponent<MapLegendUI>().bulletHandle.Next();
        }

        public void ToggleMapIconsVisibility(bool willShow)
        {
            m_iconManager.ToggleIconVisibility(willShow);

            var updatedPromptLabel = !willShow ? "Show" : "Hide";
            m_toggleIconsPrompt.SetText($"BUTTONPROMPT {updatedPromptLabel} Icons");
        }

        public void OpenMap()
        {
            EnableKeyboardPan();

            if (m_mapNeedsCompleteUpdate)
            {
                m_mapInstance?.UpdateFogOfWar();
                m_mapNeedsCompleteUpdate = false;
            }
            else
            {
                var changes = NavigationMapSceneHandle.changes;
                //Only update the ones that needs update
                if (changes != null)
                {
                    for (int i = 0; i < changes.fogOfWarChanges; i++)
                    {
                        m_mapInstance.SetFogOfwarState(changes.GetFogOfWarName(i), changes.GetFogOfWarState(i));
                    }
                    changes.Clear();
                }
            }
            m_tracker.UpdateTrackerPosition();
            MoveTrackerToCenter();
            m_collectathonManager.ShowCollectathonDetails();
            m_legendSection.GetComponent<MapLegendUI>().SetLegendList(m_iconManager.legendIcons);
            m_zoomHandler.OnMapZoom += m_iconManager.OnMapZoom;
        }

        private void MoveTrackerToCenter()
        {
            if (m_currentMap == null)
                return;

            m_currentMap.anchoredPosition = -m_tracker.trackerPosition;
        }

        public void HideNavigationMap()
        {
            DisableKeyboardPan();

            if (m_currentMap == null)
            {
                return;
            }

            var showMap = m_currentMap.GetComponent<UIContainer>();
            m_zoomHandler.OnMapZoom -= m_iconManager.OnMapZoom;
            showMap.Hide();
        }

        public void ShowNavigationMap()
        {
            EnableKeyboardPan();

            if (m_currentMap == null)
            {
                return;
            }

            var showMap = m_currentMap.GetComponent<UIContainer>();
            showMap.Show();
        }

        private void EnableKeyboardPan()
        {
            if (m_keyboardPan == null)
            {
                return;
            }

            var playerManager = GameplaySystem.playerManager;
            var playerInput = playerManager?.PlayerInput;
            m_keyboardPan.enabled = m_keyboardPan.BindInput(playerInput);
        }

        private void DisableKeyboardPan()
        {
            if (m_keyboardPan == null)
            {
                return;
            }

            m_keyboardPan.enabled = false;
            m_keyboardPan.UnbindInput();
        }
    }
}
