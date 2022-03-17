/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class ForceCameraAfterCutscene : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera m_camera;
        [SerializeField]
        private bool m_cutToCamera;

        private void OnCutsceneDone(PlayableDirector obj)
        {
            if (m_cutToCamera)
            {
                StartCoroutine(CutToCameraRoutine(GameplaySystem.cinema.currentBrain));
            }
            m_camera.enabled = true;
            m_camera.MoveToTopOfPrioritySubqueue();
        }

        private IEnumerator CutToCameraRoutine(CinemachineBrain brain)
        {
            var originalTime = brain.m_DefaultBlend.m_Time;
            brain.m_DefaultBlend.m_Time = 0;
            yield return null;
            while (brain.IsBlending)
            {
                yield return null;
            }

            brain.m_DefaultBlend.m_Time = originalTime;
        }

        private void Start()
        {
            GetComponent<PlayableDirector>().stopped += OnCutsceneDone;
        }

    }
}