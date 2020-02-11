using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:: Find a way to change this to acleaner one

public class PlatFormStick : MonoBehaviour
{
    [SerializeField]
    private Transform m_platForm;

    public List<Transform> childs = new List<Transform>();



    private void OnTriggerEnter2D(Collider2D zee)
    {
        Debug.Log("check");
        if (zee.name.Contains("Zee"))
        {
            zee.transform.root.SetParent(m_platForm, true);
        }
        else
        {
            zee.transform.root.SetParent(m_platForm, true);
        }

    }

    private void OnTriggerExit2D(Collider2D zee)
    {

        if (zee.name.Contains("Zee"))
        {
            zee.transform.parent.parent.SetParent(null);
        }
        else
        {
            zee.transform.root.SetParent(null);
        }

    }


}
