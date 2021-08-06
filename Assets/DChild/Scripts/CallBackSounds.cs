using DarkTonic.MasterAudio;
using UnityEngine;

namespace DChild
{

    [AddComponentMenu("DChild/Audio/CallBack Sounds")]
    public class CallBackSounds : MonoBehaviour
    {
        [SerializeField]
        private MasterAudio.SoundSpawnLocationMode m_spawnLocationMode;

        public void Execute(CallBackSoundsData data)
        {
            string currentSoundGroup = null;
            if (data.stopCount > 0)
            {
                for (int i = 0; i < data.stopCount; i++)
                {
                    currentSoundGroup = data.GetStopSoundGroup(i);
                    if (IsTransformPlaying(currentSoundGroup))
                    {
                        StopSound(currentSoundGroup);
                    }
                }
            }

            for (int i = 0; i < data.playCount; i++)
            {
                currentSoundGroup = data.GetPlaySoundGroup(i);
                if (!IsTransformPlaying(currentSoundGroup))
                {
                    PlaySound(currentSoundGroup);
                }
            }
        }

        public void PlaySound(string soundGroup, bool shouldStopAllSoundsBeforePlayingThis = false)
        {
            if (shouldStopAllSoundsBeforePlayingThis)
            {
                StopAllSounds();
            }

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

        public void StopAllSounds()
        {
            MasterAudio.StopAllSoundsOfTransform(transform);
        }

        public void StopSound(string soundGroup)
        {
            MasterAudio.StopSoundGroupOfTransform(transform, soundGroup);
        }

        public void StopBus(string bus)
        {
            MasterAudio.StopBusOfTransform(transform, bus);
        }

        public void StopBus(string[] buses)
        {
            for (int i = 0; i < buses.Length; i++)
            {
                MasterAudio.StopBusOfTransform(transform, buses[i]);
            }
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

        public bool IsTransformPlaying(string soundGroup) => MasterAudio.IsTransformPlayingSoundGroup(soundGroup, transform);
    }
}