using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeController : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTF;
    [SerializeField]
    private Transform m_capeTFReference;

    private void Update()
    {
        if (m_playerTF.localScale.x < 0)
        {
            m_capeTFReference.localScale = Vector3.left;
        }
        else
        {
            m_capeTFReference.localScale = Vector3.one;
        }
    }
}
