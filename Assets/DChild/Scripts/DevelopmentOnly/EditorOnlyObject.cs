using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug 
{
    public class EditorOnlyObject : MonoBehaviour
    {
        private void OnValidate()
        {
            var allChildren = GetComponentsInChildren<Transform>();
            foreach (var child in allChildren)
            {
                if(child.tag != "EditorOnly")
                {
                    child.tag = "EditorOnly";
                }
            }
        }
    }
}