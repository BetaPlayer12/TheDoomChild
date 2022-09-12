/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class AfterCutsceneActions : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent m_actions;
        private void OnCutsceneDone(PlayableDirector obj)
        {
            m_actions?.Invoke();
        }

        private void Start()
        {
            GetComponent<PlayableDirector>().stopped += OnCutsceneDone;
        }

    }
}