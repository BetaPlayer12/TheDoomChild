using DChild.Gameplay;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Serialization
{
    public class ColorSwitch : MonoBehaviour
    {
        
        public scriptSample SS;
        public  ZoneDataHandle saveFiles;
        public GameObject ZoneData;

        private void Start()
        {
            //GameObject go = GameObject.Find("ZoneHandler");
            saveFiles = ZoneData.GetComponent<ZoneDataHandle>();
        }


        [Button]
        public void ColorOn()
        {
            this.gameObject.SetActive(true);
            SS.storeData.BoolValue[0] = true;
            saveFiles.storeDataFiles(SS);
        }

        [Button]
        public void ColorOff()
        {
            this.gameObject.SetActive(false);
            SS.storeData.BoolValue[0] = false;
            saveFiles.storeDataFiles(SS);
        }

        [Button]
        public void UpdateItem() {
            SS = saveFiles.LoadDataFiles(SS);
        }





        public void Update()
        {
            if (SS.storeData.BoolValue[0] == true)
            {
                this.gameObject.SetActive(true);
            }

            if (SS.storeData.BoolValue[0] == false)
            {
                this.gameObject.SetActive(false);
            }
        }
        
        public scriptSample input()
        {

            return SS;
        }


    }

   
}
