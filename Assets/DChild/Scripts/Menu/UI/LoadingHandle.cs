using Holysoft.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class LoadingHandle : MonoBehaviour
    {
        [SerializeField]
        private LoadingAnimation m_animation;

        public static string sceneToLoad;
        public static EventAction<EventActionArgs> SceneDone;

        private void Awake()
        {
            m_animation.AnimationEnd += OnAnimationEnd;
        }

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        private void Start()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            m_animation.MonitorProgress(asyncOperation);
        }

        private void OnDestroy()
        {
            m_animation.AnimationEnd -= OnAnimationEnd;
            SceneDone?.Invoke(this, EventActionArgs.Empty);
        }
    }
}