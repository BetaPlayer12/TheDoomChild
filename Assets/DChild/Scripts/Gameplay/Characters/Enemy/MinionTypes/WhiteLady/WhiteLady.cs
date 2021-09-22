using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.AI;
using Spine.Unity.Modules;

namespace DChild.Gameplay.Characters.Enemies
{
    public class WhiteLady : Minion, IMovingEnemy, IFlinch
    {
        [SerializeField]
        [AttackDamageList(DamageType.Holy)]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        private WhiteLadyAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private PhysicsMovementHandler2D m_movement;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public struct SpecterSummonInfo
        {
            public SpecterSummonInfo(GameObject specter, Vector2 position) : this()
            {
                this.specter = specter;
                this.position = position;
            }

            public  GameObject specter { get; }
            public Vector2 position { get; }
        }

        public void SummonSpecters(SpecterSummonInfo[] summonInfoList, IEnemyTarget target)
        {
            if(m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SummonRoutine(summonInfoList, target)));
            
        }

        public void MoveTo(Vector2 position)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            //EnableRootMotion(true, true, false);
            m_movement.MoveTo(position, m_moveSpeed);
            m_animation.DoMove();
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, WhiteLadyAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, WhiteLadyAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SummonRoutine(SpecterSummonInfo[] summonInfoList, IEnemyTarget target)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSummon();
            for (int i = 0; i < summonInfoList.Length; i++)
            {
                var summonInfo = summonInfoList[i];
                var specterGO = this.InstantiateToScene(summonInfo.specter);
                var specter = specterGO.GetComponent<Specter>();
                if (i == 1)
                {
                    yield return new WaitForAnimationEvent(m_animation.animationState, WhiteLadyAnimation.EVENT_SUMMON_1ST_SPECTRE);
                    specter.SpawnAt(summonInfo.position, Quaternion.identity);
                    var facing = target.position.x > specter.position.x ? HorizontalDirection.Right : HorizontalDirection.Left;
                    specter.SetFacing(facing);
                    ((IAITargetingBrain)specter.brain).SetTarget(target);
                }
                else if (i == 2)
                {
                    yield return new WaitForAnimationEvent(m_animation.animationState, WhiteLadyAnimation.EVENT_SUMMON_2ND_SPECTRE);
                }
                else if (i == 3)
                {
                    yield return new WaitForAnimationEvent(m_animation.animationState, WhiteLadyAnimation.EVENT_SUMMON_2ND_SPECTRE);
                }
                         
                yield return new WaitForAnimationComplete(m_animation.animationState, WhiteLadyAnimation.ANIMATION_SUMMON_SPECTRES);
                m_waitForBehaviourEnd = false;
                m_behaviour.SetActiveBehaviour(null);
            }
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void EnableRootMotion(bool enable, bool useX, bool useY)
        {
            m_rootMotion.enabled = enable;
            if (enable)
            {
                m_rootMotion.useX = useX;
                m_rootMotion.useY = useY;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_animation = GetComponent<WhiteLadyAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            
        }    
    }
}