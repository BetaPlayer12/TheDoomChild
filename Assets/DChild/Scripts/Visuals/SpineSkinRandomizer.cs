using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineSkinRandomizer : MonoBehaviour
{
    [SerializeField, SpineSkin]
    private List<string> m_skins;

    private SkeletonAnimation m_skeletonAnimation;

    private void Awake()
    {
        m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        m_skeletonAnimation.skeleton.SetSkin(m_skins[UnityEngine.Random.Range(0, m_skins.Count)]);
    }
}
