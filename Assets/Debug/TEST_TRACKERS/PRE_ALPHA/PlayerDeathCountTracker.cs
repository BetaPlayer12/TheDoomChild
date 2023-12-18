using DChild.Gameplay;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{
    [AddComponentMenu(PreAlphaUtility.COMPONENTMENU_ADDRESS + "PlayerDeathCountTracker")]
    public class PlayerDeathCountTracker : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class SaveData
        {
            private Dictionary<string, int> m_sceneDeathCountPair;

            public SaveData(Dictionary<string, int> sceneDeathCountPair)
            {
                this.m_sceneDeathCountPair = sceneDeathCountPair;
            }

            public Dictionary<string, int> sceneDeathCountPair => m_sceneDeathCountPair;
        }


        [SerializeField]
        private Dictionary<string, int> m_sceneDeathCountPair;
        public SaveData Save() => new SaveData(m_sceneDeathCountPair);

        public void Load(SaveData data)
        {

        }

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

        private void OnDestroy()
        {
            GameplaySystem.playerManager.player.damageableModule.Destroyed -= OnPlayerDeath;
        }
    }

}