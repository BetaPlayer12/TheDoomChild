using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class DamageShake : MonoBehaviour
    {
        [SerializeField]
        private float m_radiusOffset = 1;
        [SerializeField]
        private float m_duration = 1f;
        [SerializeField]
        private Transform[] m_affectedTransforms;

        private Vector2[] m_startingPositionList;
        private IHitToInteract m_interractable;

        private void OnHit(object sender, HitDirectionEventArgs eventArgs)
        {
            StopAllCoroutines();
            StartCoroutine(Shake());
        }
        IEnumerator Shake()
        {

            enabled = true;
            yield return new WaitForSeconds(m_duration);
            for (int i = 0; i < m_startingPositionList.Length; i++)
            {
                m_affectedTransforms[i].position = m_startingPositionList[i];
            }
            enabled = false;
        }

        private void Start()
        {
            m_interractable = GetComponentInParent<IHitToInteract>();
            m_interractable.OnHit += OnHit;
            m_startingPositionList = new Vector2[m_affectedTransforms.Length];
            for (int i = 0; i < m_startingPositionList.Length; i++)
            {
                m_startingPositionList[i] = m_affectedTransforms[i].position;
            }
            enabled = false;
        }
        private void Update()
        {
            var offset = Random.insideUnitCircle;
            for (int i = 0; i < m_startingPositionList.Length; i++)
            {
                m_affectedTransforms[i].position = m_startingPositionList[i] + offset * m_radiusOffset;
            }
        }
    }

}