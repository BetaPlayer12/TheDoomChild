using Holysoft.Collections;
using UnityEngine;
using Spine.Unity;
using Holysoft.Event;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Environment
{
    public class PoisonMushroom : MonoBehaviour
    {
        [SerializeField]
        private CountdownTimer m_poisonEmissionTime;
        [SerializeField]
        private CountdownTimer m_uninteractableTime;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_emmisionAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_uninteractableAnimation;
        [SerializeField]
        private Collider2D m_trigger;
        [SerializeField]
        private Collider2D m_damageCollider;

        private string m_idleAnimation;
        private SpineAnimation m_animation;

        private void OnUninteractionEnd(object sender, EventActionArgs eventArgs)
        {
            enabled = false;
            m_trigger.enabled = true;
            m_animation.SetAnimation(0, m_idleAnimation, true);

        }
        private void OnPoisonEmissionEnd(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_uninteractableAnimation, false);
            m_uninteractableTime.Reset();
            m_damageCollider.enabled = false;
        }

        [Button]
        private void EmitPoison()
        {
            m_animation.SetAnimation(0, m_emmisionAnimation, true);
            m_poisonEmissionTime.Reset();
            enabled = true;
            m_damageCollider.enabled = true;
            m_trigger.enabled = false;
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SpineAnimation>();
            m_trigger = GetComponentInChildren<Collider2D>();
            m_poisonEmissionTime.CountdownEnd += OnPoisonEmissionEnd;
            m_poisonEmissionTime.EndTime(false);
            m_uninteractableTime.CountdownEnd += OnUninteractionEnd;
            m_uninteractableTime.EndTime(false);
            m_damageCollider.enabled = false;
        }


        private void Start()
        {
            m_idleAnimation = m_animation.animationState.GetCurrent(0).Animation.Name;
            enabled = false;
        }

        private void Update()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            m_poisonEmissionTime.Tick(deltaTime);
            m_uninteractableTime.Tick(deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                if (collision.TryGetComponentInParent(out Hitbox hitbox))
                {
                    EmitPoison();
                }
            }
        }


    }

}
