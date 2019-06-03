/*****************************************
 * 
 * Handles How a Cutscene  Starts, Play and End
 * 
 *****************************************/

using UnityEngine.Playables;
using System.Linq;
using UnityEngine;
using Holysoft.Event;

namespace DChild.Gameplay.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public abstract class Cutscene : MonoBehaviour
    {
        public event EventAction<EventActionArgs> CutsceneEnd;

        protected PlayableDirector m_director;

        public virtual void Play()
        {
            GameplaySystem.PlayCutscene(this);
            gameObject.SetActive(true);
        }

        public virtual void Pause()
        {
            gameObject.SetActive(false);
            m_director.Pause();
        }

        public virtual void Stop()
        {
            GameplaySystem.StopCutscene(this);
            CutsceneEnd?.Invoke(this, EventActionArgs.Empty);
            m_director.Stop();
            gameObject.SetActive(false);
        }

        public abstract void SetAsComplete();
        public abstract void InitializeScene();

        private void Awake() => m_director = GetComponent<PlayableDirector>();

        protected virtual void Start()
        {
            var camera = GameplaySystem.cinema.mainCamera.gameObject;
            var cutsceneCam = m_director.playableAsset.outputs.First(c => c.streamName == "Cinemachine Track");
            m_director.SetGenericBinding(cutsceneCam.sourceObject, camera);
        }

        private void Update()
        {
            if (m_director.state != PlayState.Playing)
            {
                Stop();
            }
        }
    }
}