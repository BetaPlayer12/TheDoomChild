using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerActionInputReferenceData", menuName = "DChild/Database/Player Action Input Reference Data")]
public class PlayerInputReferences : ScriptableObject
{
    [SerializeField]
    private List<InputActionReference> m_referenceList = new List<InputActionReference>();

    public List<InputActionReference> referenceList => m_referenceList;
}
