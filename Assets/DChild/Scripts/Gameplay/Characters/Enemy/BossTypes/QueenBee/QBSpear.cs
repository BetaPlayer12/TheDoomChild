using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay;

public class QBSpear : MonoBehaviour
{
    [SerializeField, TabGroup("Reference")]
    private SpineRootAnimation m_animation;

    private void Start()
    {
        m_animation.SetAnimation(0, "SpearDrop_SpinLoop", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_animation.SetAnimation(0, "SpearDrop_GroundContact", false).MixDuration = 0;
    }
}
