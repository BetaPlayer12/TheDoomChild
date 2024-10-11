using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Items;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPetMandaroga : MonoBehaviour
{
    [SerializeField]
    private float m_runDuration;
    [SerializeField]
    private float m_runSpeed;
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private Vector2 m_spawnOffset;
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
    private string m_detectAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_moveAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_attackAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation;

    [SerializeField, TabGroup("Sensors")]
    private RaySensor m_groundSensor;
    [SerializeField, TabGroup("Sensors")]
    private RaySensor m_wallSensor;
    [SerializeField, TabGroup("Sensors")]
    private RaySensor m_edgeSensor;
    [SerializeField, TabGroup("Sensors")]
    private RaySensor m_enemySensor;

    [SerializeField, TabGroup("Sensors")]
    private RaySensorFaceRotator m_groundSensorRotator;
    [SerializeField, TabGroup("Sensors")]
    private RaySensorFaceRotator m_wallSensorRotator;
    [SerializeField, TabGroup("Sensors")]
    private RaySensorFaceRotator m_edgeSensorRotator;
    [SerializeField, TabGroup("Sensors")]
    private RaySensorFaceRotator m_enemySensorRotator;

    private IsolatedPhysics2D m_physics;
    private SimpleAttackProjectile m_projectile;
    private ShadowPetHandler m_eventHandler;

    private Character m_parentCharacter;

    private IEnumerator AttackRoutine()
    {
        m_spine.SetAnimation(0, m_idleAnimation, true);
        while (!m_groundSensor.isDetecting)
        {
            m_groundSensor.Cast();
            yield return null;
        }
        //yield return new WaitUntil(() => m_groundSensor.isDetecting);
        var timer = m_runDuration;
        m_spine.SetAnimation(0, m_moveAnimation, true);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            m_physics.SetVelocity(m_runSpeed * transform.localScale.x, m_physics.velocity.y);
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            m_enemySensor.Cast();
            if (m_wallSensor.isDetecting || !m_edgeSensor.isDetecting || m_enemySensor.isDetecting)
                timer = 0;
            yield return null;
        }
        m_physics.SetVelocity(Vector2.zero);
        m_spine.SetAnimation(0, m_attackAnimation, true);
        //yield return new WaitForAnimationComplete(m_spine.animationState, m_attackAnimation);
        yield return new WaitForSeconds(m_duration);
        m_spine.SetAnimation(0, m_deathAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_deathAnimation);
        m_eventHandler.PetDesummon();
        m_projectile.CallPoolRequest();
        yield return null;
    }

    private void Start()
    {
        if (GetComponentInParent<Character>() != null)
            m_parentCharacter = GetComponentInParent<Character>();
        transform.SetParent(null);
        transform.localScale = new Vector3(m_parentCharacter.facing == HorizontalDirection.Right ? 1 : -1, 1, 1);
        transform.position = new Vector3(m_parentCharacter.facing == HorizontalDirection.Right ? transform.position.x + m_spawnOffset.x : transform.position .x - m_spawnOffset.x, transform.position.y + m_spawnOffset.y, transform.position.z);
        m_physics.SetVelocity(Vector2.zero);

        m_groundSensorRotator.AlignRotationToFacing(m_parentCharacter.facing);
        m_wallSensorRotator.AlignRotationToFacing(m_parentCharacter.facing);
        m_edgeSensorRotator.AlignRotationToFacing(m_parentCharacter.facing);
        m_enemySensorRotator.AlignRotationToFacing(m_parentCharacter.facing);

        StartCoroutine(AttackRoutine());
    }

    private void Awake()
    {
        m_projectile = GetComponent<SimpleAttackProjectile>();
        m_physics = GetComponent<IsolatedPhysics2D>();
        m_eventHandler = GetComponent<ShadowPetHandler>();
    }
}
