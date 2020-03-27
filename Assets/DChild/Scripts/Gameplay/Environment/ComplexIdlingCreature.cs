using UnityEngine;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using Spine;
using System;

namespace DChild.Gameplay.Environment
{
    public class ComplexIdlingCreature : SerializedMonoBehaviour
    {
        public interface IBehaviour
        {

            bool isDone { get; }
            void Initialize(GameObject rootObject, SpineAnimation animation, ref float timer, ref Instruction instruction, bool snap = false);
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
            private Instruction m_instruction;

            public bool isDone => m_isDone;

            public virtual void Initialize(GameObject rootObject, SpineAnimation animation, ref float timer, ref Instruction instruction, bool snap = false)
            {
                animation.SetAnimation(0, m_animation, m_loop, snap ? 0 : m_mixDuration);
                timer = 0;
                m_isActive = true;
                m_isDone = false;
                instruction = m_instruction;
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
            private float m_duration;
            private Vector3 m_origin;

#if UNITY_EDITOR
            [ReadOnly]
            public Vector3 m_relativeDestination = Vector3.left;

            public Vector3 destination { get => m_destination; set => m_destination = value; }
            public string animation { get => m_animation; }
#endif

            public override void Initialize(GameObject rootObject, SpineAnimation animation, ref float timer, ref Instruction instruction, bool snap = false)
            {
                m_origin = rootObject.transform.position;
                base.Initialize(rootObject, animation, ref timer, ref instruction, snap);
            }

            public override void Update(GameObject rootObject, ref float timer)
            {
                if (rootObject.transform.position == m_destination)
                {
                    m_isActive = false;
                    m_isDone = true;
                }
                else
                {
                    timer += GameplaySystem.time.deltaTime;
                    var lerpValue = Mathf.Clamp01(timer / m_duration);
                    rootObject.transform.position = Vector3.Lerp(m_origin, m_destination, lerpValue);
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

            public override void Initialize(GameObject rootObject, SpineAnimation animation, ref float timer, ref Instruction instruction, bool snap = false)
            {
                base.Initialize(rootObject, animation, ref timer, ref instruction, snap);
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
        public class RedirectionBehaviour : IBehaviour
        {
            [SerializeField, MinValue(0)]
            private int m_goToBehaviourIndex;

            public bool isDone => true;

            public void Initialize(GameObject rootObject, SpineAnimation animation, ref float timer, ref Instruction instruction, bool snap = false)
            {
                instruction = new Instruction
                {
                    behaviourIndex = m_goToBehaviourIndex,
                    isValid = true
                };
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

        public struct Instruction
        {
            public int behaviourIndex;
            public bool isValid;
        }

        [SerializeField, OnValueChanged("UpdateReference")]
        private SpineAnimation m_spineAnimation;
        [SerializeField, OnValueChanged("UpdateReference"), ListDrawerSettings(ShowIndexLabels = true)]
        private IBehaviour[] m_idlingBehaviour;
        private bool m_isReacting;
        private float m_timer;
        private IBehaviour m_currentBehaviour;

        private int m_idlingBehaviourIndex;
        private Instruction m_instruction;

        public void React()
        {

        }

        private void Awake()
        {
            m_timer = 0f;
            m_idlingBehaviourIndex = 0;
            if (m_idlingBehaviour.Length > 0)
            {
                m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
            }
        }

        private void Start()
        {
            m_currentBehaviour?.Initialize(gameObject, m_spineAnimation, ref m_timer, ref m_instruction, true);
        }

        private void LateUpdate()
        {
            if (m_currentBehaviour.isDone)
            {
                if (m_isReacting)
                {
                    //Do Something;
                }
                else
                {
                    if (m_instruction.isValid)
                    {
                        m_idlingBehaviourIndex = m_instruction.behaviourIndex;
                        m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
                    }
                    else
                    {
                        m_idlingBehaviourIndex = (int)Mathf.Repeat(m_idlingBehaviourIndex + 1, m_idlingBehaviour.Length);
                        m_currentBehaviour = m_idlingBehaviour[m_idlingBehaviourIndex];
                    }
                    m_currentBehaviour.Initialize(gameObject, m_spineAnimation, ref m_timer, ref m_instruction);
                }
            }
            else
            {
                m_currentBehaviour.Update(gameObject, ref m_timer);
            }
        }

        private void UpdateReference()
        {
            for (int i = 0; i < m_idlingBehaviour.Length; i++)
            {
                m_idlingBehaviour[i].Set(m_spineAnimation.skeletonAnimation.SkeletonDataAsset, i);
            }
        }
    }
}
