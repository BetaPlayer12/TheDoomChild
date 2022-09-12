using Holysoft.Collections;
using UnityEngine;
using Holysoft.Event;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;

namespace DChild.Gameplay.Environment.Obstacles
{   
    public class PoisonMushroom : MonoBehaviour
    {
        [SerializeField,OnValueChanged("OnDataChanged",includeChildren: true)]
        private PoisonMushroomData m_data;

        private CountdownTimer m_emmisionDelayTime;
        private CountdownTimer m_resetDelayTime;


        [SerializeField]
        private Collider2D m_trigger;
        [SerializeField]
        private Collider2D m_damageCollider;

        [SerializeField]
        private Transform m_anticipationTransformReference;
        [SerializeField]
        private float m_delayTime;
        private FX m_anticipationFX;

        private FX m_emissionFX;

        private string m_idleAnimation;
        private SpineAnimation m_animation;
        private SortingHandle m_sortingHandle;

        private FXSpawnHandle<FX> m_fxSpawner;

        private void OnResetDelayEnd(object sender, EventActionArgs eventArgs)
        {
            enabled = false;
            m_trigger.enabled = true;
            m_animation.SetAnimation(0, m_idleAnimation, true);
           
        }

        private void OnDelayEnd(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_data.emissionAnimation, false);
            m_animation.animationState.Complete += OnEmisionComplete;

            m_anticipationFX.Stop();
            m_anticipationFX = null;
            enabled = false;
            m_damageCollider.enabled = false;
        }

        private void OnEmisionComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_data.emissionAnimation)
            {
                m_resetDelayTime.Reset();
                enabled = true;
            }
        }

        private void OnEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_data.emissionFXEvent)
            {
               // OnEmissionFXSpawn(m_fxSpawner.InstantiateFX(m_data.emissionFXReference, Vector2.zero).gameObject, 0);
            }
        }

        private void OnEmissionFXSpawn(GameObject instance, int index)
        {
            var instanceTransform = instance.transform;
            instanceTransform.parent = transform;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localRotation = Quaternion.identity;
            instanceTransform.localScale = Vector3.one;
            instanceTransform.parent = null;
            m_emissionFX = instance.GetComponent<FX>();
            m_emissionFX.Play();
            OnEmissionFXDone();
            instance.GetComponent<SortingHandle>().SetOrder(m_sortingHandle.sortingLayerID, m_sortingHandle.sortingOrder+1);
            m_damageCollider.enabled = true;
        }

        private void OnEmissionFXDone()
        {
            m_damageCollider.enabled = false;
            m_emissionFX = null;
        }

        [Button]
        private void EmitPoison()
        {
            m_animation.SetAnimation(0, m_data.anticipationAnimation, true);
            m_emmisionDelayTime.Reset();
            OnAnticipationFXSpawn(m_fxSpawner.InstantiateFX(m_data.anticipationFXReference, Vector2.zero).gameObject,0);

            enabled = true;
            m_trigger.enabled = false;
            StartCoroutine(DelayCoroutine());

        }
        IEnumerator DelayCoroutine()
        {

            yield return new WaitForSeconds(m_delayTime);
            m_damageCollider.enabled = true;

        }
        private void OnAnticipationFXSpawn(GameObject instance, int index)
        {
            var instanceTransform = instance.transform;
            instanceTransform.parent = m_anticipationTransformReference;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localRotation = Quaternion.identity;
            instanceTransform.localScale = Vector3.one;
            instanceTransform.parent = null;
            m_anticipationFX = instance.GetComponent<FX>();
            m_anticipationFX.Play();
            instance.GetComponent<SortingHandle>().SetOrder(m_sortingHandle.sortingLayerID, m_sortingHandle.sortingOrder+1);
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SpineAnimation>();
            m_sortingHandle = GetComponent<SortingHandle>();
            m_emmisionDelayTime = new CountdownTimer(m_data.emmisionDelayTime);
            m_emmisionDelayTime.CountdownEnd += OnDelayEnd;
            m_emmisionDelayTime.EndTime(false);
            m_resetDelayTime = new CountdownTimer(m_data.resetDelayTime);
            m_resetDelayTime.CountdownEnd += OnResetDelayEnd;
            m_resetDelayTime.EndTime(false);
            m_damageCollider.enabled = false;
        }

        private void Start()
        {
            m_idleAnimation = m_animation.animationState.GetCurrent(0).Animation.Name;
            m_animation.animationState.Event += OnEvent;
            enabled = false;
        }

        private void Update()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            m_emmisionDelayTime.Tick(deltaTime);
            m_resetDelayTime.Tick(deltaTime);
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

#if UNITY_EDITOR
        private void OnDataChanged()
        {
            m_emmisionDelayTime.SetStartTime(m_data.emmisionDelayTime);
            m_resetDelayTime.SetStartTime(m_data.resetDelayTime);
        }
#endif
    }

}
