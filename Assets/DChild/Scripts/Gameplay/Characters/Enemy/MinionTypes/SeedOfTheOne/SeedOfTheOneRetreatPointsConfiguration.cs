using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class SeedOfTheOneRetreatPointsConfiguration : MonoBehaviour
    {
        [SerializeField]
        private Vector2[] m_retreatPoints;
        public Vector2[] retreatPoints => m_retreatPoints;

#if UNITY_EDITOR

#endif
    }

}
