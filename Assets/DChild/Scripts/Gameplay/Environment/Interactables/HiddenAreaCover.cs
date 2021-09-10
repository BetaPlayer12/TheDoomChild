/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Serialization;
using Holysoft;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class HiddenAreaCover : MonoBehaviour, ISerializableComponent
    {
        [SerializeField, Range(0, 1)]
        private float m_invisibleAlpha;
        [SerializeField]
        private LerpDuration m_lerpDuration;
        [SerializeField]
        private SpriteRenderer[] m_sprites;
        [SerializeField]
        private SkeletonRenderer[] m_spines;
        [ShowInInspector]
        private bool m_visible;

        private bool m_isInitialized;
        private Coroutine m_currentRoutine;

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isInvisible) : this()
            {
                this.m_isInvisible = isInvisible;
            }

            [SerializeField]
            private bool m_isInvisible;

            public bool isInvisible => m_isInvisible;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isInvisible);
        }

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

        public ISaveData Save() => new SaveData(m_visible);

        public void Load(ISaveData data)
        {
            m_visible = ((SaveData)data).isInvisible;
            if (m_isInitialized)
            {
                var lerpValue = m_visible ? 1 : 0;
                m_lerpDuration.SetValue(lerpValue);
                LerpColors(lerpValue);
            }
        }

        public void SetVisibility(bool isVisible)
        {
            if (m_visible != isVisible)
            {
                if (isVisible)
                {
                    if (m_currentRoutine != null)
                    {
                        StopCoroutine(m_currentRoutine);
                    }

                    m_currentRoutine = StartCoroutine(LerpTo(false));
                }
                else
                {
                    if (m_currentRoutine != null)
                    {
                        StopCoroutine(m_currentRoutine);
                    }

                    m_currentRoutine = StartCoroutine(LerpTo(true));
                }
                m_visible = isVisible;
            }
        }

        public void SetAsVisible(bool isVisible)
        {
            StopAllCoroutines();
            LerpColors(isVisible ? 0 : 1);
            SetRenderersActive(isVisible);
            m_visible = isVisible;
        }

        private IEnumerator LerpTo(bool isVisible)
        {
            int destination = isVisible ? 0 : 1;

            if (isVisible == false)
            {
                SetRenderersActive(true);
            }

            int signModifier = isVisible ? -1 : 1;
            do
            {
                m_lerpDuration.Update(Time.deltaTime * signModifier);
                LerpColors(m_lerpDuration.lerpValue);
                yield return null;
            } while (m_lerpDuration.lerpValue != destination);

            if (isVisible)
            {
                SetRenderersActive(false);
            }
        }

        private void LerpColors(float lerpValue)
        {
            for (int i = 0; i < m_sprites.Length; i++)
            {
                m_sprites[i].color = m_colorInfos[m_sprites[i]].GetValue(lerpValue);
            }

            for (int i = 0; i < m_spines.Length; i++)
            {
                Color color = m_colorInfos[m_spines[i]].GetValue(lerpValue);
                var skeleton = m_spines[i].skeleton;
                m_spines[i].skeleton.SetColor(color);
            }
        }

        private void SetRenderersActive(bool isTrue)
        {
            for (int i = 0; i < m_sprites.Length; i++)
            {
                m_sprites[i].enabled = isTrue;
            }

            //Enable Mesh Renderer for Skeleton Renderers
        }

        private void Awake()
        {
            m_visible = true;
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

            m_isInitialized = true;

            if (m_visible)
            {
                LerpColors(1);
            }
        }
    }
}