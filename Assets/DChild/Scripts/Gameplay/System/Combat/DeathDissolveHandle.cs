using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class DeathDissolveHandle : MonoBehaviour
    {
        [SerializeField]
        private SpineAnimation m_animation;
        [SerializeField]
        private MeshRenderer m_renderer;
        [SerializeField, SpineEvent]
        private string m_startDissolveEvent;
        [SerializeField, Min(0)]
        private float m_dissolveDuration = 1;
        [SerializeField]
        private float m_delay; //YEA

        private int m_dissolveValueID;
        private MaterialPropertyBlock m_propertyBlock;

        [Button, HideInEditorMode]
        private void Execute()
        {
            SetDissolveValue(1f);
            StopAllCoroutines();
            StartCoroutine(DissolveRoutine());
        }

        private void OnAnimationEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_startDissolveEvent)
            {
                Execute();
            }
        }

        private IEnumerator DissolveRoutine()
        {
            yield return new WaitForSeconds(m_delay);
            var lerpValue = 1f;
            var lerpRate = 1f / m_dissolveDuration;
            do
            {
                lerpValue -= GameplaySystem.time.deltaTime * lerpRate;
                SetDissolveValue(lerpValue);
                yield return null;
            } while (lerpValue > 0);
        }

        private void SetDissolveValue(float lerpValue)
        {
            m_renderer.GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetFloat(m_dissolveValueID, lerpValue);
            m_renderer.SetPropertyBlock(m_propertyBlock);
        }

        private void Start()
        {
            m_animation.animationState.Event += OnAnimationEvent;

            m_dissolveValueID = Shader.PropertyToID("_OpacityDissolveControl");
            m_propertyBlock = new MaterialPropertyBlock();

            //m_animation.animationState.Complete += OnAnimationStart; // For Testing Only
        }

        //// For Testing Only
        //private void OnAnimationStart(TrackEntry trackEntry)
        //{
        //    StopAllCoroutines();
        //    SetDissolveValue(1f);
        //}
    }
}
