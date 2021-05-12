using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Environment
{
    public class LocationNameUIHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private Dictionary<Sprite, Image> m_pair;
        private Image m_currentEnabled;

        public void Show(Sprite sprite)
        {
            m_currentEnabled.enabled = false;
            m_currentEnabled = m_pair[sprite];
        }

        private void Start()
        {
            foreach (var image in m_pair.Values)
            {
                image.enabled = false;
            }
        }
    }
}