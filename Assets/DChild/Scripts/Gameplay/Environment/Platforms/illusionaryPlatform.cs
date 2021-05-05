using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class illusionaryPlatform : MonoBehaviour
    {
        [Button]
        public void reappear()
        {
            gameObject.SetActive(true);
        }
        [Button]
        public void disappear()
        {
            gameObject.SetActive(false);
        }
    }
}
