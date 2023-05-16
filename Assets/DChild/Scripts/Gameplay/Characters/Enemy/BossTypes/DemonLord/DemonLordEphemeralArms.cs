using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonLordEphemeralArms : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animation)
    {
        m_spine = spineRoot;
    }
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_ephemeralArmsSmashAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_ephemeralArmsComboAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_threeFireBallsAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_threeFireBallsFireAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_threeFireBallsPreAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_rayOfFrostAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_rayOfFrostChargeAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_rayOfFrostFireAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_rayOfFrostFireToIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_iceBombAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_iceBombThrowAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_lightningGroundAnimation;

    public void EphemeralArmsSmash(bool loop)
    {
        m_spine.SetAnimation(0, m_ephemeralArmsSmashAnimation, loop);
    }

    public void EphemeralArmsCombo(bool loop)
    {
        m_spine.SetAnimation(0, m_ephemeralArmsComboAnimation, loop);
    }

    public void ThreeFireBalls(bool loop)
    {
        m_spine.SetAnimation(0, m_threeFireBallsAnimation, loop);
    }

    public void ThreeFireBallsFire(bool loop)
    {
        m_spine.SetAnimation(0, m_threeFireBallsFireAnimation, loop);
    }

    public void ThreeFireBallsPre(bool loop)
    {
        m_spine.SetAnimation(0, m_threeFireBallsPreAnimation, loop);
    }

    public void RayOfFrost(bool loop)
    {
        m_spine.SetAnimation(0, m_rayOfFrostAnimation, loop);
    }

    public void RayOfFrostCharge(bool loop)
    {
        m_spine.SetAnimation(0, m_rayOfFrostChargeAnimation, loop);
    }

    public void RayOfFrostFire(bool loop)
    {
        m_spine.SetAnimation(0, m_rayOfFrostFireAnimation, loop);
    }

    public void RayOfFrostFireToIdle(bool loop)
    {
        m_spine.SetAnimation(0, m_rayOfFrostFireToIdleAnimation, loop);
    }

    public void IceBomb(bool loop)
    {
        m_spine.SetAnimation(0, m_iceBombAnimation, loop);
    }

    public void IceBombThrow(bool loop)
    {
        m_spine.SetAnimation(0, m_iceBombThrowAnimation, loop);
    }

    public void LightningGround(bool loop)
    {
        m_spine.SetAnimation(0, m_lightningGroundAnimation, loop);
    }

    public void EmptyAnimation()
    {
        m_spine.SetEmptyAnimation(0, 0);
    }

    //private void Start()
    //{
    //    m_spine.SetAnimation(0, m_closeFIdleAnimation, true);
    //    m_spine.SetAnimation(0, m_closeBIdleAnimation, true);
    //}
}
