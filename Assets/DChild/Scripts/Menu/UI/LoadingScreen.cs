using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private Canvas m_canvas;

        public void LoadScene(string sceneName)
        {
            m_canvas.enabled = true;
            m_animator.enabled = true;
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        public void CloseCanvas()
        {
            StopAllCoroutines();
            m_canvas.enabled = false;
            m_animator.enabled = false;
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            m_animator.SetTrigger("Start");
            var sceneProgess = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (sceneProgess.progress < 1)
            {

                yield return null;
            }
            m_animator.SetTrigger("End");
        }
    }
}