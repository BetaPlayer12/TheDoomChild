using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TheOneSecondFormElementalBeamAttack : MonoBehaviour
    {
        [SerializeField]
        private Character m_character;

        [SerializeField]
        private SkeletonUtilityBone m_aimBone;
        [SerializeField]
        private Vector3 m_aimOffset;

        [SerializeField]
        private LineRenderer m_indicator;

        [SerializeField]
        private LineRenderer m_beam;
        [SerializeField]
        private EdgeCollider2D m_beamCollider;
        [SerializeField]
        private DPSColliderDamage m_colliderDamage;
        [SerializeField]
        private ParticleSystem m_impactFX;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_beamFrontPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_beamBackPoint;
        [SerializeField]
        private float m_maxBeamLength = 1000;

        private Vector2 m_beamEndPoint;
        private Transform m_target;
        private bool m_isAimingAtTarget;
        private float m_indicatorStartWidth;
        private List<Vector2> m_beamPointsCache;

        private bool m_isBeamOn;

        public void EnableAimAtTarget(Transform target)
        {
            m_target = target;
            m_aimBone.mode = SkeletonUtilityBone.Mode.Override;
            m_isAimingAtTarget = true;
        }

        public void HoldAim()
        {
            m_isAimingAtTarget = false;
        }

        public void DisableAimAtTarget()
        {
            m_aimBone.mode = SkeletonUtilityBone.Mode.Follow;
        }

        protected Vector2 GetEndPointOfBeam(Transform startPoint)
        {
            int hitCount = 0;
            RaycastHit2D[] hit = DChildUtility.RayCastEnvironment(startPoint.position, startPoint.right, m_maxBeamLength, true, out hitCount, true);
            //Debug.DrawRay(startPoint.position, hit[0].point);
            return hit[0].point;
        }

        private void UpdateBeamConfigration()
        {
            m_beamEndPoint = GetEndPointOfBeam(m_character.facing == HorizontalDirection.Right ? m_beamFrontPoint : m_beamBackPoint);
        }

        public void ShowIndicator(float duration)
        {
            ResetIndicator();
            StartCoroutine(IndicatorRoutine(duration));
        }

        public void ForceStopIndicator()
        {
            StopAllCoroutines();
            ResetIndicator();
        }

        public void ExecuteBeamAttack()
        {
            m_isBeamOn = true;
            m_beam.useWorldSpace = true;
            m_impactFX.Play();
        }

        public void StopBeam()
        {
            m_isBeamOn = false;
            m_impactFX.Stop();
            ResetBeam();
        }



        public void ResetIndicator()
        {
            m_indicator.useWorldSpace = false;
            m_indicator.SetPosition(0, Vector3.zero);
            m_indicator.SetPosition(1, Vector3.zero);
            m_indicator.startWidth = m_indicatorStartWidth;
        }

        private void ResetBeam()
        {
            m_beam.useWorldSpace = false;
            m_beam.SetPosition(0, Vector3.zero);
            m_beam.SetPosition(1, Vector3.zero);
            m_beamPointsCache.Clear();
            for (int i = 0; i < m_beam.positionCount; i++)
            {
                m_beamPointsCache.Add(Vector2.zero);
            }
            m_beamCollider.points = m_beamPointsCache.ToArray();
            m_colliderDamage.ForceClearAffectedColliders();
        }

        private void UpdateBeam()
        {
            var beamEndPoint = GetBeamEndPoint();
            m_beam.SetPosition(0, m_beamFrontPoint.position);
            m_beam.SetPosition(1, beamEndPoint);
            m_impactFX.transform.position = beamEndPoint;
            for (int i = 0; i < m_beam.positionCount; i++)
            {
                var pos = m_beam.GetPosition(i) - m_beamCollider.transform.position;
                pos = new Vector2(Mathf.Abs(pos.x), pos.y);
                //if (i > 0)
                //{
                //    pos = pos * 0.7f;
                //}
                m_beamPointsCache.Add(pos);
            }

            if (m_isAimingAtTarget)
            {
                m_beam.SetPosition(0, m_beam.transform.position);
                m_beam.SetPosition(1, m_beam.transform.position);
            }

            m_beamCollider.points = m_beamPointsCache.ToArray();
            m_beamPointsCache.Clear();
        }

        private Vector2 GetBeamEndPoint()
        {
            UpdateBeamConfigration();
            Vector2 startPoint = m_beamFrontPoint.position;
            Vector2 direction = (m_beamEndPoint - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            //Debug.DrawRay(startPoint, direction);
            return hit.point;
        }

        private IEnumerator IndicatorRoutine(float duration)
        {
            yield return null;

            m_indicator.useWorldSpace = true;
            m_indicator.SetPosition(0, m_indicator.transform.position);
            m_indicator.SetPosition(1, GetBeamEndPoint());
            var narrowingRate = m_indicator.startWidth / duration;
            var timer = 0f;

            do
            {
                m_indicator.startWidth -= Time.deltaTime * narrowingRate;
                timer += Time.deltaTime;
                if (m_isAimingAtTarget)
                {
                    m_indicator.SetPosition(0, m_indicator.transform.position);
                    m_indicator.SetPosition(1, GetBeamEndPoint());
                }
                yield return null;
            } while (timer < duration);
        }

        private void Awake()
        {
            m_indicatorStartWidth = m_indicator.startWidth;

            m_beamPointsCache = new List<Vector2>();
        }

        private void Update()
        {
            if (m_isAimingAtTarget)
            {
                m_aimBone.transform.position = m_target.position + m_aimOffset; //new Vector2(m_target.position.x, m_target.position.y - 5f);
            }

            if (m_isBeamOn)
            {
                UpdateBeam();
            }
        }


    }
}