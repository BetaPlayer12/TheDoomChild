using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAnimManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerManager;


    private void Start()
    {
        playerManager = GetComponentInParent<GameObject>();
    }

    private void Update()
    {
        
    }

}
