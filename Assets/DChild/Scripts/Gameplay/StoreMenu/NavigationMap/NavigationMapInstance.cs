using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapInstance : MonoBehaviour
    {
        [SerializeField]
        private NavMapFogOfWarUI[] m_sceneFogOFWars;

        public NavMapFogOfWarUI GetFogOfWarOfScene(int index) => m_sceneFogOFWars[index];
    }
}