using DChild.Gameplay;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class SceneTransferDebugger : MonoBehaviour
    {
        [SerializeField]
        private LocationSwitch m_switcher;
        [SerializeField]
        private Character m_character;

        [Button]
        public void Switch()
        {
            m_switcher.GoToDestination(m_character);
        }
    }
}