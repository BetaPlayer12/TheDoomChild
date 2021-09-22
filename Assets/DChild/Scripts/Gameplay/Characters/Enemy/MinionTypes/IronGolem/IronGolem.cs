using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Spine;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class IronGolem : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_hostileMoveSpeedMult;
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField]
        private Transform m_projectilePos;
        [SpineEvent, SerializeField]
        private List<string> m_chargeEventName;
        [SpineBone]
        public string m_boneName;
        [SerializeField]
        private Bone m_bone;
        [SerializeField]
        private GameObject m_wreckingBall;

        [SerializeField]
        private Damage m_damage;

        private IronGolemAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private Bone m_boneOverride;

        private Vector2 m_target;

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void SetTarget(Vector2 target)
        {
            m_target = target;
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoIdle();
            }
        }

        public void Move(bool isAggro)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.SetAnimation(0, "Move", true).TimeScale = !isAggro ? 1 : m_hostileMoveSpeedMult;
            }
        }

        public void MovetoDestination(Vector2 direction)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoMove();
                m_movement.MoveTo(direction, 0);
            }
        }

        public void StopMoving()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoIdle();
                m_movement.Stop();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            StopActiveBehaviour();
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_movement.Stop();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            //TurnCharacter();
        }
        #endregion

        #region "Attack Behaviors"
        public void LeftChain()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(LeftChainRoutine()));
            }
        }

        public void RightCannon(Vector2 target)
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(RightCannonRoutine(target)));
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Move")
            {
                //Debug.Log("Must Wait");
                return m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
            }
            else
            {
                //Debug.Log("Don't Wait");
                return true;
            }
        }

        public bool IsIdle()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsMoving()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Move")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTurning()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            int range = Random.Range(1, 3);
            if (range == 1)
            {
                m_animation.DoDeath();
            }
            else
            {
                m_animation.DoDeath2();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, range == 1 ? IronGolemAnimation.ANIMATION_DEATH : IronGolemAnimation.ANIMATION_DEATH2);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, IronGolemAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, IronGolemAnimation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        private IEnumerator LeftChainRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_movement.Stop();
            m_wreckingBall.SetActive(true);
            m_animation.DoLeftChain();
            yield return new WaitForAnimationComplete(m_animation.animationState, IronGolemAnimation.ANIMATION_L_CHAIN);
            m_wreckingBall.SetActive(false);
            m_animation.DoIdle();
            yield return null;
            StopActiveBehaviour();
        }

        private IEnumerator RightCannonRoutine(Vector2 target)
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_movement.Stop();
            SetTarget(target);
            m_animation.DoRightCannon();
            yield return new WaitForAnimationComplete(m_animation.animationState, IronGolemAnimation.ANIMATION_R_CANNON);
            SetTarget(transform.position + new Vector3(-15.33001f, 6.239975f, 0));
            m_animation.DoIdle();
            yield return null;
            SetTarget(Vector2.zero);
            StopActiveBehaviour();
        }

        public void SetRootMotionY(bool condition)
        {
            m_rootMotion.useY = condition;
        }

        protected override void OnDeath()
        {
            //base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_chargeEventName[0])
            {
                //Debug.Log("GAGOO NU GINAGAWA MO");
                Vector3 target = m_target;
                Vector3 v_diff = (target - m_projectilePos.position);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject shoot = Instantiate(m_projectile, m_projectilePos.position, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));
            }
        }

        void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            Debug.Log("FKING AIM");
            //Vector3 pos = m_target;
            //var bonePosition = m_bone.GetWorldPosition(this.transform);
            //var direction = pos - bonePosition;
            //float rotation = DirectionToRotation(direction, this.transform);
            ////float parentRotation = m_bone.parent.WorldRotationX;
            //float parentRotation = m_bone.Parent.WorldRotationX;
            //m_bone.RotateWorld(rotation - parentRotation);
            if(m_target != Vector2.zero)
            {
                var localPositon = transform.InverseTransformPoint(m_target);
                m_bone.SetLocalPosition(localPositon);
            }
        }

        static float DirectionToRotation(Vector3 direction, Transform transform)
        {
            var localDirection = transform.InverseTransformDirection(direction);
            return Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;
        }

        protected override void Start()
        {
            base.Start();

            //Spine Bone
            m_bone = m_animation.skeletonAnimation.Skeleton.FindBone(m_boneName);
            m_animation.skeletonAnimation.UpdateLocal += SkeletonAnimation_UpdateLocal;
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<IronGolemAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }

            //Spine Event Listener
            //var skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            if (m_animation.skeletonAnimation == null) return;

            m_animation.skeletonAnimation.AnimationState.Event += HandleEvent;

        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
