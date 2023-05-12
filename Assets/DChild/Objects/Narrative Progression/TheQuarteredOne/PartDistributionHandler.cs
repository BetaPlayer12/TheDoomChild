using DChild.Gameplay;
using DChild.Gameplay.Items;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDistributionHandler : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_itempickup;
    [SerializeField]
    private GameObject[] m_items;
    [SerializeField]
    private ItemData[] m_data;
    private int m_itemspickedup=0;
    public void GiveItem()
    {
        int temp = 0;
        m_itemspickedup = DialogueLua.GetVariable("PartsPickedUp").AsInt;
        temp = DialogueLua.GetVariable("PartsOnPlayer").AsInt;
        GameplaySystem.playerManager.player.inventory.AddItem(m_data[m_itemspickedup]);
        DialogueLua.SetVariable("PartsPickedUp", m_itemspickedup + 1);
        DialogueLua.SetVariable("PartsOnPlayer", temp + 1);
        DialogueLua.SetVariable("PlayerHasPart", true);
        if (m_itemspickedup + 1 == 4)
        {
            DialogueLua.SetVariable("CompleteRetrieval", true);
            
        }
        

        m_itempickup.enabled = false;
        for (int i = 0; i < m_items.Length; i++)
        {
            m_items[i].SetActive(false);
        }
    }
    public void itemcheck()
    {
        m_itemspickedup = DialogueLua.GetVariable("PartsPickedUp").AsInt;
        for (int i = 0; i < m_items.Length; i++)
        {
            m_items[i].SetActive(false);
        }
        m_items[m_itemspickedup].SetActive(true);
    }
    public void removeitem()
    {
        for (int i = 0; i < m_data.Length; i++)
        {
            GameplaySystem.playerManager.player.inventory.RemoveItem(m_data[i]);
        }
        DialogueLua.SetVariable("PlayerHasPart", false);
    }
}
