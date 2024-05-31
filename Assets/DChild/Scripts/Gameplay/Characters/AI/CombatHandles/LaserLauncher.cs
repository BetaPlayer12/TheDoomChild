using DChild;
using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class LaserLauncher : MonoBehaviour
    {
        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_lineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_telegraphLineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleLoopFX;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleTelegraphFX;
        [SerializeField, TabGroup("Lazer")]
        private Animator m_lazerAnimator;
        [SerializeField, TabGroup("Lazer")]
        private Transform m_endSFXtransform;
        [SerializeField, TabGroup("Lazer")]
        private Transform m_MouthSFXtransform;
        [SerializeField, TabGroup("Lazer")]
        private Vector2 m_mouthBlastOffset;


        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_beamFrontPoint;
        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_beamBackPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_beamPoint;


        private List<Vector2> m_Points;
        private IEnumerator m_aimRoutine;

        private Vector2 m_lastTargetPos;
        private Vector2 m_lazerTargetPos;

        private bool m_beamOn;
        private bool m_aimOn;


        #region Lazer Coroutine
        private Coroutine m_lazerBeamCoroutine;
        private Coroutine m_lazerLookCoroutine;
        private Coroutine m_aimAtPlayerCoroutine;
        #endregion

        public void SetBeam(bool beamOn)
        {
            m_beamOn = beamOn;
        }

        public void SetAim(bool aimOn)
        {
            m_aimOn = aimOn;
        }

        #region Lazer Attack


        //private IEnumerator LazerLookRoutine()
        //{
        //    while (true)
        //    {
        //        m_lazerTargetPos = LookPosition(m_character.facing == HorizontalDirection.Right ? m_beamFrontPoint : m_beamBackPoint/*m_beamPoint*/);
        //        yield return null;
        //    }
        //    yield return null;
        //}

        //public IEnumerator AimAtTargtRoutine()
        //{
        //    m_aimBone.mode = SkeletonUtilityBone.Mode.Override;
        //    while (m_aimOn)
        //    {
        //        m_aimBone.transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 5f);
        //        yield return null;
        //    }
        //    m_aimBone.mode = SkeletonUtilityBone.Mode.Follow;
        //    m_aimAtPlayerCoroutine = null;
        //    yield return null;
        //}

        private IEnumerator AimRoutine()
        {
            while (true)
            {
                m_telegraphLineRenderer.SetPosition(0, m_telegraphLineRenderer.transform.position);
                m_lineRenderer.SetPosition(0, m_lineRenderer.transform.position);
                m_lineRenderer.SetPosition(1, m_lineRenderer.transform.position);
                yield return null;
            }
        }

        private Vector2 ShotPosition()
        {
            m_lazerTargetPos = LookPosition(m_beamPoint);
            Vector2 startPoint = m_beamPoint.position;
            Vector2 direction = (m_lazerTargetPos - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            //Debug.DrawRay(startPoint, direction);
            return hit.point;
        }

        private IEnumerator TelegraphLineRoutine()
        {
            //float timer = 0;
            //m_muzzleTelegraphFX.Play();
            m_telegraphLineRenderer.useWorldSpace = true;
            m_telegraphLineRenderer.SetPosition(1, ShotPosition());
            var timerOffset = m_telegraphLineRenderer.startWidth;
            
            while (m_telegraphLineRenderer.startWidth > 0)
            {
                //m_muzzleTelegraphFX.transform.position = m_beamPoint.position;
                m_telegraphLineRenderer.startWidth -= Time.deltaTime * timerOffset;
                yield return null;
            }
            yield return null;
        }

        public IEnumerator LazerBeamRoutine()
        {
            if (!m_aimOn)
            {
                StartCoroutine(TelegraphLineRoutine());
                StartCoroutine(m_aimRoutine);
            }
            StopCoroutine(m_aimRoutine);
            //m_muzzleLoopFX.Play();
            yield return new WaitUntil(() => m_beamOn);
            m_edgeCollider.transform.position = m_beamPoint.position;
            m_lineRenderer.useWorldSpace = true;
            m_MouthSFXtransform.gameObject.SetActive(true);
            while (m_beamOn)
            {
                //m_muzzleLoopFX.transform.position = ShotPosition();
                
                m_lineRenderer.SetPosition(0, m_beamPoint.position);
                m_lineRenderer.SetPosition(1, ShotPosition());
                m_MouthSFXtransform.transform.position = m_lineRenderer.GetPosition(0) + (Vector3)m_mouthBlastOffset; //new Vector2 (m_lineRenderer.GetPosition(0).x+ m_mouthBlastOffset.x, m_lineRenderer.GetPosition(0).y+ m_mouthBlastOffset.y);
                m_MouthSFXtransform.rotation = m_beamPoint.rotation;
                m_endSFXtransform.transform.position = m_lineRenderer.GetPosition(1);
                for (int i = 0; i < m_lineRenderer.positionCount; i++)
                {
                    var pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
                    pos = new Vector2(Mathf.Abs(pos.x), pos.y);
                    //if (i > 0)
                    //{
                    //    pos = pos * 0.7f;
                    //}
                    m_Points.Add(pos);
                }
                m_edgeCollider.points = m_Points.ToArray();
                m_Points.Clear();
                yield return null;
            }
            //m_muzzleLoopFX.Stop();
            //yield return new WaitUntil(() => !m_beamOn);
            ResetLaser();
            m_lazerBeamCoroutine = null;
            yield return null;
        }

        private void ResetLaser()
        {
            m_telegraphLineRenderer.useWorldSpace = false;
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(0, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.startWidth = 1;
            m_Points.Clear();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }
            m_edgeCollider.points = m_Points.ToArray();
        }
        #endregion

        public void TurnOffDamageCollider()
        {
            m_edgeCollider.gameObject.SetActive(false);
        }

        public void TurnOnDamageCollider()
        {
            m_edgeCollider.gameObject.SetActive(true);
        }

        public void PlayAnimation(string AnimToTrigger)
        {
            m_lazerAnimator.SetTrigger(AnimToTrigger);
        }

        public void PlayAnimation(string AnimToTrigger,string AnimToReset)
        {
            m_lazerAnimator.ResetTrigger(AnimToReset);
            m_lazerAnimator.SetTrigger(AnimToTrigger);
        }

        public void TurnLazer(bool TurnLazerTo)
        {
            m_lineRenderer.gameObject.SetActive(TurnLazerTo);
        }


        public void DisableMouthEffects()
        {
            m_MouthSFXtransform.gameObject.SetActive(false);
        }

        protected Vector2 LookPosition(Transform startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, 1000, true, out hitCount, true);
            //Debug.DrawRay(startPoint.position, hit[0].point); //TEMP
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            //if (debugMode)
            //{
            //    if (hitCount > 0)
            //    {
            //        Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
            //    }
            //    else
            //    {
            //        Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
            //    }
            //}
#endif
            return m_hitResults;
        }

        private void Start()
        {
            m_aimRoutine = AimRoutine();
        }

        private void Awake()
        {
            m_Points = new List<Vector2>();
        }
    }
}
