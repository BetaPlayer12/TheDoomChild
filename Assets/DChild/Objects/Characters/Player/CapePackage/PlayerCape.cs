using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCape : MonoBehaviour
{
    [SerializeField]
    private SkeletonAnimation m_skeleton;
    [SerializeField, SpineSlot]
    private string[] m_cape;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_cape.Length; i++)
        {
            m_skeleton.skeleton.SetAttachment(m_cape[i], null);

        }

    }
}
