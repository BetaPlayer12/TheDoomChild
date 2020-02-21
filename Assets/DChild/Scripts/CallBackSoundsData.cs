using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [CreateAssetMenu(fileName = "CallBackSoundsData", menuName = "DChild/Audio/CallBack Sound Data")]
    public class CallBackSoundsData : ScriptableObject
    {
        [SerializeField, SoundGroup, TabGroup("Play")]
        private string[] m_soundsToPlay;
        [SerializeField, SoundGroup, TabGroup("Stop")]
        private string[] m_soundsToStop;

        public int playCount => m_soundsToPlay.Length;
        public int stopCount => m_soundsToStop.Length;

        public string GetPlaySoundGroup(int index) => m_soundsToPlay[index];
        public string GetStopSoundGroup(int index) => m_soundsToStop[index];
    }
}