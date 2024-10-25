﻿using DChild.Gameplay.Pooling;
using DChild.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class DamageValueUI : UIObject, IDamageUI
    {
        [SerializeField]
        private TextMeshPro m_damageText;
        [SerializeField]
        private GameObject m_critText;

        public void Load(int value, TMP_ColorGradient configuration, bool isCrit)
        {
            m_damageText.text = value.ToString();
            m_damageText.colorGradientPreset = configuration;
            m_critText.SetActive(isCrit);
        }

        public void SpawnAt(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void DisableObject()
        {
            CallPoolRequest();
        }

        private void Awake()
        {
            //Scale Jitters on Awake, this is to make sure that the jitter stops
            m_damageText.transform.localScale = Vector3.zero;
        }
    }

}