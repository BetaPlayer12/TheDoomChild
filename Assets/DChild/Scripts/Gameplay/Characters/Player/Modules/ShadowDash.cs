using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity.Examples;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowDash : MonoBehaviour, IDash, IComplexCharacterModule
    {
        [SerializeField]
        private Dash m_dash;
        [SerializeField, HideLabel]
        private ShadowDashStatsInfo m_configuration;
        [SerializeField]
        private ParticleSystem m_shadowFX;
        [SerializeField]
        private Hitbox m_hitbox;
        [SerializeField]
        private Character m_character;

        private ICappedStat m_source;
        private IPlayerModifer m_modifier;
        private Damageable m_damageable;
        private Animator m_animator;
        private bool m_wasUsed;
        private bool m_hasExecuted;
        private int m_animationParameter;
        private SkeletonGhost m_skeletonGhost;

        private Coroutine m_dashRoutine;

        //[ShowInInspector, ReadOnly, HideInEditorMode]
        //protected int sourceRequiredAmount => Mathf.FloorToInt(m_baseSourceRequiredAmount * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement));

        public event EventAction<EventActionArgs> ExecuteModule;
        public event EventAction<EventActionArgs> End;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.magic;
            m_modifier = info.modifier;
            m_damageable = info.damageable;
            m_animator = info.animator;
            m_skeletonGhost = info.skeletonGhost;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode);
        }

        public void SetConfiguration(ShadowDashStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void Cancel()
        {
            //if (m_dashRoutine != null)
            //{
            //    StopCoroutine(m_dashRoutine);
            //    m_dashRoutine = null;
            //}
            m_dash.Cancel();
            GameplaySystem.world.SetShadowColliders(false);
            m_damageable.SetInvulnerability(Invulnerability.None);
            if (m_hasExecuted)
            {
                m_hasExecuted = false;
                //m_hitbox.Enable();
            }
            m_wasUsed = false;

            if (m_shadowFX != null)
            {
                m_shadowFX?.Stop(true);
            }

            m_animator.SetBool(m_animationParameter, false);
            //m_skeletonGhost.enabled = false;

            End?.Invoke(this, EventActionArgs.Empty);
        }

        public bool HaveEnoughSourceForExecution() => GetSourceRequiredAmount() <= m_source.currentValue;

        public void ConsumeSource() => m_source.ReduceCurrentValue(GetSourceRequiredAmount());

        public void HandleCooldown() => m_dash.HandleCooldown();

        public void ResetCooldownTimer() => m_dash.ResetCooldownTimer();

        public void HandleDurationTimer() => m_dash.HandleDurationTimer();

        public bool IsDashDurationOver() => m_dash.IsDashDurationOver();

        public void ResetDurationTimer() => m_dash.ResetDurationTimer();

        public int GetSourceRequiredAmount()
        {
            return Mathf.FloorToInt(m_configuration.baseSourceRequiredAmount * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement));
        }

        public void Execute()
        {
            if (m_wasUsed == false)
            {
                m_hasExecuted = true;
                m_dashRoutine = StartCoroutine(DashRoutine());
                GameplaySystem.world.SetShadowColliders(true);
                m_damageable.SetInvulnerability(Invulnerability.Level_2);
                //m_hitbox.Disable();
                m_wasUsed = true;

                if (m_shadowFX != null)
                {
                    m_shadowFX?.Play(true);
                }

                m_animator.SetBool(m_animationParameter, true);
                //m_skeletonGhost.enabled = true;
                ExecuteModule?.Invoke(this, EventActionArgs.Empty);
            }
            m_dash.Execute();
        }

        public void Reset()
        {
            m_dash.Reset();
        }

        private IEnumerator DashRoutine()
        {
            while (m_hasExecuted)
            {
                if (LookTransform(m_character.centerMass, 5f)?.GetComponent<LocationSwitcher>())
                {
                    Debug.Log("Player Only trigger detected");
                    m_character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                yield return null;
            }
        }

        protected Transform LookTransform(Transform startPoint, float distance)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, distance, false, out hitCount, true);
            Debug.DrawRay(startPoint.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].transform;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(LayerMask.GetMask("PlayerOnly"));
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }
    }
}
