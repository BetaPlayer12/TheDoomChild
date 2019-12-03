using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
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
            GameplaySystem.campaignSerializer.slot.UpdateLocation(m_sceneInfo, m_location, m_spawnPosition);
            GameplaySystem.campaignSerializer.Save();
            GameplaySystem.playerManager.player.health.ResetValueToMax();
            GameplaySystem.playerManager.player.magic.ResetValueToMax();
        }

        private void Awake()
        {
            m_sceneInfo = new SceneInfo(gameObject.scene);
        }


        private void OnValidate()
        {
            Vector2 position = transform.position;
            if (m_spawnPosition != position)
            {
                m_spawnPosition = position;
            }
        }
    }
}