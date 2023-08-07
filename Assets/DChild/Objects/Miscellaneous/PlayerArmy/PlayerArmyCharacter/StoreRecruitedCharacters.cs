using System.Collections.Generic;
using UnityEngine;

public class StoreRecruitedCharacters : MonoBehaviour
{
    [SerializeField]
    private List<int> m_recruitedCharacterID = new();
    public int recruitedCharactersIDCount => m_recruitedCharacterID.Count;

    public void AddIDToList(int ID)
    {

        m_recruitedCharacterID.Add(ID);

    }

    public int RetrieveRecruitedCharacterIDList(int index)
    {
        return m_recruitedCharacterID[index];
    }


}
