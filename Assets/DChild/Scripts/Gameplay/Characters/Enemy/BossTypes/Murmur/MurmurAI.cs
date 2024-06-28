using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using DChild.Temp;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/Murmur")]
    public class MurmurAI : CombatAIBrain<MurmurAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, HideReferenceObjectPicker]
            private PhaseInfo<Phase> m_phaseInfo = new PhaseInfo<Phase>();
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, MinValue(0), BoxGroup("Basic")]
            private float m_heightDifferenceThreshold;
            public float heightDifferenceThreshold => m_heightDifferenceThreshold;
            [SerializeField, BoxGroup("Basic")]
            private RangeFloat m_attackInterval;
            public RangeFloat attackInterval => m_attackInterval;


            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeatAnimation;
            public BasicAnimationInfo defeatAnimation => m_defeatAnimation;

            [SerializeField]
            private MovementInfo m_moveInfo = new MovementInfo();
            public MovementInfo moveInfo => m_moveInfo;

            [SerializeField, BoxGroup("Sound Wave Attacks")]
            private SimpleProjectileAttackInfo m_frontalScreamInfo = new SimpleProjectileAttackInfo();
            [SerializeField, BoxGroup("Sound Wave Attacks")]
            private SimpleProjectileAttackInfo m_diagonalSoundInfo = new SimpleProjectileAttackInfo();
            [SerializeField, BoxGroup("Sound Wave Attacks")]
            private MovementInfo m_diagonalSoundChaseInfo = new MovementInfo();

            [SerializeField, MinValue(1), BoxGroup("Sound Wave Attacks")]
            private float m_diagonalSoundHeight;
            [SerializeField, MinValue(0), BoxGroup("Sound Wave Attacks")]
            private float m_diagonalSoundInterval;
            public SimpleProjectileAttackInfo frontalScreamInfo => m_frontalScreamInfo;
            public SimpleProjectileAttackInfo diagonalSoundInfo => m_diagonalSoundInfo;
            public MovementInfo diagonalSoundChaseInfo => m_diagonalSoundChaseInfo;
            public float diagonalSoundHeight => m_diagonalSoundHeight;
            public float diagonalSoundInterval => m_diagonalSoundInterval;


            [SerializeField, HideReferenceObjectPicker, BoxGroup("SoundBallThrow")]
            private ProjectileInfo m_soundBall = new ProjectileInfo();
            [SerializeField, MinValue(0.1), BoxGroup("SoundBallThrow")]
            private float m_soundBallSpawnOffset;
            [SerializeField, BoxGroup("SoundBallThrow")]
            private BasicAnimationInfo m_chargeAnimation;
            [SerializeField, MinValue(0.1), BoxGroup("SoundBallThrow")]
            private float m_soundBallChargeDuration;
            [SerializeField, BoxGroup("SoundBallThrow")]
            private BasicAnimationInfo[] m_throwAnimations;


            [SerializeField, BoxGroup("Detonate In Place")]
            private GameObject m_detonateInPlaceSoundBallsCenter;
            [SerializeField, BoxGroup("Detonate In Place")]
            private GameObject m_detonateInPlaceSoundBalls;
            [SerializeField, BoxGroup("Detonate In Place")]
            private BasicAnimationInfo m_summonAnimation;

            [SerializeField, MinValue(0), BoxGroup("Detonate In Place")]
            private float m_summonStartDelay;
            [SerializeField, MinValue(0), BoxGroup("Detonate In Place")]
            private float m_soundBallMaxDistanceSpawn;
            [SerializeField, BoxGroup("Detonate In Place")]
            private RangeFloat m_soundballSpawnInterval;
            [SerializeField, MinValue(1), BoxGroup("Detonate In Place/Center Summon")]
            private int m_centerSpawnCount = 1;
            [SerializeField, MinValue(0f), BoxGroup("Detonate In Place/Center Summon")]
            private float m_centerSpawnDistance = 1;

            public ProjectileInfo soundball => m_soundBall;
            public float soundBallSpawnOffset => m_soundBallSpawnOffset;
            public BasicAnimationInfo chargeAnimation => m_chargeAnimation;
            public float soundBallChargeDuration => m_soundBallChargeDuration;
            public BasicAnimationInfo[] throwAnimations => m_throwAnimations;
            public BasicAnimationInfo summonAnimation => m_summonAnimation;
            public GameObject detonateInPlaceSoundBallsCenter => m_detonateInPlaceSoundBallsCenter;
            public GameObject detonateInPlaceSoundBalls => m_detonateInPlaceSoundBalls;
            public float summonStartDelay => m_summonStartDelay;
            public float soundBallMaxDistanceSpawn => m_soundBallMaxDistanceSpawn;
            public RangeFloat soundballSpawnInterval => m_soundballSpawnInterval;

            public int centerSpawnCount => m_centerSpawnCount;
            public float centerSpawnDistance => m_centerSpawnDistance;
            public override void Initialize()
            {
                m_moveInfo.SetData(m_skeletonDataAsset);
                m_frontalScreamInfo.SetData(m_skeletonDataAsset);
                m_diagonalSoundInfo.SetData(m_skeletonDataAsset);
                m_diagonalSoundChaseInfo.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_defeatAnimation.SetData(m_skeletonDataAsset);
                m_chargeAnimation.SetData(m_skeletonDataAsset);
                for (int i = 0; i < m_throwAnimations.Length; i++)
                {
                    m_throwAnimations[i].SetData(m_skeletonDataAsset);
                }
                m_summonAnimation.SetData(m_skeletonDataAsset);
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [System.Serializable]
            public struct HealthBaseProjectileCountInfo
            {
                [SerializeField, MinValue(1)]
                private int m_amount;
                [SerializeField, PropertyRange(0, 100)]
                private int m_atleastHealthPercentage;

                public HealthBaseProjectileCountInfo(int amount) : this()
                {
                    m_amount = amount;
                    m_atleastHealthPercentage = 100;
                }

                public int amount => m_amount;
                public int atleastHealthPercentage => m_atleastHealthPercentage;
            }

            [SerializeField, ValueDropdown("GetAttackList", IsUniqueList = true)]
            private List<MurmurAI.Attack> m_availableAttacks = new List<Attack>();

            [SerializeField, MinValue(1), ShowIf("@m_availableAttacks.Contains(MurmurAI.Attack.FrontalScream)")]
            private int m_frontalScreamRepititions = 1;
            [SerializeField, MinValue(1), ShowIf("@m_availableAttacks.Contains(MurmurAI.Attack.DiagonalSound)")]
            private int m_diagonalSoundRepititions = 1;
            [SerializeField, Tooltip("Arrange Elements on Descending Order using Health Percentage as basis"), ShowIf("@m_availableAttacks.Contains(MurmurAI.Attack.SoundBallThrow)")]
            private HealthBaseProjectileCountInfo[] m_soundBallThrowInfos = new HealthBaseProjectileCountInfo[] { };
            [SerializeField, Tooltip("Arrange Elements on Descending Order using Health Percentage as basis"), ShowIf("@m_availableAttacks.Contains(MurmurAI.Attack.SoundBallSummon)")]
            private HealthBaseProjectileCountInfo[] m_soundBallSummonInfos = new HealthBaseProjectileCountInfo[] { };

            public List<MurmurAI.Attack> availableAttacks => m_availableAttacks;

            public int frontalScreamRepitions => m_frontalScreamRepititions;
            public int diagonalSoundRepititions => m_diagonalSoundRepititions;

            public int GetAmountOfSoundBallToThrow(Health health) => GetAmountOfSoundBall(health, m_soundBallThrowInfos);
            public int GetAmountOfSoundBallToSummon(Health health) => GetAmountOfSoundBall(health, m_soundBallSummonInfos);

            private int GetAmountOfSoundBall(Health health, HealthBaseProjectileCountInfo[] reference)
            {
                var healthFactor = (float)health.currentValue / health.maxValue;
                var percentageHealth = Mathf.CeilToInt(healthFactor * 100);
                for (int i = 0; i < reference.Length; i++)
                {
                    if (percentageHealth >= reference[i].atleastHealthPercentage)
                    {
                        return reference[i].amount;
                    }
                }
                return 0;
            }

#if UNITY_EDITOR
            private IEnumerable GetAttackList() => Enum.GetValues(typeof(MurmurAI.Attack));
#endif
        }

        public enum Attack
        {
            DiagonalSound,
            FrontalScream,
            SoundBallSummon,
            SoundBallThrow,
        }

        public enum Phase
        {
            Phase1,
            Phase2
        }

        private enum State
        {
            PhaseTransistion,
            WaitForBehaviour,
            Attack,
            ReevaluateSituation,
            AttackCooldown,
        }

        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Reference")]
        private SimpleTurnHandle m_turnHandle;

        [SerializeField]
        private Transform m_mouth;
        [SerializeField]
        private Transform[] m_pipes;
        [SerializeField]
        private Transform m_roomCenter;
        [SerializeField]
        private Transform m_leftSideRoom;
        [SerializeField]
        private Transform m_rightSideRoom;
        [SerializeField]
        private Transform m_soundBallThrowSpawn;
        [SerializeField]
        private float m_baseFlightHeight;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;

        private PhaseInfo m_currentPhaseInfo;

        private float m_targetHeight;
        private bool m_hasAttacked;
        private CountdownTimer m_attackIntervalTimer;
        private List<Projectile> m_soundBallList;

        private bool m_isDetecting;


        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    //GameEventMessage.SendEvent("Boss Encounter");
                }
            }
        }

        private void ApplyPhaseData(PhaseInfo obj)
        {
            UpdateAttackList(obj.availableAttacks);
            m_currentPhaseInfo = obj;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            //TEMP
            //this.gameObject.SetActive(false);
            this.enabled = false;
            GetComponentInChildren<Hitbox>().Disable();
            m_animation.SetAnimation(0, m_info.defeatAnimation, false);
            m_isDetecting = false;
        }

        private void UpdateAttackList(IReadOnlyList<Attack> availableAttacks)
        {
            List<AttackInfo<Attack>> infoList = new List<AttackInfo<Attack>>();
            for (int i = 0; i < availableAttacks.Count; i++)
            {
                var attack = availableAttacks[i];
                switch (attack)
                {
                    case Attack.DiagonalSound:
                        infoList.Add(new AttackInfo<Attack>(attack, 100));
                        break;
                    case Attack.FrontalScream:
                        infoList.Add(new AttackInfo<Attack>(attack, 100));
                        break;
                    case Attack.SoundBallSummon:
                        infoList.Add(new AttackInfo<Attack>(attack, 100));
                        break;
                    case Attack.SoundBallThrow:
                        infoList.Add(new AttackInfo<Attack>(attack, 100));
                        break;
                }
            }

            m_attackDecider.SetList(infoList.ToArray());
        }

        private void OnPhaseChange()
        {
            m_hasAttacked = false;
            m_stateHandle.SetState(State.PhaseTransistion);
        }

        protected override void OnTargetDisappeared()
        {
            //Target Will not disappear
            //this.gameObject.SetActive(true);
        }

        protected HorizontalDirection GetProposedFacing(Vector2 characterPosition, Vector2 destination) => destination.x < characterPosition.x ? HorizontalDirection.Left : HorizontalDirection.Right;

        private void MoveTo(Vector2 destination, float speed)
        {
            m_agent.SetDestination(destination);

            m_agent.Move(speed);
            LookAt(destination);
        }

        private void LookAt(Vector2 destination)
        {
            if (GetProposedFacing(m_centerMass.position, destination) != m_character.facing)
            {
                m_turnHandle.Execute();
            }
        }

        private void OnAttackCooldownEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.SetState(State.Attack);
        }


        #region Atttacks
        private IEnumerator FrontalScreamRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;
            m_stateHandle.Wait(State.ReevaluateSituation);
            var attackInfo = m_info.frontalScreamInfo;
            //Move to Player height
            bool isInRange = false;
            do
            {
                Vector2 currentPosition = m_centerMass.position;
                var targetPosition = m_targetInfo.position;
                var xDistance = Mathf.Abs(currentPosition.x - targetPosition.x);
                if (xDistance <= attackInfo.range + 0.1f && Mathf.Abs(currentPosition.y - targetPosition.y) <= m_info.heightDifferenceThreshold)
                {
                    isInRange = true;
                }
                else
                {
                    //Move in X if the player is not within x-range
                    Vector2 destination = targetPosition;
                    if (xDistance > attackInfo.range)
                    {
                        var fromTargetToEntity = (currentPosition - targetPosition);
                        var xPosToBeInMaxRange = targetPosition.x + (Mathf.Sign(fromTargetToEntity.normalized.x) * attackInfo.range);
                        destination.x = xPosToBeInMaxRange;
                    }
                    else
                    {
                        destination.x = currentPosition.x;
                    }
                    m_animation.SetAnimation(0, m_info.moveInfo.animation, true);
                    MoveTo(destination, m_info.moveInfo.speed);
                }
                yield return null;
            } while (isInRange == false);
            LookAt(m_targetInfo.position);
            yield return null;
            yield return new WaitForSeconds(0.7f);
            m_animation.SetAnimation(0, attackInfo.animation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0f);
            //Spawn the Thingy via event
            yield return new WaitForAnimationComplete(m_animation.animationState, attackInfo.animation);
            m_hasAttacked = true;
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator DiagonalSoundRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;
            m_stateHandle.Wait(State.ReevaluateSituation);
            float height = 0;
            bool InRange = false;

            bool isDone = false;
            int remainingSoundCharges = m_currentPhaseInfo.diagonalSoundRepititions;
            float soundInterval = m_info.diagonalSoundInterval;
            do
            {

                Vector2 destination = m_targetInfo.position;
                destination.y = height;
                if (InRange == false)
                {
                    Vector2 currentPosition = m_centerMass.position;
                    var targetPosition = m_targetInfo.position;
                    var xDistance = Mathf.Abs(currentPosition.x - targetPosition.x);
                    var range = m_info.diagonalSoundInfo.range;
                    if (xDistance > range)
                    {
                        var fromTargetToEntity = (currentPosition - targetPosition);
                        var xPosToBeInMaxRange = targetPosition.x + (Mathf.Sign(fromTargetToEntity.normalized.x) * range);
                        destination.x = xPosToBeInMaxRange;
                    }
                    else
                    {
                        destination.x = currentPosition.x;
                    }
                    height = m_info.diagonalSoundHeight + m_targetInfo.position.y;
                    m_animation.SetAnimation(0, m_info.moveInfo.animation, true);
                    MoveTo(destination, m_info.moveInfo.speed);
                    if (Mathf.Abs(m_centerMass.position.y - height) <= 1)
                    {
                        if (Mathf.Abs(m_centerMass.position.x - m_targetInfo.position.x) <= range + 1)
                        {
                            InRange = true;
                        }
                    }
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.diagonalSoundChaseInfo.animation, true);
                    MoveTo(destination, m_info.diagonalSoundChaseInfo.speed);
                    soundInterval -= GameplaySystem.time.deltaTime;
                    if (soundInterval <= 0)
                    {
                        m_animation.SetAnimation(0, m_info.diagonalSoundInfo.animation, false);
                        m_animation.AddAnimation(0, m_info.diagonalSoundChaseInfo.animation, true, 0);
                        remainingSoundCharges--;
                        soundInterval = m_info.diagonalSoundInterval;
                        if (remainingSoundCharges <= 0)
                        {
                            isDone = true;
                        }
                        var direction = (int)m_character.facing;
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.diagonalSoundInfo.animation);
                        //Give Offset for fx to be over
                        float time = 0.65f;
                        do
                        {
                            time -= GameplaySystem.time.deltaTime;
                            destination = m_character.centerMass.position;
                            destination.y = height;
                            destination.x += direction;
                            MoveTo(destination, m_info.diagonalSoundChaseInfo.speed);
                            yield return null;
                        } while (time > 0);

                    }
                }

                yield return null;
            } while (isDone == false);
            yield return null;
            m_hasAttacked = true;
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator SummonSoundBallCenterRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;
            m_stateHandle.Wait(State.ReevaluateSituation);

            yield return MoveToRoutine(m_roomCenter.position);

            yield return new WaitForSeconds(m_info.summonStartDelay);
            m_animation.SetAnimation(0, m_info.summonAnimation, true);
            int soundCharges = m_info.centerSpawnCount;
            var radius = m_info.centerSpawnDistance;
            var degreeOffset = 360f / soundCharges;
            Vector2 position = m_centerMass.position;

            PoolableObject lastSpawn = null;
            bool lastSpawnDestroyed = false;

            EventAction<PoolItemEventArgs> action = null;
            action = (object sender, PoolItemEventArgs eventArgs) =>
            {
                lastSpawnDestroyed = true;
                lastSpawn.PoolRequest -= action;
            };

            var wait = new WaitForSeconds(0.5f);
            for (int i = 0; i < soundCharges; i++)
            {
                Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degreeOffset * i) * Vector2.right).normalized;
                var spawnPosition = position - (dir * m_info.centerSpawnDistance);
                var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.detonateInPlaceSoundBallsCenter, gameObject.scene);
                instance.SpawnAt(spawnPosition, Quaternion.identity);
                instance.gameObject.GetComponentInChildren<ParticleSystem>().Play(true);
                if (i == soundCharges - 1)
                {
                    lastSpawn = instance;
                    lastSpawn.PoolRequest += action;
                }
                yield return wait;
            }

            while (lastSpawnDestroyed == false)
            {
                yield return null;
            }

            yield return null;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator SummonSoundBallRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;
            m_stateHandle.Wait(State.ReevaluateSituation);

            var destination = m_centerMass.position;
            destination.y = m_baseFlightHeight;

            yield return MoveToRoutine(destination);

            yield return new WaitForSeconds(m_info.summonStartDelay);
            m_animation.SetAnimation(0, m_info.summonAnimation, true);
            int soundCharges = m_currentPhaseInfo.GetAmountOfSoundBallToSummon(m_health);
            float summonInterval = m_info.soundballSpawnInterval.GenerateRandomValue();
            do
            {
                summonInterval -= GameplaySystem.time.deltaTime;
                if (summonInterval <= 0)
                {
                    soundCharges--;
                    var randomDirection = new Vector2(GetRandomNormalizeNumber(), GetRandomNormalizeNumber());
                    var randomDistanceModifier = UnityEngine.Random.Range(0f, m_info.soundBallMaxDistanceSpawn);
                    var spawnPosition = (randomDirection * randomDistanceModifier) + m_targetInfo.position;
                    var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.detonateInPlaceSoundBalls, gameObject.scene);
                    instance.SpawnAt(spawnPosition, Quaternion.identity);
                    instance.gameObject.GetComponentInChildren<ParticleSystem>().Play(true);
                    summonInterval = m_info.soundballSpawnInterval.GenerateRandomValue();
                }
                yield return null;
            } while (soundCharges > 0);
            yield return null;

            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hasAttacked = true;
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;
            float GetRandomNormalizeNumber() => UnityEngine.Random.Range(-1f, 1f);
        }

        private IEnumerator SoundBallThrowRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;
            m_stateHandle.Wait(State.ReevaluateSituation);

            var leftSideDistance = Vector3.Distance(m_centerMass.position, m_leftSideRoom.position);
            var rightSideDistance = Vector3.Distance(m_centerMass.position, m_rightSideRoom.position);
            Vector3 facePosition;
            if (leftSideDistance < rightSideDistance)
            {
                facePosition = m_rightSideRoom.position;
                yield return MoveToRoutine(m_leftSideRoom.position);
            }
            else
            {
                facePosition = m_leftSideRoom.position;
                yield return MoveToRoutine(m_rightSideRoom.position);
            }

            LookAt(facePosition);
            yield return new WaitForSeconds(1);
            m_animation.SetAnimation(0, m_info.chargeAnimation, true);
            m_soundBallList.Clear();
            var soundCharges = m_currentPhaseInfo.GetAmountOfSoundBallToThrow(m_health);
            for (int i = 0; i < soundCharges; i++)
            {
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.soundball.projectile);
                instance.transform.parent = m_soundBallThrowSpawn;
                var offsetIndex = Mathf.CeilToInt((i + 1) / 2f);
                var sign = Mathf.Pow(-1, i);
                instance.transform.localPosition = new Vector3(offsetIndex * m_info.soundBallSpawnOffset * sign, 0, 0);
                instance.Launch(Vector2.zero, 0);
                instance.Impacted += OnEarlyImpact;
                m_soundBallList.Add(instance);
            }

            yield return new WaitForSeconds(m_info.soundBallChargeDuration); // Wait for charge animation to be done

            for (int i = 0; i < soundCharges; i++)
            {
                m_animation.SetAnimation(0, m_info.throwAnimations[i], false);
                if (i < soundCharges - 1)
                {
                    m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                }
                yield return new WaitForSeconds(0.5f); // Must be event based after this;

                var projectile = m_soundBallList[i];
                if (projectile != null)
                {
                    projectile.Impacted -= OnEarlyImpact;
                    m_soundBallList[i].transform.parent = null;
                    var toTarget = (m_targetInfo.position - (Vector2)m_soundBallList[i].transform.position).normalized;
                    projectile.Launch(toTarget, m_info.soundball.speed);
                }
            }


            m_hasAttacked = true;
            m_stateHandle.ApplyQueuedState();
            m_phaseHandle.allowPhaseChange = true;

            void OnEarlyImpact(object sender, EventActionArgs eventArgs)
            {
                var projectile = (Projectile)sender;
                projectile.Impacted -= OnEarlyImpact;
                var index = m_soundBallList.FindIndex(x => x == projectile);
                m_soundBallList[index] = null;
            }
        }

        private IEnumerator MoveToRoutine(Vector2 position)
        {
            bool hasMovedAway = false;
            do
            {
                m_animation.SetAnimation(0, m_info.moveInfo.animation, true);
                MoveTo(position, m_info.moveInfo.speed);

                if (Vector3.Distance((Vector2)m_centerMass.position, position) < 1)
                {
                    //CheckToDo Something
                    hasMovedAway = true;
                }
                yield return null;
            } while (hasMovedAway == false);
        }
        #endregion

        private void OnEvent(TrackEntry trackEntry, Spine.Event e)
        {
            var eventName = e.Data.Name;
            if (eventName == m_info.diagonalSoundInfo.launchOnEvent)
            {
                for (int i = 0; i < m_pipes.Length; i++)
                {
                    var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_info.diagonalSoundInfo.projectileInfo.projectile);
                    Vector3 scale = instance.transform.localScale;
                    instance.transform.parent = m_pipes[i];
                    //scale.x = Mathf.Abs(scale.x) * -1;
                    instance.transform.localScale = scale;
                    instance.transform.localPosition = Vector3.zero;
                    instance.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (eventName == m_info.frontalScreamInfo.launchOnEvent)
            {
                var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_info.frontalScreamInfo.projectileInfo.projectile);
                var scale = instance.transform.localScale;
                instance.transform.parent = m_mouth;
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = scale;
                instance.transform.parent = null;
            }
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_damageable.SetHitboxActive(false);
            enabled = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_stateHandle = new StateHandle<State>(State.Attack, State.WaitForBehaviour);
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.Phase1, m_info.phaseInfo, m_character, OnPhaseChange, ApplyPhaseData);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_attackDecider.SetMaxRepeatAttack(1);

            m_attackIntervalTimer = new CountdownTimer(0);
            m_attackIntervalTimer.CountdownEnd += OnAttackCooldownEnd;
            m_phaseHandle.ApplyChange();

            m_soundBallList = new List<Projectile>();
            m_damageable.Destroyed += OnDeath;
        }



        protected override void Start()
        {
            base.Start();
            m_animation.animationState.Event += OnEvent;
        }


        //private void OnEnable()
        //{
        //    m_phaseHandle.MonitorPhase();
        //    m_phaseHandle.ApplyChange();
        //}

        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Attack:
                    m_attackDecider.DecideOnAttack();
                    if (m_attackDecider.hasDecidedOnAttack)
                    {
                        switch (m_attackDecider.chosenAttack.attack)
                        {
                            case Attack.DiagonalSound:
                                StartCoroutine(DiagonalSoundRoutine());
                                break;
                            case Attack.FrontalScream:
                                StartCoroutine(FrontalScreamRoutine());
                                break;
                            case Attack.SoundBallSummon:
                                StartCoroutine(SummonSoundBallRoutine());
                                break;
                            case Attack.SoundBallThrow:
                                StartCoroutine(SoundBallThrowRoutine());
                                break;
                        }
                        m_attackDecider.hasDecidedOnAttack = false;
                    }
                    break;
                case State.AttackCooldown:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_attackIntervalTimer.Tick(GameplaySystem.time.deltaTime);
                    break;
                case State.PhaseTransistion:
                    StopAllCoroutines();
                    StartCoroutine(SummonSoundBallCenterRoutine());
                    m_phaseHandle.ApplyChange();
                    break;
                case State.ReevaluateSituation:
                    if (m_hasAttacked)
                    {
                        m_attackIntervalTimer.SetStartTime(m_info.attackInterval.GenerateRandomValue());
                        m_attackIntervalTimer.Reset();
                        m_stateHandle.SetState(State.AttackCooldown);
                        m_hasAttacked = false;
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Attack);
                    }
                    break;
                case State.WaitForBehaviour:

                    break;
            }
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }

}