using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroControlsController : MonoBehaviour
{
    private bool m_isUsingIntroControls = false;

    public void Enable()
    {
        m_isUsingIntroControls = true;
    }

    public void Disable()
    {
        m_isUsingIntroControls = false;
    }

    public bool IsUsingIntroControls() => m_isUsingIntroControls;

}