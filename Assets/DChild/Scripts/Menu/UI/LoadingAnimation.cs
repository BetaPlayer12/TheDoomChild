using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Menu
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        public event EventAction<EventActionArgs> AnimationEnd;

        public void MonitorProgress(AsyncOperation loadingOperation)
        {
            StartCoroutine(AnimationRoutine(loadingOperation));
        }

        private IEnumerator AnimationRoutine(AsyncOperation loadingOperation)
        {
            m_animator.SetTrigger("Start");
            yield return null;
            loadingOperation.allowSceneActivation = false;
            while (loadingOperation.progress < 0.9f)
            {
                yield return null;
            }
            m_animator.SetTrigger("End");
            Debug.Log("End Animation");
            yield return new WaitForSeconds(2.25f);
            loadingOperation.allowSceneActivation = true;
            AnimationEnd?.Invoke(this, EventActionArgs.Empty);
        }
    }
}