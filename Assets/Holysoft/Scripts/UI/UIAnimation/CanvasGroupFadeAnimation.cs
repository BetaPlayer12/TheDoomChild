using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public class CanvasGroupFadeAnimation : UIAnimation
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup;
        [SerializeField]
        protected LerpDuration m_lerp;
        [SerializeField]
        private bool m_startAsFaded;
        [SerializeField]
        private bool m_returnToStart;
        private bool m_toFade;

        public event EventAction<EventActionArgs> FadeInComplete;
        public event EventAction<EventActionArgs> FadeOutComplete;

        public override void Pause()
        {
            enabled = false;
        }

        public override void Play()
        {
            enabled = true;
        }

        public override void Stop()
        {
            enabled = false;
            m_lerp.SetValue(m_startAsFaded ? 0 : 1);
            m_canvasGroup.alpha = Mathf.Lerp(0, 1, m_lerp.lerpValue);
            m_canvasGroup.interactable = !m_startAsFaded;
            m_toFade = !m_startAsFaded;
        }

        private void EndAnimation()
        {
            enabled = false;
            m_canvasGroup.interactable = true;
            CallAnimationEnd();
        }

        private void FadeIn()
        {
            m_lerp.Update(Time.unscaledDeltaTime);
            m_canvasGroup.alpha = Mathf.Lerp(0, 1, m_lerp.lerpValue);
            if (m_lerp.lerpValue == 1)
            {
                m_toFade = true;
                FadeInComplete?.Invoke(this, EventActionArgs.Empty);
                if (m_returnToStart)
                {
                    if (m_startAsFaded == false)
                    {
                        if (m_repeat)
                        {
                            CallAnimationLoopReached();
                        }
                        else
                        {
                            EndAnimation();
                        }
                    }
                }
                else
                {
                    if (m_repeat)
                    {
                        CallAnimationLoopReached();
                    }
                    else
                    {
                        EndAnimation();
                    }
                }
            }
        }



        private void FadeOut()
        {
            m_lerp.Update(-Time.unscaledDeltaTime);
            m_canvasGroup.alpha = Mathf.Lerp(0, 1, m_lerp.lerpValue);
            if (m_lerp.lerpValue == 0)
            {
                m_toFade = false;
                FadeOutComplete?.Invoke(this, EventActionArgs.Empty);
                if (m_returnToStart)
                {
                    if (m_startAsFaded)
                    {
                        if (m_repeat)
                        {

                            CallAnimationLoopReached();
                        }
                        else
                        {
                            EndAnimation();
                        }
                    }
                }
                else
                {
                    if (m_repeat)
                    {
                        CallAnimationLoopReached();
                    }
                    else
                    {
                        EndAnimation();
                    }
                }
            }
        }

        private void Start()
        {
            m_toFade = !m_startAsFaded;
            m_lerp.SetValue(m_startAsFaded ? 0 : 1);
        }

        private void Update()
        {
            if (m_canvasGroup.interactable)
            {
                m_canvasGroup.interactable = false;
            }
            if (m_toFade)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_canvasGroup);
        }
    }

}