using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Environment
{
    public class BaseIllusionPlatform : IllusionPlatform
    {
        [SerializeField]
        private float m_setLerpSpeed;
        [SerializeField]
        private float m_lerpValue;
        [SerializeField]
        private MaterialParameterCall m_materialParameterCall;
        [Button]
        public override void Appear(bool instant)
        {
            //gameObject.SetActive(true);
            AppearEffect();
        }


        [Button]
        public override void Disappear(bool instant)
        {
            // gameObject.SetActive(false);
            DissapearEffect();
        }

        private void DissapearEffect()
        {
            m_materialParameterCall.SetLerpSpeed(m_setLerpSpeed);
            m_materialParameterCall.SetValue(true);
            m_materialParameterCall.LerpValue(m_lerpValue);
        }

        private void AppearEffect()
        {
            m_materialParameterCall.SetLerpSpeed(m_setLerpSpeed);
            m_materialParameterCall.SetValue(false);
            m_materialParameterCall.LerpValue(1f);
        }
    }
}
