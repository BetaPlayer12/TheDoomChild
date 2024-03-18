using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class BlackBloodWall : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("References")]
        private Damageable m_damageable;
        [SerializeField]
        private GameObject m_wall;
        [SerializeField, FoldoutGroup("References")]
        private SpineAnimation m_eyeAnimator;
        [SerializeField]
        private ParticleSystem m_deathFX;
        [SerializeField]
        private ParticleSystem m_onDamageFX;
        [SerializeField, FoldoutGroup("References")]
        private SpriteRenderer[] m_renders;


        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_flinchAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_idleAnimation;
        [SerializeField]
        private float m_flinchDissolveThreshold = 0.5f;
        [SerializeField]
        private float m_flinchDissolveDuration = 0.5f;
        [SerializeField]
        private float m_deathDuration = 3;

        private MaterialPropertyBlock m_renderPropertyBlock;
        private int m_dissolveControlID;

        private IEnumerator DissolveRoutine(float dissolveValueDestination, float duration)
        {
            //Since All Renders will morelikely have the same value for Dissolve Control, we just need to use 1 reference for the Start Value 
            var render = m_renders[0];
            render.GetPropertyBlock(m_renderPropertyBlock);
            float m_renderDissolveStartValue = m_renderPropertyBlock.GetFloat(m_dissolveControlID);
            var time = 0f;

            do
            {
                time += GameplaySystem.time.deltaTime;
                var dissolveValue = Mathf.Lerp(m_renderDissolveStartValue, dissolveValueDestination, time / duration);
                for (int i = 0; i < m_renders.Length; i++)
                {
                    SetDissolveControl(m_renders[i], dissolveValue);
                }
                yield return null;
            } while (time < m_deathDuration);
            for (int i = 0; i < m_renders.Length; i++)
            {
                SetDissolveControl(m_renders[i], dissolveValueDestination);
            }

	
	 if(dissolveValueDestination <= 0){
            m_wall.SetActive(false);
}
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            m_deathFX?.Play(true);
            StopAllCoroutines();
            StartCoroutine(DissolveRoutine(0, m_deathDuration));
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_onDamageFX?.Play(true);
            if (m_damageable.health.currentValue > 0)
            {
                m_eyeAnimator.SetAnimation(0, m_flinchAnimation, false);
                m_eyeAnimator.AddAnimation(0, m_idleAnimation, true, 0);

                var health = m_damageable.health;
                var percentHealth = (float)health.currentValue / health.maxValue;
                var dissolveControlValue = Mathf.Lerp(m_flinchDissolveThreshold, 1, percentHealth);
                StopAllCoroutines();
                StartCoroutine(DissolveRoutine(dissolveControlValue, m_flinchDissolveDuration));
            }
        }

        private void SetDissolveControl(SpriteRenderer renderer, float value)
        {
            renderer.GetPropertyBlock(m_renderPropertyBlock);
            m_renderPropertyBlock.SetFloat(m_dissolveControlID, value);
            renderer.SetPropertyBlock(m_renderPropertyBlock);
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void ForcedTakeDamage()
        {
            m_damageable.TakeDamage(1, DamageType.True);
        }
#endif

        private void Awake()
        {
            m_damageable.DamageTaken += OnDamageTaken;
            m_damageable.Destroyed += OnDeath;
            m_renderPropertyBlock = new MaterialPropertyBlock();
            m_dissolveControlID = Shader.PropertyToID("_DissolveControl");
        }

        private void Start()
        {
            //Need to do this since the first Dissolve apparently return 0 as starting even if it was set as 1
            for (int i = 0; i < m_renders.Length; i++)
            {
                SetDissolveControl(m_renders[i], 1);
            }
        }
    }

}