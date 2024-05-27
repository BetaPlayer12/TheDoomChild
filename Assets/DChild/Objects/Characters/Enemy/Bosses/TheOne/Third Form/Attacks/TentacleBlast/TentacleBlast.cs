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

        private GameObject m_tentacleBlastLaser;

        [SerializeField, BoxGroup("Laser")]
        private LaserLauncher m_launcher;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        private IEnumerator EmergeTentacle()
        {
            m_animation.SetAnimation(0, m_spawnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_spawnAnimation);

            m_launcher.SetBeam(true);
            m_launcher.SetAim(false);
        }

        private IEnumerator DespawnTentacle()
        {
            m_animation.SetAnimation(0, m_despawnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_despawnAnimation);
        }

        private IEnumerator ShootTentacleBeam()
        {
            m_animation.SetAnimation(0, m_mouthBlastAnimation, false);
            StartCoroutine(m_launcher.LazerBeamRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_mouthBlastAnimation);
            m_launcher.SetBeam(false);
        }

        public IEnumerator TentacleBlastAttack()
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            yield return EmergeTentacle();
            yield return ShootTentacleBeam();
            yield return new WaitForSeconds(2f);
            yield return DespawnTentacle();
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_tentacleOriginalPosition = m_tentacleEntity.transform.position;
            //m_tentacleBlastLaser.SetActive(false);
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
    }
}

