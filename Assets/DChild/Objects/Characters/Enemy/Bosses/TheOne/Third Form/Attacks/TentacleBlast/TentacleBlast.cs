using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleBlast : MonoBehaviour
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_initializeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_despawnAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_mouthBlastAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_spawnAnimation;
        [SerializeField]
        private SpineEventListener m_spineListener;

        private GameObject m_tentacleBlastLaser;

        [SerializeField, BoxGroup("Laser")]
        private LaserLauncher m_launcher;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        [SerializeField, ValueDropdown("GetEvents")]
        private string m_StartWarning;
        [SerializeField, ValueDropdown("GetEvents")]
        private string m_StartBlast;
        [SerializeField, ValueDropdown("GetEvents")]
        private string m_BlastDissipate;

        private IEnumerator EmergeTentacle()
        {
            m_launcher.SetAim(false);
            m_animation.SetAnimation(0, m_spawnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_spawnAnimation);
        }

        private IEnumerator DespawnTentacle()
        {
            m_animation.SetAnimation(0, m_despawnAnimation, false);
            yield return new WaitForSeconds(0.5f);
            m_launcher.TurnLazer(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_despawnAnimation);
        }

        private IEnumerator ShootTentacleBeam()
        {
            m_animation.SetAnimation(0, m_mouthBlastAnimation, false);
            yield return new WaitForSeconds(2f);
            m_launcher.PlayAnimation("TentacleBlastAnticipation");
            m_launcher.TurnLazer(true);
            StartCoroutine(m_launcher.LazerBeamRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_mouthBlastAnimation);
            
        }

        public IEnumerator TentacleBlastAttack()
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            yield return EmergeTentacle();
            yield return ShootTentacleBeam();
            yield return new WaitForSeconds(0.5f);
            yield return DespawnTentacle();
            yield return new WaitForSeconds(0.5f);
            //m_launcher.DisableMouthEffects();
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void WarningBeam()
        {
            m_launcher.TurnOffDamageCollider();
            m_launcher.SetBeam(true);
        }

        private void ShootBeam()
        {
            m_launcher.PlayAnimation("TentacleBlast", "TentacleBlastAnticipation");
            m_launcher.TurnOnDamageCollider();
        }

        private void DissipateBeam()
        {
            m_launcher.PlayAnimation("TentacleBlastDissipation", "TentacleBlast");
            m_launcher.TurnOffDamageCollider();
            m_launcher.SetBeam(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_tentacleOriginalPosition = m_tentacleEntity.transform.position;
            //m_tentacleBlastLaser.SetActive(false);
            m_spineListener.Subscribe(m_StartWarning, WarningBeam);
            m_spineListener.Subscribe(m_StartBlast, ShootBeam);
            m_spineListener.Subscribe(m_BlastDissipate, DissipateBeam);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        [Button]
        private void ShootBlast()
        {
            StartCoroutine(TentacleBlastAttack());
        }

        [SerializeField, PreviewField, OnValueChanged("Initialize")]
        private SkeletonDataAsset m_skeletonDataAsset;
        //#if UNITY_EDITOR
        private IEnumerable GetEvents()
        {
            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events.ToArray();
            for (int i = 0; i < reference.Length; i++)
            {
                list.Add(reference[i].Name);
            }
            return list;
        }
    }
}

