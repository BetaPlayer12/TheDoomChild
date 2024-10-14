using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmyCompositionData", menuName = "DChild/Gameplay/Army/Player Army Characters")]
public class PlayerArmyCharacters : ScriptableObject
{

    [SerializeField, AssetSelector, PropertyOrder(2), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
    private ArmyCharacterData[] m_playerCharacter;

    public ArmyCharacterData GetCharacters(int index) => m_playerCharacter[index];
    public int charactersCount => m_playerCharacter.Length;

}
