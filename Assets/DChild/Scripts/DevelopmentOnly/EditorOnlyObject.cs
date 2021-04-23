using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug 
{
    public class EditorOnlyObject : MonoBehaviour
    {

        private void OnValidate()
        {
            gameObject.tag = "EditorOnly";
        }
    }

}