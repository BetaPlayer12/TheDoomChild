using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SpectreSpine")]
    public class SpectreSpineAI : CombatAIBrain<SpectreSpineAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            #region VerticalStab
            [SerializeField]
            private SimpleAttackInfo m_verticalStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo verticalStabAttack => m_verticalStabAttack;

            [SerializeField]
            private BasicAnimationInfo m_verticalStabFakeOut;
            public BasicAnimationInfo verticalStabFakeOut => m_verticalStabFakeOut;

            [SerializeField]
            private BasicAnimationInfo m_verticalStabStart;
            public BasicAnimationInfo verticalStabStart => m_verticalStabStart;
            [SerializeField]
            private BasicAnimationInfo m_verticalStabEnd;
            public BasicAnimationInfo verticalStabEnd => m_verticalStabEnd;

            #endregion

            #region ChargedStab
            [SerializeField]
            private SimpleAttackInfo m_chargeStabAattack = new SimpleAttackInfo();
            public SimpleAttackInfo ChargeStabAttack => m_chargeStabAattack;
            [SerializeField]
            private BasicAnimationInfo m_attackCharge;
            public BasicAnimationInfo attackCharge => m_attackCharge;
            [SerializeField,]
            private BasicAnimationInfo m_chargeAttackEnd;
            public BasicAnimationInfo chargeAttackEnd => m_chargeAttackEnd;
            [SerializeField]
            private BasicAnimationInfo m_chargeAttackAnticipation;
            public BasicAnimationInfo chargeAttackAnticipation => m_chargeAttackAnticipation;
            [SerializeField]
            private BasicAnimationInfo m_chargeAttackAimComplete;
            public BasicAnimationInfo chargeAttackAimComplete => m_chargeAttackAimComplete;
            #endregion
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;

            [SerializeField]
            private float m_swingNumber;
            public float swingNumber => m_swingNumber;

            [SerializeField]
            private float m_swingDuration;
            public float swingDuration => m_swingDuration;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fadeInAnimation;
            public BasicAnimationInfo fadeInAnimation => m_fadeInAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fadeOutAnimation;
            public BasicAnimationInfo fadeOutAnimation => m_fadeOutAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_verticalStabAttack.SetData(m_skeletonDataAsset);
                m_chargeStabAattack.SetData(m_skeletonDataAsset);

                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_fadeInAnimation.SetData(m_skeletonDataAsset);
                m_fadeOutAnimation.SetData(m_skeletonDataAsset);
                m_attackCharge.SetData(m_skeletonDataAsset);
                m_chargeAttackAimComplete.SetData(m_skeletonDataAsset);
                m_chargeAttackAnticipation.SetData(m_skeletonDataAsset);
                m_chargeAttackEnd.SetData(m_skeletonDataAsset);
                m_verticalStabFakeOut.SetData(m_skeletonDataAsset);
                m_verticalStabEnd.SetData(m_skeletonDataAsset);
                m_verticalStabStart.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Detect,
            ReturnToPatrol,
            Patrol,
            Idle,
            Cooldown,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            VerticalStab,
            ChargeAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_chargedAttackBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_stabAttackBB;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Attacker m_chargedAttackAttacker;
        [SerializeField, TabGroup("Reference")]
        private Attacker m_stabAttackAttacker;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Vfx")]
        private ParticleSystem m_chargeGlow;
        [SerializeField, TabGroup("Vfx")]
        private ParticleSystem m_dustTrail;
        [SerializeField, TabGroup("Vfx")]
        private ParticleSystem m_dustImpact;

        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentCD;
        private bool m_isDetecting;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;

        private bool m_attackHit;

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_selfCollider.SetActive(true);
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            StopAllCoroutines();
            m_agent.Stop();
            m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            if (m_targetInfo.isValid)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }

                else
                {
                    StartCoroutine(FlinchRoutine());
                }
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }



        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (!m_targetInfo.isValid)
            //{
            //    m_stateHandle.ApplyQueuedState();
            //}
            m_stateHandle.ApplyQueuedState();
        }
        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.VerticalStab, m_info.verticalStabAttack.range)
                                  , new AttackInfo<Attack>(Attack.ChargeAttack, m_info.ChargeStabAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private Vector3 GetVectorDirection(float angle)
        {
            float relativeScatterAngle = GetRelativeScatterAngle(angle) * Mathf.Deg2Rad;
            var angleVector = new Vector3(Mathf.Sin(relativeScatterAngle), Mathf.Cos(relativeScatterAngle));
            return angleVector;
        }

        private float GetRelativeScatterAngle(float angle)
        {
            var baseTransformRightAngle = transform.rotation.eulerAngles.z - 90;
            var relativeScatterAngle = Mathf.Repeat((angle - baseTransformRightAngle), 360);
            return relativeScatterAngle;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
        private IEnumerator FakeOutRoutine()
        {
            var distanceToGround = 0f;
            var distanceToWall = Vector2.Distance(transform.position, WallPosition());
            var lastPostion = transform.position;
            var targetPosition = m_targetInfo.position;
            m_dustTrail.Play();
            m_animation.SetAnimation(0, m_info.verticalStabFakeOut, true);
            do
            {
                m_agent.MoveTowardsForced(Vector2.down, 50);
                distanceToGround = MathF.Abs(transform.position.y - targetPosition.y);
                distanceToWall = Vector2.Distance(transform.position, WallPosition());

                yield return null;
            } while (distanceToGround > 15f && distanceToWall >= 20f);

            m_agent.Stop();

            yield return null;

            do
            {
                m_agent.MoveTowardsForced(Vector2.up, 50);
                distanceToGround = MathF.Abs(transform.position.y - GroundPosition().y);
                distanceToWall = Vector2.Distance(transform.position, WallPosition());
                yield return null;
            } while (distanceToGround <= 30f && distanceToWall >= 20f);

            m_agent.Stop();
            yield return null;
        }
        private IEnumerator VerticalStabRoutine()
        {
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            m_hitbox.gameObject.SetActive(false);
            var distanceToGround = 0f;
            var distanceToWall = 0f;
            m_animation.SetAnimation(0, m_info.fadeOutAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeOutAnimation);
            transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 30);
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);

            m_animation.SetAnimation(0, m_info.verticalStabStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.verticalStabStart);

            //fakeOut
            yield return FakeOutRoutine();

            //attackProper
            m_stabAttackAttacker.TargetDamaged += OnTargetHit;
            var targetPOsition = m_targetInfo.position;
            m_stabAttackAttacker.enabled = true;
            m_animation.SetAnimation(0, m_info.verticalStabAttack.animation, false);

            do
            {
                m_agent.MoveTowardsForced(new Vector2(targetPOsition.x, GroundPosition().y + 1), 120);
                distanceToGround = MathF.Abs(transform.position.y - GroundPosition().y + 1);
                distanceToWall = Vector2.Distance(transform.position, WallPosition());
                yield return null;
            } while (distanceToGround >= 10f && distanceToWall >= 20f);

            m_agent.Stop();
            m_dustTrail.Stop();
            m_animation.SetAnimation(0, m_info.verticalStabEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.verticalStabEnd);
            if (m_attackHit)
            {
                m_dustImpact.Play();
                m_attackHit = false;
                m_dustImpact.Stop();
            }
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            var random = UnityEngine.Random.Range(0, 2);
            transform.position = new Vector2(m_targetInfo.position.x + (random == 0 ? 5 : -5), m_targetInfo.position.y + 20);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_hitbox.gameObject.SetActive(true);
            m_stabAttackBB.enabled = false;
            m_stabAttackAttacker.TargetDamaged -= OnTargetHit;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_selfCollider.SetActive(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator AimSwingRoutine()
        {
            Vector3 v_diff = (m_targetInfo.transform.position - transform.position);
            float angle = Mathf.Atan2(v_diff.y, v_diff.x);

            if (m_character.facing == HorizontalDirection.Right)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 180);
            }

            var numberOfSwings = m_info.swingNumber;
            var duration = m_info.swingDuration;
            var durationPerSwing = duration / numberOfSwings;

            var maxArcSwing = 90f;
            var arcExtent = maxArcSwing / 2f;

            var minSwingAngle = angle - arcExtent;
            var maxSwingAngle = angle + arcExtent;


            bool willSwingToOpposste = true;
            m_animation.SetAnimation(0, m_info.chargeAttackAnticipation, true);
            for (int i = 0; i < numberOfSwings; i++)
            {
                if (willSwingToOpposste)
                {
                    yield return SwingRoutine(minSwingAngle, maxSwingAngle, durationPerSwing);
                }
                else
                {
                    yield return SwingRoutine(maxSwingAngle, minSwingAngle, durationPerSwing);
                }
                willSwingToOpposste = !willSwingToOpposste;
            }

            yield return AimCompleteRoutine();
        }
        private IEnumerator SwingRoutine(float from, float to, float duration)
        {
            var speed = 1f / duration;
            var lerpValue = 0f;
            m_animation.SetAnimation(0, m_info.chargeAttackAnticipation, true);
            do
            {
                lerpValue += Time.deltaTime * speed;
                var currentValue = Mathf.LerpAngle(from, to, lerpValue);
                var angleOfRotation = GetRelativeScatterAngle(currentValue);
                transform.rotation = Quaternion.Euler(0, 0, currentValue);
                yield return null;
            } while (lerpValue < 1);
            yield return null;
            yield return AimCompleteRoutine();
        }
        private IEnumerator AimCompleteRoutine()
        {

            Vector3 v_diff = (m_targetInfo.transform.position - transform.position);
            float angle = Mathf.Atan2(v_diff.y, v_diff.x);
            if (m_character.facing == HorizontalDirection.Right)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 180);
            }
            m_animation.SetAnimation(0, m_info.chargeAttackAimComplete, false);
            m_chargeGlow.Play();
            yield return null;
        }
        private IEnumerator ChargedStabRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            m_chargedAttackAttacker.TargetDamaged += OnTargetHit;
            m_chargedAttackBB.enabled = true;
            float distanceToGround = 0f;
            m_animation.SetAnimation(0, m_info.fadeOutAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeOutAnimation);
            transform.position = new Vector2(m_targetInfo.position.x + 10, m_targetInfo.position.y + 30);
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);

            //Aim
            yield return AimSwingRoutine();

            m_animation.SetAnimation(0, m_info.attackCharge, true);
            yield return new WaitForSeconds(1f);
            m_chargeGlow.Stop();

            //Attack

            //var currenttarget = m_targetInfo.transform.position;
            var directionFacing = transform.right * (m_character.facing == HorizontalDirection.Right ? 1 : -1);
            var distanceToWall = 0f;
            m_animation.SetAnimation(0, m_info.ChargeStabAttack, true);
            m_dustTrail.Play();
            do
            {
                m_agent.MoveTowardsForced(directionFacing, 120);
                distanceToGround = MathF.Abs(transform.position.y - GroundPosition().y + 1);
                distanceToWall = Vector2.Distance(transform.position, WallPosition());
                yield return null;
            } while (distanceToGround >= 10f && distanceToWall >= 20f);
            m_dustTrail.Stop();
            m_agent.Stop();


            //attackHitOrMiss
            if (m_attackHit)
            {
                m_animation.SetAnimation(0, m_info.chargeAttackEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeAttackEnd);
                m_dustImpact.Play();
                yield return null;
                m_dustImpact.Stop();
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                var random = UnityEngine.Random.Range(0, 2);
                transform.position = new Vector2(m_targetInfo.position.x + (random == 0 ? 5 : -5), m_targetInfo.position.y + 30);
                yield return new WaitForSeconds(2f);
                m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);
                m_attackHit = false;
                m_stateHandle.OverrideState(State.Attacking);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                m_animation.SetAnimation(0, m_info.idleAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
                m_stateHandle.OverrideState(State.Attacking);
            }


            //attackend
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_hitbox.gameObject.SetActive(true);
            m_selfCollider.SetActive(false);
            m_chargedAttackBB.enabled = false;
            m_chargedAttackAttacker.TargetDamaged -= OnTargetHit;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnTargetHit(object sender, CombatConclusionEventArgs eventArgs)
        {
            m_attackHit = true;
        }

        private void CalculateRunPath()
        {
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -3 : 3), m_targetInfo.position.y + 20);
            while (Vector2.Distance(transform.position, WallPosition()) <= 20)
            {
                movePos = new Vector2(movePos.x + 0.1f, movePos.y + 20);
                break;
            }
            m_agent.SetDestination(movePos);
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);

            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_bodyCollider.enabled = true;
            m_agent.Stop();
            var rb2d = GetComponent<Rigidbody2D>();
            m_character.physics.simulateGravity = true;
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            switch (m_attack)
            {
                case Attack.VerticalStab:
                    m_animation.EnableRootMotion(true, false);
                    StartCoroutine(VerticalStabRoutine());
                    break;
                case Attack.ChargeAttack:
                    //m_animation.EnableRootMotion(true, false);
                    StartCoroutine(ChargedStabRoutine());
                    break;
            }
        }

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            var targetPOsition = m_targetInfo.position;
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(targetPOsition.x, targetPOsition.y + 20f);
                bool xTargetInRange = Mathf.Abs(targetPOsition.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(targetPOsition.y - transform.position.y) < attackRange ? true : false;
                if (attack == Attack.VerticalStab)
                {
                    if (yTargetInRange)
                    {
                        inRange = true;
                    }
                }
                else
                {
                    inRange = true;
                }

                DynamicMovement(newPos, moveSpeed);
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
            var rb2d = GetComponent<Rigidbody2D>();
            m_agent.SetDestination(target);
            if (IsFacing(target))
            {
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }


        #endregion

        protected override void Start()
        {
            base.Start();
            m_startPos = transform.position;
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            //m_selfCollider.SetActive(false);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();

            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.VerticalStab, Attack.ChargeAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.verticalStabAttack.range, m_info.ChargeStabAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReturnToPatrol:
                    if (IsFacing(m_startPos))
                    {
                        if (Vector2.Distance(m_startPos, transform.position) > 5f)
                        {
                            var rb2d = GetComponent<Rigidbody2D>();
                            m_bodyCollider.enabled = false;
                            m_agent.Stop();
                            Vector3 dir = (m_startPos - (Vector2)rb2d.transform.position).normalized;
                            rb2d.MovePosition(rb2d.transform.position + dir * m_info.move.speed * Time.fixedDeltaTime);
                            m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Patrol);
                        }
                    }
                    else
                    {
                        m_turnState = State.ReturnToPatrol;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.Patrol;
                    m_animation.DisableRootMotion();
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_currentAttackRange, m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                        }
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_selfCollider.SetActive(true);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();
                    var targetPosition = m_targetInfo.position;
                    var distanceToPlayer = Vector2.Distance(transform.position, targetPosition);
                    if (distanceToPlayer >= 20 /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                    {
                        m_agent.Stop();
                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                        CalculateRunPath();
                        m_agent.Move(m_info.move.speed);
                        m_stateHandle.SetState(State.ReevaluateSituation);
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_targetInfo.isValid)
            {
                if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }
            }
        }
        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_bodyCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            ResetAI();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
