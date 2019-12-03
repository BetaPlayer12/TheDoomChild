using DarkTonic.MasterAudio;
using UnityEngine;

namespace DChild
{
    [AddComponentMenu("DChild/Audio/CallBack Sounds")]
    public class CallBackSounds : MonoBehaviour
    {
        [SerializeField]
        private MasterAudio.SoundSpawnLocationMode m_spawnLocationMode;

        public void PlaySound(string soundGroup)
        {
            switch (m_spawnLocationMode)
            {
                case MasterAudio.SoundSpawnLocationMode.AttachToCaller:
                    MasterAudio.PlaySound3DFollowTransformAndForget(soundGroup, transform);
                    break;
                case MasterAudio.SoundSpawnLocationMode.CallerLocation:
                    MasterAudio.PlaySound3DAtTransformAndForget(soundGroup, transform);
                    break;
                case MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
                    MasterAudio.PlaySoundAndForget(soundGroup);
                    break;
            }
        }

        public void StopSound(string soundGroup)
        {
            MasterAudio.StopSoundGroupOfTransform(transform, soundGroup);
        }


        public void StopSounds(params string[] soundGroups)
        {
            for (int i = 0; i < soundGroups.Length; i++)
            {
                MasterAudio.StopSoundGroupOfTransform(transform, soundGroups[i]);
            }
        }

        public void InterruptSoundWith(string toPlay, params string[] toInterrupt)
        {
            StopSounds(toInterrupt);
            PlaySound(toPlay);
        }
    } 
}