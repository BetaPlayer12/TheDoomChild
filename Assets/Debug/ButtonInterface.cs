using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInterface : MonoBehaviour
{
    [System.Serializable]
    private class Keystorage 
    {
        [SerializeField]
        public KeyCode m_buttons;
        [SerializeField]
        public UIView m_screenItem;
    }

    [SerializeField]
    private Keystorage[] m_storage;
    


    void Update()
    {
        for (int i = 0; i < m_storage.Length; i++)
        {
            var item = m_storage[i];
            if (Input.GetKey(item.m_buttons))
            {
                item.m_screenItem.Show();
            }
            else
            {
                item.m_screenItem.Hide();
            }
        }
    }

}
