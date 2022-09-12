using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Audio
{
    [RequireComponent(typeof(AmbientSound))]
    public class CompositeAmbientSound : MonoBehaviour
    {
        [SerializeField]
        private CompositeAmbientSoundData m_soundData;

        private void OnValidate()
        {
            var selfAmbient = GetComponent<AmbientSound>();
            var childrenAmbients = GetComponentsInChildren<AmbientSound>(true);

            var soundGroup = m_soundData.soundGroup;
            int numberOfRelevantAmbientSounds = 0;

            for (int i = 1; i < childrenAmbients.Length; i++)
            {
                var ambient = childrenAmbients[i];
                if (ambient.AmbientSoundGroup == soundGroup)
                {
                    ambient.enabled = false;
                    numberOfRelevantAmbientSounds++;
                }
            }
            var relevantSound = m_soundData.GetRelevantSoundGroup(numberOfRelevantAmbientSounds);
            selfAmbient.AmbientSoundGroup = relevantSound;
            selfAmbient.exitMode = MasterAudio.AmbientSoundExitMode.FadeSound;
            selfAmbient.reEnterMode = MasterAudio.AmbientSoundReEnterMode.FadeInSameSound;

            if (Application.isPlaying)
            {
                for (int i = childrenAmbients.Length - 1; i >= 0; i--)
                {
                    var ambient = childrenAmbients[i];
                    if (ambient.enabled == false)
                    {
                        Destroy(ambient);
                    }
                }

                Destroy(this);
            }
        }
    }
}
