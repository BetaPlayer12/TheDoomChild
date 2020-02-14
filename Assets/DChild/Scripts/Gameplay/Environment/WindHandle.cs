using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WindHandle : MonoBehaviour
    {
        [SerializeField,OnValueChanged("UpdateRenderers")]
        private float m_strength;
        [SerializeField, ValueDropdown("GetRenderers", IsUniqueList = true)]
        private Renderer[] m_affectedRenderers;

        private MaterialPropertyBlock m_propertyBlock;

        private bool m_isInterpolating;
        private static string property => "Vector1_WindValue";

        public bool isInterpolating => m_isInterpolating;

        public void SetStrength(float windStrength)
        {
            StopAllCoroutines();
            m_isInterpolating = false;
            m_strength = windStrength;
            UpdateRenderers();
        }

        public void SetStrength(float windStrength, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(StrengthInterpolationRoutine(windStrength, duration));
        }

        private IEnumerator StrengthInterpolationRoutine(float windStrength, float duration)
        {
            var prevStrength = m_strength;
            var lerpValue = 0f;
            var speed = 1 / duration;
            m_isInterpolating = true;
            do
            {
                lerpValue += speed * GameplaySystem.time.deltaTime;
                m_strength = Mathf.Lerp(prevStrength, windStrength, lerpValue);
                UpdateRenderers();
                yield return null;
            } while (lerpValue < 1);
            m_isInterpolating = false;
        }

        private void UpdateRenderers()
        {
            for (int i = 0; i < m_affectedRenderers.Length; i++)
            {
                m_affectedRenderers[i].GetPropertyBlock(m_propertyBlock);
                m_propertyBlock.SetFloat(property, m_strength);
                m_affectedRenderers[i].SetPropertyBlock(m_propertyBlock);
            }
        }

        private void Start()
        {
            m_propertyBlock = new MaterialPropertyBlock();
            UpdateRenderers();
        }


#if UNITY_EDITOR
        private IEnumerable GetRenderers()
        {
            Func<Transform, string> getPath = null;
            getPath = x => (x ? getPath(x.parent) + "/" + x.gameObject.name : "");
            return FindObjectsOfType<Renderer>().Select(x => new ValueDropdownItem(getPath(x.transform), x));
        }
#endif
    }
}