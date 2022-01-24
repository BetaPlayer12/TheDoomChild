using DChild.Gameplay;
using DChild.Menu;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Optimizers
{
    public class RoomActivityManager : MonoBehaviour
    {
        [SerializeField]
        private SceneInfo m_scene;

        private static List<string> m_activeRooms = new List<string>();
        private static event EventAction<EventActionArgs> ForceUnloadRooms;

        private bool m_roomLoaded;

        public static void UnloadAllRooms()
        {
            for (int i = 0; i < m_activeRooms.Count; i++)
            {
            LoadingHandle.UnloadScenes(m_activeRooms[i]);
            }
            m_activeRooms.Clear();
            ForceUnloadRooms?.Invoke(null,EventActionArgs.Empty);
        }

        private void OnForceUnloadRooms(object sender, EventActionArgs eventArgs)
        {
            m_roomLoaded = false;
        }

        private void Awake()
        {
            ForceUnloadRooms += OnForceUnloadRooms;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor"))
            {
                if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
                {
                    LoadRoom();
                }
            }
        }

        [Button]
        private void LoadRoom()
        {
            m_activeRooms.Add(m_scene.sceneName);
            SceneManager.LoadScene(m_scene.sceneName, LoadSceneMode.Additive);
            m_roomLoaded = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

            if (collision.CompareTag("Sensor"))
            {
                if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
                {
                    m_activeRooms.Remove(m_scene.sceneName);
                    SceneManager.UnloadSceneAsync(m_scene.sceneName);
                }
            }

        }

        private void OnDestroy()
        {
            if (m_roomLoaded)
            {
                m_activeRooms.Remove(m_scene.sceneName);
                SceneManager.UnloadSceneAsync(m_scene.sceneName);
            }
        }
    }
   
}
