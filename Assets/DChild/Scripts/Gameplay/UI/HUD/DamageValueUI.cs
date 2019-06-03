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

        public override void DestroyItem()
        {
            Destroy(gameObject);
        }

        public void Load(int value, IDamageUIConfig damageUI, bool isCrit)
        {
            m_damageText.text = value.ToString();
            m_damageText.colorGradient = damageUI.vertexGradient;
            m_critText.SetActive(isCrit);
        }

        public override void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

        public void SpawnAt(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void DisableObject()
        {
            GameSystem.poolManager.GetOrCreatePool<UIObjectPool>().AddToPool(this);
        }
    }

}