using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class DollyCart : MonoBehaviour, ITrackingCamera
    {
        [SerializeField]
        private CinemachineDollyCart m_cart;
        [SerializeField]
        private CinemachineSmoothPath m_path;
        [SerializeField]
        private bool m_doNotFollowPlayer;
        [SerializeField, ShowIf("m_doNotFollowPlayer")]
        private Transform m_toFollow;

        public CinemachineBasicMultiChannelPerlin noiseModule => null;

        public void Track(Transform transform)
        {
            m_toFollow = transform;
        }

        private void Start()
        {
            if (m_doNotFollowPlayer == false)
            {
                GameplaySystem.cinema.AllowTracking(this);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            var toFollow = m_toFollow.position;
            var point = m_path.FindClosestPoint(m_toFollow.position, 0, -1, 10);
            var pos = m_path.EvaluatePosition(point);
            pos.x = toFollow.x;
            m_cart.m_Position = m_path.FindClosestPoint(pos, 0, -1, 10);

        }
    }

}