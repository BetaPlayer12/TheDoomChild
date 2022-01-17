using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Combat.StatusAilment.UI
{
    public class StatusEffectScreenFilterUI : SerializedMonoBehaviour
    {
        [SerializeField]
        private Image m_filter;
        [SerializeField, MinValue(0)]
        private float m_fadeInDuration;
        [SerializeField, MinValue(0)]
        private float m_fadeOutDuration;
        [SerializeField]
        private Dictionary<StatusEffectType, Material> m_filterPair;

        private StatusEffectType m_currentFilter = StatusEffectType._COUNT;
        private bool m_isFilterShown;
        private float m_currentFilterFadeLerp = 0;

        public void ShowFilter(StatusEffectType type)
        {
            if (m_filterPair.TryGetValue(type, out Material material))
            {
                m_filter.enabled = true;
                m_filter.material = material;
                m_currentFilter = type;

                if (m_isFilterShown == false)
                {
                    if (m_currentFilterFadeLerp > 0)
                    {
                        m_currentFilterFadeLerp = Mathf.Abs(m_currentFilterFadeLerp - 1);
                    }

                    StopAllCoroutines();
                    m_currentFilterFadeLerp = 0;
                    StartCoroutine(FadeRoutine(0, 1, m_fadeInDuration));
                    m_isFilterShown = true;
                }
                else
                {
                    var fadeValue = Mathf.Lerp(0, 1, m_currentFilterFadeLerp);
                    SetFadeValue(m_filter, fadeValue);
                }
            }
        }

        public void HideFilter(StatusEffectType type)
        {
            if (m_currentFilter == type)
            {
                //m_filter.enabled = false;
                //m_filter.material = null;

                if (m_isFilterShown)
                {
                    if (m_currentFilterFadeLerp > 0)
                    {
                        m_currentFilterFadeLerp = Mathf.Abs(m_currentFilterFadeLerp - 1);
                    }

                    StopAllCoroutines();
                    m_currentFilterFadeLerp = 0;
                    StartCoroutine(FadeRoutine(1, 0, m_fadeOutDuration));
                    m_isFilterShown = false;
                }
            }
        }

        private IEnumerator FadeRoutine(float from, float to, float duration)
        {
            var speed = Mathf.Abs(to - from) / duration;
            var fadeValue = Mathf.Lerp(from, to, m_currentFilterFadeLerp);
            SetFadeValue(m_filter, fadeValue);
            yield return null;
            do
            {
                m_currentFilterFadeLerp += speed * Time.deltaTime;
                fadeValue = Mathf.Lerp(from, to, m_currentFilterFadeLerp);
                SetFadeValue(m_filter, fadeValue);
                yield return null;
            } while (m_currentFilterFadeLerp < 1);

            m_filter.enabled = to == 1;
        }

        private void SetFadeValue(Image image, float value)
        {
            image.material.SetFloat("_RadialFade", value);
        }

        private void Awake()
        {
            if (m_filter.material)
            {
                SetFadeValue(m_filter, 0);
            }
        }
    }
}