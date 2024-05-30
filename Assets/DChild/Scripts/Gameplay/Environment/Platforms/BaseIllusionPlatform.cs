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
        private GameObject m_worldInteraction;
        [SerializeField]
        private Collider2D m_environmentCollider;
        [SerializeField]
        private List<MaterialParameterCall> m_materialParameterCall;
        [Button]
        public override void Appear(bool instant)
        {
            m_environmentCollider.enabled = true;
            AppearEffect();
            if (m_worldInteraction == null)
            {
                return;
            }
            m_worldInteraction.SetActive(true);
        }

        [Button]
        public override void Disappear(bool instant)
        {
            m_environmentCollider.enabled = false;
            DissapearEffect();
            if (m_worldInteraction == null)
            {
                return;
            }
            m_worldInteraction.SetActive(false);
            
        }

        private void DissapearEffect()
        {
            for (int i = 0; i < m_materialParameterCall.Count; i++)
            {

                var individualMaterialParamCall = m_materialParameterCall[i];
                individualMaterialParamCall.SetLerpSpeed(m_setLerpSpeed);
                individualMaterialParamCall.SetValue(true);
                individualMaterialParamCall.LerpValue(m_lerpValue);
                //m_materialParameterCall.SetLerpSpeed(m_setLerpSpeed);
                //m_materialParameterCall.SetValue(true);
                //m_materialParameterCall.LerpValue(m_lerpValue);
            }
        }

        private void AppearEffect()
        {
                for (int i = 0; i < m_materialParameterCall.Count; i++)
                {
                    var individualMaterialParamCall = m_materialParameterCall[i];
                    individualMaterialParamCall.SetLerpSpeed(m_setLerpSpeed);
                    individualMaterialParamCall.SetValue(false);
                    individualMaterialParamCall.LerpValue(1f);
                }
            //    m_materialParameterCall.SetLerpSpeed(m_setLerpSpeed);
            //m_materialParameterCall.SetValue(false);
            //m_materialParameterCall.LerpValue(1f);
        }
    }
}
