using DChild.Gameplay.Environment;
using DChild.Serialization;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/SavePoint")]
    public class SavePoint : MonoBehaviour
    {
        private SceneInfo m_sceneInfo;
        [SerializeField]
        private Location m_location;
        [SerializeField]
        private Vector2 m_spawnPosition;

        [Button]
        public void SaveGame()
        {
            GameplaySystem.playerManager.player.Revitilize();

            //#if UNITY_EDITOR
            if (m_dontActuallySave)
                return;
            //#endif
            GameplaySystem.campaignSerializer.slot.UpdateLocation(m_sceneInfo, m_location, m_spawnPosition);
            GameplaySystem.campaignSerializer.Save(SerializationScope.Gameplay);
        }
        public void SaveGameAfterZoneInitialize()
        {
            StopAllCoroutines();
            StartCoroutine(SaveGameAfterZoneInitializeRoutine());
        }

        private IEnumerator SaveGameAfterZoneInitializeRoutine()
        {
            var zoneDataHandle = FindObjectOfType<ZoneDataHandle>();
            while (zoneDataHandle.hasLoaded == false)
            {
                yield return null;
            }
            SaveGame();

        }

        private void Awake()
        {
            m_sceneInfo = new SceneInfo(gameObject.scene);
        }


        private void OnValidate()
        {
            m_spawnPosition = transform.position;
        }

        //#if UNITY_EDITOR
        [SerializeField]
        private bool m_dontActuallySave;
        //#endif
    }
}