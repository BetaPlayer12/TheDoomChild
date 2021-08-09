using DarkTonic.MasterAudio;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class DamageReactionHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_fx;
        [SerializeField, SoundGroup]
        private string m_sound;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void SetFX(GameObject fx) => m_fx = fx;

        public void ReactToDamageAt(Vector2 position, HorizontalDirection direction)
        {
            m_spawnHandle.InstantiateFX(m_fx, position, direction);
            MasterAudio.PlaySound3DAtVector3AndForget(m_sound, position);
        }
    }
}