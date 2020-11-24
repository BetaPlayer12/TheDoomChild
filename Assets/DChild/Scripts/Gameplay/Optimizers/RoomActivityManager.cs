using DChild.Gameplay;
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


        private void OnTriggerEnter2D(Collider2D collision)
        {
            // if(collision.CompareTag("Sensor")){
            //if (collision.GetComponentInParent<Character> == GameplaySystem.playerManager.player.character)
            // {
            SceneManager.LoadScene(m_scene.sceneName, LoadSceneMode.Additive);
            //} 
            //}
        }
        private void OnTriggerExit2D(Collider2D collision)
        {

            // if (collision.CompareTag("Sensor"))
            //{
            //if (collision.GetComponentInParent<Character> == GameplaySystem.playerManager.player.character)
            // {
            SceneManager.UnloadSceneAsync(m_scene.sceneName);
            // }
            // } 

        }


    }
}
