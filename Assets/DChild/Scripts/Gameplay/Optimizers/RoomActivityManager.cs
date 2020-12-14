using DChild.Gameplay;
using DChild.Menu;
using Holysoft.Collections;
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

        public static void UnloadAllRooms()
        {
            for (int i = 0; i < m_activeRooms.Count; i++)
            {
            LoadingHandle.UnloadScenes(m_activeRooms[i]);

            }
            m_activeRooms.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor"))
            {
                if (collision.GetComponentInParent<Character>() == GameplaySystem.playerManager.player.character)
                {
                    m_activeRooms.Add(m_scene.sceneName);
                    SceneManager.LoadScene(m_scene.sceneName, LoadSceneMode.Additive);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {

            if (collision.CompareTag("Sensor"))
            {
                if (collision.GetComponentInParent<Character>() == GameplaySystem.playerManager.player.character)
                {
                    m_activeRooms.Remove(m_scene.sceneName);
                    SceneManager.UnloadSceneAsync(m_scene.sceneName);
                }
            }

        }
    }
   
}
