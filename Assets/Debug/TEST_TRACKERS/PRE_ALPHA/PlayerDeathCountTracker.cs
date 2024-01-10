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
            [System.Serializable]
            public class Info
            {
                public string scene;
                public int count;

                public Info(string scene, int count)
                {
                    this.scene = scene;
                    this.count = count;
                }
            }

            [SerializeField]
            private Info[] m_infos;
            public SaveData(Dictionary<string, int> sceneDeathCountPair)
            {
                m_infos = new Info[sceneDeathCountPair.Count];
                int i = 0;
                foreach (var item in sceneDeathCountPair)
                {
                    m_infos[i] = new Info(item.Key, item.Value);
                    i++;
                }
            }

            public Info[] infos => m_infos;
        }


        [SerializeField]
        private Dictionary<string, int> m_sceneDeathCountPair;
        public SaveData Save() => new SaveData(m_sceneDeathCountPair);

        public void Load(SaveData data)
        {
            if (m_sceneDeathCountPair == null)
            {
                m_sceneDeathCountPair = new Dictionary<string, int>();
            }

            m_sceneDeathCountPair.Clear();
            var infos = data.infos;
            foreach (var info in infos)
            {
                m_sceneDeathCountPair.Add(info.scene, info.count);
            }
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
            if (m_sceneDeathCountPair == null)
            {
                m_sceneDeathCountPair = new Dictionary<string, int>();
            }
            GameplaySystem.playerManager.player.damageableModule.Destroyed += OnPlayerDeath;
        }

        private void OnDestroy()
        {
            GameplaySystem.playerManager.player.damageableModule.Destroyed -= OnPlayerDeath;
        }
    }

}