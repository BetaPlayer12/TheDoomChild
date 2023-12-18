using DChild.Gameplay;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Testing.PreAlpha
{
    public class PlayerDeathCountTracker : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class SaveData
        {

        }

        public SaveData Save() => null;

        public void Load(SaveData data)
        {

        }

        private Dictionary<string, int> m_sceneDeathCountPair;

        private void OnPlayerDeath(object sender, EventActionArgs eventArgs)
        {
            var sceneName = FindObjectOfType<ZoneDataHandle>().gameObject.scene.name;
            if (m_sceneDeathCountPair.ContainsKey(sceneName))
            {
                m_sceneDeathCountPair[sceneName] += 1;
            }
            else
            {
                m_sceneDeathCountPair.Add(sceneName, 1);
            }
        }

        private void Start()
        {
            m_sceneDeathCountPair = new Dictionary<string, int>();
            GameplaySystem.playerManager.player.damageableModule.Destroyed += OnPlayerDeath;
        }
    }

}