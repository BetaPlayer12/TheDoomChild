/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using Holysoft;
using Holysoft.Collections;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class HiddenArea : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float m_invisibleAlpha;
        [SerializeField]
        private LerpDuration m_lerpDuration;
        [SerializeField]
        private SpriteRenderer[] m_sprites;
        [SerializeField]
        private SkeletonRenderer[] m_spines;

        private Coroutine m_currentRoutine;

        private struct ColorInfo
        {
            private Color visible;
            private Color invisible;

            public ColorInfo(Color visible, float invisibleAlpha)
            {
                this.visible = visible;
                invisible = visible;
                invisible.a = invisibleAlpha;
            }

            public Color GetValue(float lerpValue)
            {
                return Color.Lerp(invisible, visible, lerpValue);
            }
        }

        private Dictionary<Object, ColorInfo> m_colorInfos;

        private void Awake()
        {
            m_colorInfos = new Dictionary<Object, ColorInfo>();
            for (int i = 0; i < m_sprites.Length; i++)
            {
                m_colorInfos.Add(m_sprites[i], new ColorInfo(m_sprites[i].color, m_invisibleAlpha));
            }

            m_lerpDuration.SetValue(1);
        }

        private void Start()
        {
            for (int i = 0; i < m_spines.Length; i++)
            {
                var skeleton = m_spines[i].skeleton;
                Color color = new Color(skeleton.R, skeleton.G, skeleton.B, skeleton.A);
                m_colorInfos.Add(m_spines[i], new ColorInfo(color, m_invisibleAlpha));
            }
        }

        private IEnumerator LerpTo(bool isVisible)
        {
            int destination = isVisible ? 1 : 0;
            int signModifier = isVisible ? 1 : -1;
            do
            {
                m_lerpDuration.Update(Time.deltaTime * signModifier);
                LerpColors();
                yield return null;
            } while (m_lerpDuration.lerpValue != destination);
        }

        private void LerpColors()
        {
            for (int i = 0; i < m_sprites.Length; i++)
            {
                m_sprites[i].color = m_colorInfos[m_sprites[i]].GetValue(m_lerpDuration.lerpValue);
            }

            for (int i = 0; i < m_spines.Length; i++)
            {
                Color color = m_colorInfos[m_spines[i]].GetValue(m_lerpDuration.lerpValue);
                var skeleton = m_spines[i].skeleton;
                m_spines[i].skeleton.SetColor(color);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(m_currentRoutine != null)
            {
                StopCoroutine(m_currentRoutine);
            }

            m_currentRoutine = StartCoroutine(LerpTo(false));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_currentRoutine != null)
            {
                StopCoroutine(m_currentRoutine);
            }

            m_currentRoutine = StartCoroutine(LerpTo(true));
        }
    }
}