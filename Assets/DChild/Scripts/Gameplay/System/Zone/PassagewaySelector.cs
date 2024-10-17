using DChild.Gameplay.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassagewaySelector : MonoBehaviour
{
    [SerializeField]
    private LocationSwitcher m_locationSwitcher;
    public void SelectPassageway(LocationSwitcher variablename)
    {
        m_locationSwitcher = variablename;
    }

    public void ForceInteract() 
    {
        m_locationSwitcher.ForceActivation();
    }
}
