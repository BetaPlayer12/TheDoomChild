using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class ParticleTimeHandler
    {
        private float m_timeScale;
        private List<IFXObjects> m_fXObjectsList;

#if UNITY_EDITOR
        public int registeredObjectCount => m_fXObjectsList.Count;
#endif

        public ParticleTimeHandler(float timeScale)
        {
            this.m_timeScale = timeScale;
            m_fXObjectsList = new List<IFXObjects>();
        }

        public static void AlignTime(ParticleSystem[] particleSystems, float timeScale)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                var main = particleSystems[i].main;
                main.simulationSpeed = timeScale;
            }
        }

        public void Register(IFXObjects fx)
        {
            fx.AlignTime(m_timeScale);
            m_fXObjectsList.Add(fx);
        }

        public void Unregister(IFXObjects fx)
        {
            fx.AlignTime(1f);
            m_fXObjectsList.Remove(fx);
        }


        public void AlignTime(float timeScale)
        {
            m_timeScale = timeScale;
            for (int i = 0; i < m_fXObjectsList.Count; i++)
            {
                m_fXObjectsList[i].AlignTime(m_timeScale);
            }
        }

        public void ClearNull() => m_fXObjectsList = m_fXObjectsList.Where(item => item != null).ToList();
    }

}