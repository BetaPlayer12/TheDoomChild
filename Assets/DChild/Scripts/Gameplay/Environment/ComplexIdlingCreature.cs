using UnityEngine;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using Spine;
using System;
using Holysoft.Collections;

namespace DChild.Gameplay.Environment
{
    public class ComplexIdlingCreature : SerializedMonoBehaviour
    {
        #region Behaviours
        public interface IBehaviour
        {
            bool isDone { get; }
            void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false);
            void Update(GameObject rootObject, ref float timer);

#if UNITY_EDITOR
            void Set(SkeletonDataAsset skeletonComponent, int index);
#endif
        }

        [System.Serializable]
        public abstract class BaseAnimationBehaviour : IBehaviour
        {
#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            private SkeletonDataAsset m_skeletonDataAsset;
            [SerializeField, HideInInspector]
            private int m_index;

            public void Set(SkeletonDataAsset skeletonComponent, int index)
            {
                m_skeletonDataAsset = skeletonComponent;
                m_index = index;
            }

            protected IEnumerable GetAnimations()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                if (m_skeletonDataAsset)
                {
                    var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.ToArray();
                    for (int i = 0; i < reference.Length; i++)
                    {
                        list.Add(reference[i].Name);
                    }
                }
                return list;
            }
            protected virtual bool showLoopProp => true;
#endif

            [SerializeField, ValueDropdown("GetAnimations")]
            protected string m_animation;
            [SerializeField, ShowIf("showLoopProp")]
            protected bool m_loop;
            [SerializeField, MinValue(0)]
            protected float m_mixDuration = 0.2f;

            protected bool m_isDone;
            [ShowInInspector, ReadOnly, PropertyOrder(100)]
            protected bool m_isActive;

            public bool isDone => m_isDone;

            public virtual void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                animation.SetAnimation(0, m_animation, m_loop, snap ? 0 : m_mixDuration);
                timer = 0;
                m_isActive = true;
                m_isDone = false;
                instruction.isValid = false;
            }

            public abstract void Update(GameObject rootObject, ref float timer);
        }

        [System.Serializable]
        public class InPlaceBehaviour : BaseAnimationBehaviour
        {
            [SerializeField, MinValue(0.1f)]
            private float m_duration;

            public float duration => m_duration;

            public override void Update(GameObject rootObject, ref float timer)
            {
                timer += GameplaySystem.time.deltaTime;
                if (timer >= m_duration)
                {
                    m_isActive = false;
                    m_isDone = true;
                }
            }
        }

        [System.Serializable]
        public class MovingBehaviour : BaseAnimationBehaviour
        {
            [SerializeField]
            private Vector3 m_destination;
            [SerializeField]
            private bool m_useSlerp;
            [SerializeField, ShowIf("m_useSlerp"), Indent]
            private float m_slerpCenterOffset;
            [SerializeField]
            private bool m_useCustomizeInterpolation;
            [SerializeField, ShowIf("m_useCustomizeInterpolation"), Indent]
            private AnimationCurve m_interpolationCurve;
            [SerializeField]
            private float m_duration;
            private Vector3 m_origin;

#if UNITY_EDITOR
            [ReadOnly]
            public Vector3 m_relativeDestination = Vector3.left;

            public Vector3 destination { get => m_destination; set => m_destination = value; }
            public string animation { get => m_animation; }
            public bool useSlerp { get => m_useSlerp; }
#endif

            public override void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                m_origin = rootObject.transform.position;
                base.Initialize(rootObject, animation, instruction, ref timer, snap);
            }

            public override void Update(GameObject rootObject, ref float timer)
            {
                if (rootObject.transform.position == m_destination || m_duration < timer)
                {
                    m_isActive = false;
                    m_isDone = true;
                }
                else
                {
                    timer += GameplaySystem.time.deltaTime;
                    var lerpValue = GetLerpValue(timer, m_duration);
                    rootObject.transform.position = EvaluateLerp(m_origin, m_destination, lerpValue);
                }
            }

            private float GetLerpValue(float timer, float duration)
            {
                var lerp = Mathf.Clamp01(timer / duration);
                if (m_useCustomizeInterpolation)
                {
                    lerp = m_interpolationCurve.Evaluate(lerp);
                }
                return lerp;
            }

            public Vector3 EvaluateLerp(Vector3 origin, Vector3 destination, float lerpValue)
            {
                if (m_useSlerp)
                {
                    var centerPivot = (origin + destination) * 0.5f;
                    centerPivot -= new Vector3(0, -m_slerpCenterOffset);

                    var originRelativeCenter = origin - centerPivot;
                    var destinationRelativeCenter = destination - centerPivot;

                    return Vector3.Slerp(originRelativeCenter, destinationRelativeCenter, lerpValue) + centerPivot;
                }
                else
                {
                    return Vector3.Lerp(m_origin, m_destination, lerpValue);
                }
            }
        }

        [System.Serializable]
        public class TurnBehaviour : BaseAnimationBehaviour
        {
#if UNITY_EDITOR
            protected override bool showLoopProp => false;
#endif
            private SpineAnimation m_animationReference;
            private Transform m_transform;

            public override void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                base.Initialize(rootObject, animation, instruction, ref timer, snap);
                m_animationReference = animation;
                m_animationReference.animationState.Complete += OnComplete;
                m_transform = rootObject.transform;
            }

            private void OnComplete(TrackEntry trackEntry)
            {
                m_isActive = false;
                m_isDone = true;
                m_animationReference.animationState.Complete -= OnComplete;
                m_animationReference = null;
                var scale = m_transform.localScale;
                scale.x *= -1;
                m_transform.localScale = scale;
                m_transform = null;
            }

            public override void Update(GameObject rootObject, ref float timer)
            {

            }
        }


        [System.Serializable]
        public class FlipBehaviour : BaseAnimationBehaviour
        {
#if UNITY_EDITOR
            protected override bool showLoopProp => false;
#endif

            private Transform m_transform;

            public override void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                base.Initialize(rootObject, animation, instruction, ref timer, snap);
                m_transform = rootObject.transform;
            }

            public override void Update(GameObject rootObject, ref float timer)
            {
                var scale = m_transform.localScale;
                scale.x *= -1;
                m_transform.localScale = scale;
                m_isActive = false;
                m_isDone = true;
            }
        }



        [System.Serializable]
        public class RedirectionBehaviour : IBehaviour
        {
            [SerializeField]
            private bool m_overrideBehaviourIndex;
            [SerializeField, MinValue(0), ShowIf("m_overrideBehaviourIndex")]
            private int m_goToBehaviourIndex;
            [SerializeField]
            private bool m_isReacting;

            public bool isDone => true;

            public void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                instruction.useCurrentIndex = !m_overrideBehaviourIndex;
                instruction.behaviourIndex = m_goToBehaviourIndex;
                instruction.isReacting = m_isReacting;
                instruction.isValid = true;
            }

#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            private int m_index;

            public void Set(SkeletonDataAsset skeletonComponent, int index)
            {
                m_index = index;
            }
#endif
            public void Update(GameObject rootObject, ref float timer)
            {
            }
        }

        public class TeleportBehaviour : IBehaviour
        {
            [SerializeField]
            private Vector3 m_destination;
            [SerializeField]
            private Vector3 m_scale = Vector3.one;

            public bool isDone => true;

            public void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                var transform = rootObject.transform;
                transform.position = m_destination;
                transform.localScale = m_scale;
            }

#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            private int m_index;

            public void Set(SkeletonDataAsset skeletonComponent, int index)
            {
                m_index = index;
            }
#endif
            public void Update(GameObject rootObject, ref float timer)
            {
            }
        }

        [TypeInfoBox("Behaviour Should End here")]
        public class DisappearBehaviour : IBehaviour
        {
            public bool isDone => true;

            public void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                rootObject.SetActive(false);
            }

#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            private int m_index;

            public void Set(SkeletonDataAsset skeletonComponent, int index)
            {
                m_index = index;
            }
#endif
            public void Update(GameObject rootObject, ref float timer)
            {
            }
        }

        #endregion

        [System.Serializable]
        public class BehaviourList : IBehaviour
        {
            [SerializeField, OnValueChanged("UpdateReference"), ListDrawerSettings(ShowIndexLabels = true)]
            private IBehaviour[] m_behaviours;

            private int m_currentIndex;
            private IBehaviour m_currentBehaviour;
            private GameObject m_rootObject;
            private SpineAnimation m_animation;
            private Instruction m_instruction;
            private SkeletonDataAsset m_skeletonComponent;

            public IBehaviour[] behaviours => m_behaviours;
            public bool isDone => m_currentIndex == m_behaviours.Length - 1 && m_behaviours[m_currentIndex].isDone;

            public void Initialize(GameObject rootObject, SpineAnimation animation, Instruction instruction, ref float timer, bool snap = false)
            {
                m_rootObject = rootObject;
                m_animation = animation;
                timer = 0;
                m_instruction = instruction;
                m_currentIndex = 0;
                m_currentBehaviour = m_behaviours[m_currentIndex];
                m_currentBehaviour.Initialize(rootObject, m_animation, m_instruction, ref timer);
            }

#if UNITY_EDITOR
            public void Set(SkeletonDataAsset skeletonComponent, int index)
            {
                m_skeletonComponent = skeletonComponent;
                for (int i = 0; i < m_behaviours.Length; i++)
                {
                    m_behaviours[i].Set(skeletonComponent, i);
                }
            }

            private void UpdateReference()
            {

                for (int i = 0; i < m_behaviours.Length; i++)
                {
                    m_behaviours[i].Set(m_skeletonComponent, i);
                }
            }

#endif

            public void Update(GameObject rootObject, ref float timer)
            {
                if (m_currentBehaviour.isDone)
                {
                    if (m_currentIndex < m_behaviours.Length)
                    {
                        m_currentIndex++;
                        m_currentBehaviour = m_behaviours[m_currentIndex];
                        m_currentBehaviour.Initialize(rootObject, m_animation, m_instruction, ref timer);
                    }
                }
                else
                {
                    m_currentBehaviour.Update(rootObject, ref timer);
                }
            }
        }

        public class Instruction
        {
            public int behaviourIndex;
            public bool useCurrentIndex;
            public bool isReacting;
            public bool isValid;
        }

        [SerializeField, OnValueChanged("UpdateReference")]
        private SpineAnimation m_spineAnimation;
        [SerializeField, OnValueChanged("UpdateReference"), ListDrawerSettings(ShowIndexLabels = true), TabGroup("Idling")]
        private IBehaviour[] m_idlingBehaviour = new IBehaviour[0];
        [SerializeField, OnValueChanged("UpdateReference"), ListDrawerSettings(ShowIndexLabels = true, NumberOfItemsPerPage = 1), TabGroup("Reacting")]
        private IBehaviour[] m_reactBehaviour = new IBehaviour[0];
        private bool m_isReacting;
        private float m_timer;
        private IBehaviour m_currentBehaviour;

        private int m_idlingBehaviourIndex;
        private int m_reactingBehaviourIndex;
        private Instruction m_instruction;


        [Button, HideInEditorMode]
        public void React()
        {
            m_isReacting = true;
            m_reactingBehaviourIndex = UnityEngine.Random.Range(0, m_reactBehaviour.Length);
            m_currentBehaviour = m_reactBehaviour[m_reactingBehaviourIndex];
            m_currentBehaviour?.Initialize(gameObject, m_spineAnimation, m_instruction, ref m_timer);
        }

        private void Awake()
        {
            m_timer = 0f;
            m_idlingBehaviourIndex = 0;
            m_instruction = new Instruction();
            if (m_idlingBehaviour.Length > 0)
            {
                m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
            }
        }

        private void Start()
        {
            m_currentBehaviour?.Initialize(gameObject, m_spineAnimation, m_instruction, ref m_timer, true);
        }

        private void LateUpdate()
        {
            if (m_currentBehaviour.isDone)
            {
                if (m_isReacting)
                {
                    if (m_instruction.isValid && m_instruction.isReacting == false)
                    {
                        m_isReacting = false;
                        if (m_instruction.useCurrentIndex == false)
                        {
                            m_idlingBehaviourIndex = m_instruction.behaviourIndex;
                        }
                        m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
                    }
                }
                else
                {
                    if (m_instruction.isValid)
                    {
                        m_isReacting = m_instruction.isReacting;

                        if (m_isReacting)
                        {
                            if (m_instruction.useCurrentIndex == false)
                            {
                                m_reactingBehaviourIndex = m_instruction.behaviourIndex;
                            }
                            m_currentBehaviour = m_reactBehaviour[m_reactingBehaviourIndex];
                        }
                        else
                        {
                            if (m_instruction.useCurrentIndex == false)
                            {
                                m_idlingBehaviourIndex = m_instruction.behaviourIndex;
                            }
                            m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
                        }
                    }
                    else
                    {
                        m_idlingBehaviourIndex = (int)Mathf.Repeat(m_idlingBehaviourIndex + 1, m_idlingBehaviour.Length);
                        m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
                    }
                }
                m_currentBehaviour.Initialize(gameObject, m_spineAnimation, m_instruction, ref m_timer);
            }
            else
            {
                m_currentBehaviour.Update(gameObject, ref m_timer);
            }
        }

#if UNITY_EDITOR
        [Button]
        private void UpdateReference()
        {
            for (int i = 0; i < m_idlingBehaviour.Length; i++)
            {
                m_idlingBehaviour[i].Set(m_spineAnimation.skeletonAnimation.SkeletonDataAsset, i);
            }
            for (int i = 0; i < m_reactBehaviour.Length; i++)
            {
                m_reactBehaviour[i].Set(m_spineAnimation.skeletonAnimation.SkeletonDataAsset, i);
            }
        }
#endif
    }
}
