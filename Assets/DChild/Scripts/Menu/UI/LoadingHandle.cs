using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class LoadingHandle : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            m_animator.gameObject.SetActive(true);
            m_animator.SetTrigger("Start");
            var sceneProgess = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            yield return null;
            sceneProgess.allowSceneActivation = false;
            while (sceneProgess.progress < 0.9f)
            {
                yield return null;

            }
            m_animator.SetTrigger("End");
            yield return new WaitForSeconds(2.5f);
            sceneProgess.allowSceneActivation = true;
        }
    }
}