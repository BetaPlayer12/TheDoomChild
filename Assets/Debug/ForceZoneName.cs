﻿using DChild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceZoneName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        GameSystem.ForceCurrentZoneName(gameObject.scene.name);
#endif
    }
}