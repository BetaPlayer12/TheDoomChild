using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug 
{
    public class EditorOnlyObject : MonoBehaviour
    {
        [SerializeField]
        private Transform someTransform;
        private void OnValidate()
        {
           
            foreach (Transform tr in someTransform)
            {
                if (tr.tag != "EditorOnly")
                {
                    tr.tag = "EditorOnly";
                }
            }
            //Transform.childCount;
            //transform.GetChild()
            //var allChildren = GetComponentsInChildren<Transform>();
            //foreach (var child in allChildren)
            //{
               // if(child.tag != "EditorOnly")
               // {
                   // child.tag = "EditorOnly";
                //}
           //}
        }
    }
}