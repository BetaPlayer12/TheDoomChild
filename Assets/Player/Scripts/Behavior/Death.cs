using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    
    public class Death : MonoBehaviour
    {
       
        private BasicHealth playerStatus;
        // Start is called before the first frame update
        void Start()
        {
            playerStatus = GetComponent<BasicHealth>();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(playerStatus.currentValue);
        }
    }

}
