using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPetEyeBat : MonoBehaviour
{
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private SpineRootAnimation m_spine;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot)
    {
        m_spine = spineRoot;
    }
#endif

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_attackAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation;

    [SerializeField, TabGroup("Lazer")]
    private LineRenderer m_lineRenderer;
    [SerializeField, TabGroup("Lazer")]
    private LineRenderer m_telegraphLineRenderer;
    [SerializeField, TabGroup("Lazer")]
    private EdgeCollider2D m_edgeCollider;
    [SerializeField, TabGroup("Lazer")]
    private GameObject m_muzzleFXGO;
    [SerializeField, TabGroup("Lazer")]
    private ParticleFX m_muzzleLoopFX;
    [SerializeField, TabGroup("Lazer")]
    private ParticleFX m_muzzleTelegraphFX;
    [SerializeField, TabGroup("Lazer")]
    private ParticleFX m_lazerOriginMuzzleFX;
    [SerializeField, TabGroup("Lazer")]
    private Transform m_lazerOrigin;
    [SerializeField, TabGroup("Lazer")]
    private float m_lazerDuration;

    private Vector2 m_lazerTargetPos;
    private bool m_beamOn;
    private bool m_aimOn;

    private List<Vector2> m_Points;
    private IEnumerator m_aimRoutine;

    #region Lazer Coroutine
    private Coroutine m_lazerBeamCoroutine;
    private Coroutine m_lazerLookCoroutine;
    private Coroutine m_aimAtPlayerCoroutine;
    #endregion

    private SimpleAttackProjectile m_projectile;

    private IEnumerator LazerRoutine()
    {
        //if (GetComponentInParent<Character>() != null)
        //    transform.localScale = GetComponentInParent<Character>().transform.localScale;
        transform.localScale = Vector3.one;
        m_spine.SetAnimation(0, m_attackAnimation, false);
        //StartCoroutine(TelegraphLineRoutine());
        //StartCoroutine(m_aimRoutine);
        m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
        m_aimOn = true;
        m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
        //m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
        yield return new WaitForSeconds(1f);
        //StopCoroutine(m_aimRoutine);
        m_aimOn = false;

        m_beamOn = true;
        yield return new WaitForAnimationComplete(m_spine.animationState, m_attackAnimation);

        m_spine.SetAnimation(0, m_idleAnimation, true);

        yield return new WaitForSeconds(m_lazerDuration);
        m_beamOn = false;
        //StopCoroutine(m_lazerLookCoroutine);
        //m_lazerLookCoroutine = null;
        m_spine.animationState.GetCurrent(0).MixDuration = 0;
        //yield return new WaitForSeconds(m_duration);
        yield return new WaitUntil(() => m_lazerBeamCoroutine == null);
        m_spine.SetAnimation(0, m_deathAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_deathAnimation);
        m_projectile.CallPoolRequest();
        yield return null;
    }

    #region Lazer Attack
    //private IEnumerator ElementalOverloadRoutine()
    //{
    //    m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
    //    m_animation.SetAnimation(0, m_info.elementalBeamOverloadAttack.animation, false);
    //    m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
    //    yield return new WaitUntil(() => m_beamOn);
    //    m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
    //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamOverloadAttack.animation);
    //    StopCoroutine(m_lazerLookCoroutine);
    //    m_lazerLookCoroutine = null;
    //    StopCoroutine(m_aimAtPlayerCoroutine);
    //    m_aimAtPlayerCoroutine = null;
    //    m_currentAttackCount++;
    //    m_movement.Stop();
    //    m_animation.SetAnimation(0, RandomIdleAnimation(), true);
    //    m_attackDecider.hasDecidedOnAttack = false;
    //    m_currentAttackCoroutine = null;
    //    m_stateHandle.ApplyQueuedState();
    //    yield return null;
    //}

    private IEnumerator LazerLookRoutine()
    {
        while (true)
        {
            m_lazerTargetPos = LookPosition(m_lazerOrigin);
            yield return null;
        }
        yield return null;
    }

    //public IEnumerator AimAtTargtRoutine()
    //{
    //    m_aimBone.mode = SkeletonUtilityBone.Mode.Override;
    //    while (m_aimOn)
    //    {
    //        m_aimBone.transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 5f);
    //        yield return null;
    //    }
    //    //Vector2 spitPos = m_aimBone.transform.position;
    //    //Vector3 v_diff = (target - spitPos);
    //    //float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
    //    //m_aimBone.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
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
        m_lazerTargetPos = LookPosition(m_lazerOrigin);
        Vector2 startPoint = m_lazerOrigin.position;

        var direction = m_lazerOrigin.right;
        if (GetComponentInParent<Character>() != null)
            direction = GetComponentInParent<Character>().facing == HorizontalDirection.Right ? m_lazerOrigin.right : -m_lazerOrigin.right;

        RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
        //Debug.DrawRay(startPoint, direction);
        return hit.point;
    }

    //private Vector2 ShotPosition()
    //{
    //    Vector2 startPoint = m_lazerOrigin.position;
    //    Vector2 direction = ((Vector2)m_lazerOrigin.right - startPoint).normalized;

    //    RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
    //    Debug.DrawRay(startPoint, direction);
    //    return hit.point;
    //}

    private IEnumerator TelegraphLineRoutine()
    {
        //float timer = 0;
        m_muzzleTelegraphFX.Play();
        m_telegraphLineRenderer.useWorldSpace = true;
        var timerOffset = m_telegraphLineRenderer.startWidth;
        while (m_telegraphLineRenderer.startWidth > 0)
        {
            m_telegraphLineRenderer.SetPosition(1, ShotPosition());
            m_telegraphLineRenderer.startWidth -= Time.deltaTime * timerOffset;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator LazerBeamRoutine()
    {
        if (m_aimOn)
        {
            StartCoroutine(TelegraphLineRoutine());
            StartCoroutine(m_aimRoutine);
        }

        yield return new WaitUntil(() => m_beamOn);
        StopCoroutine(m_aimRoutine);
        m_lazerOriginMuzzleFX.Play();
        m_muzzleLoopFX.Play();

        m_lineRenderer.useWorldSpace = true;
        while (m_beamOn)
        {
            m_muzzleLoopFX.transform.position = ShotPosition();

            m_lineRenderer.SetPosition(0, m_lazerOrigin.position);
            m_lineRenderer.SetPosition(1, ShotPosition());
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
        m_lazerOriginMuzzleFX.Stop();
        m_muzzleLoopFX.Stop();
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

    private void Start()
    {
        m_aimRoutine = AimRoutine();
        StartCoroutine(LazerRoutine());
    }

    private void Awake()
    {
        m_Points = new List<Vector2>();
        m_projectile = GetComponent<SimpleAttackProjectile>();
    }

    protected Vector2 LookPosition(Transform startPoint)
    {
        int hitCount = 0;
        //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
        RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, 1000, true, out hitCount, true);
        Debug.DrawRay(startPoint.position, hit[0].point);
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
        if (debugMode)
        {
            if (hitCount > 0)
            {
                Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
            }
            else
            {
                Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
            }
        }
#endif
        return m_hitResults;
    }
}
