using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class IllusionPlatform : MonoBehaviour
    {
        [Button]
        public void Appear(bool instant)
        {
            gameObject.SetActive(true);
        }

        [Button]
        public void Disappear(bool instant)
        {
            gameObject.SetActive(false);
        }
    }
}
