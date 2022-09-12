using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMiniPlayerInitializer : MonoBehaviour, IComplexCharacterModule
{
    [SerializeField]
    private InteractableDetector m_detector;

    private Character m_character;
    private Vector2 m_prevCharacterPosition;

    public void Initialize(ComplexCharacterInfo info)
    {
        m_character = info.character;
        m_prevCharacterPosition = m_character.centerMass.transform.position;
    }
    public void Start()
    {
        m_detector.remoteInitialization(m_character, m_prevCharacterPosition);
    }

}
