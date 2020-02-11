using Sirenix.OdinInspector;
using UnityEngine;
using DarkTonic.MasterAudio;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerSounds : SerializedMonoBehaviour
    {
        [System.Serializable]
        private class Info
        {
            [SoundGroup]
            public string soundGroup;
        }

        public enum SoundType {
            Footstep,
            Jump,
            Land,
        }

        private struct SoundTypeComparer : IEqualityComparer<SoundType>
        {
            public bool Equals(SoundType x, SoundType y) => x == y;

            public int GetHashCode(SoundType obj) => (int)obj;
        }

        [SerializeField]
        private Transform m_soundCaller;
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<SoundType, Info> m_info = new Dictionary<SoundType, Info>(new SoundTypeComparer());

        public void PlaySound(SoundType soundType)
        {
            MasterAudio.PlaySound3DAtTransformAndForget(m_info[soundType].soundGroup, m_soundCaller);
        }

        public void StopSound(SoundType soundType)
        {
            MasterAudio.StopSoundGroupOfTransform(m_soundCaller, m_info[soundType].soundGroup);
        }
    }
}