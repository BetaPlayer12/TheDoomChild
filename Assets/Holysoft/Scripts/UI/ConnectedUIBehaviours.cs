using System;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public class ConnectedUIBehaviours : UICanvasComponent
    {
        [SerializeField]
        private CanvasTransistion m_transistion;
        [SerializeField]
        private UIBehaviour[] m_behaviourOnShow;
        [SerializeField]
        [ShowIf("m_hasTransistion")]
        private UIBehaviour[] m_behaviourOnTransistionEnd;


        protected override void OnHide(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < (m_behaviourOnShow?.Length ?? 0); i++)
            {
                m_behaviourOnShow[i].Disable();
            }

            for (int i = 0; i < (m_behaviourOnTransistionEnd?.Length ?? 0); i++)
            {
                m_behaviourOnTransistionEnd[i].Disable();
            }
        }

        protected override void OnShow(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < (m_behaviourOnShow?.Length ?? 0); i++)
            {
                m_behaviourOnShow[i].Enable();
            }
        }

        private void OnEnterTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < (m_behaviourOnTransistionEnd?.Length ?? 0); i++)
            {
                m_behaviourOnTransistionEnd[i].Enable();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (m_transistion)
            {
                m_transistion.EnterTransistionEnd += OnEnterTransistionEnd;
            }
        }




#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private bool m_hasTransistion;
#endif
        private void OnValidate()
        {
#if UNITY_EDITOR
            m_hasTransistion = m_transistion; 
#endif
        }
    }
}