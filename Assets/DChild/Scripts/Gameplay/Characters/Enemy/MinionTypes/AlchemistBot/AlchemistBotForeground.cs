using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBotForeground : MonoBehaviour
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
    private string m_dormantAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_awakenAnimation;

    public void Awaken()
    {
        m_spine.SetAnimation(0, m_awakenAnimation, false).TimeScale = 1;
    }

    private void Start()
    {
        m_spine.SetAnimation(0, m_dormantAnimation, false).TimeScale = 0;
    }
}