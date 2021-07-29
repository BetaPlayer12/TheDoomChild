using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class LightIntesityAdjusterManager : MonoBehaviour
    {
        [SerializeField]
        private BaseLightIntensityAdjuster[] m_lightAdjusters;

        public void SetIntensity(float value)
        {
            for (int i = 0; i < m_lightAdjusters.Length; i++)
            {
                m_lightAdjusters[i].SetIntensity(value);
            }
        }
    }
}
