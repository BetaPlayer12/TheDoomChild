using UnityEngine;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using DarkTonic.MasterAudio;

[CreateAssetMenu(fileName = "CompositeAmbientSoundData", menuName = "DChild/Gameplay/Composite Ambient Data")]
public class CompositeAmbientSoundData : ScriptableObject
{
    [SerializeField, SoundGroup]
    private string SoundGroup;
    [SerializeField]
    private int m_amountThreshold;
    [SerializeField, SoundGroup]
    private string m_soundGroupToReplace;

}
