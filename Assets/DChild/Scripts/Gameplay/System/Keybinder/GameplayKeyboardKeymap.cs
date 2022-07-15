using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayKeyboardKeymap : MonoBehaviour
{
    public struct KeyboardKeybinding
    {

    }
    [SerializeField]
    private List<InputActionReference> m_referenceList = new List<InputActionReference>();
    [SerializeField]
    private List<BindingInfo> m_bindingIds = new List<BindingInfo>();

    [SerializeField]
    private PlayerInputReferences m_playerInputReferences;

    [System.Serializable]
    public class BindingInfo
    {
        [SerializeField]
        private string m_compositeName;
        [SerializeField]
        private string m_bindingName;
        [SerializeField]
        private int m_bindingId;
        [SerializeField]
        private string m_button;
        [SerializeField]
        private string m_changeBindinTo;


        public string compositeName { get { return m_compositeName; } set { m_compositeName = value; } }
        public string bindingName { get { return m_bindingName; } set { m_bindingName = value; } }
        public int bindingId { get { return m_bindingId; } set { m_bindingId = value; } }
        public string button { get { return m_button; } set { m_button = value; } }
        public string changeBindingTo { get { return m_changeBindinTo; } set { m_changeBindinTo = value; } }
    }

    [Button]
    void SetKeyBindings()
    {
        var inputList = m_playerInputReferences.referenceList;
        for (int x = 0; x < inputList.Count; x++)
        {
            if (inputList[x].action.actionMap.name == "Gameplay")
            {
                var currentButton = inputList[x].action.bindings;
                for (int y = 0; y < currentButton.Count; y++)
                {
                    if (currentButton[y].isComposite)
                    {

                    }
                    else
                    {
                        if (currentButton[y].path.Contains("<Keyboard>") || currentButton[y].path.Contains("<Mouse>"))
                        {
                            for (int z = 0; z < m_bindingIds.Count; z++)
                            {
                                var currentBindingIdKey = m_bindingIds[z];
                                if (inputList[x].name == currentBindingIdKey.compositeName)
                                {
                                    if (currentBindingIdKey.changeBindingTo != string.Empty)
                                    {
                                        inputList[x].action.Disable();
                                        inputList[x].action.ApplyBindingOverride(currentBindingIdKey.bindingId, $"<Keyboard>/{m_bindingIds[z].changeBindingTo}");
                                        inputList[x].action.Enable();
                                    }
                                }
                            }
                        }
                    }
               

                }
            }
      
        }
        //m_actionAsset.Disable();
        //var actionMap = m_actionAsset.FindActionMap("Gameplay");

        //for (int x = 0; x < m_bindingIds.Count; x++)
        //{
        //    if (m_bindingIds[x].changeBindingTo != null)
        //    {
        //        InputAction moveHorizontal = actionMap.FindAction($"{m_bindingIds[x].compositeName}");
        //        moveHorizontal.ApplyBindingOverride(m_bindingIds[x].bindingId, $"<Keyboard>/{m_bindingIds[x].changeBindingTo}");
        //        Debug.Log($"{moveHorizontal}");

        //    }

        //}

        //if (m_actionAsset.enabled)
        //{

        //}
        //else
        //{
        //    m_actionAsset.Enable();
        //}
    }
    [Button]
    void ResetBindings()
    {
        var inputList = m_playerInputReferences.referenceList;
        for (int x = 0; x < inputList.Count; x++)
        {
            inputList[x].action.RemoveAllBindingOverrides();
        }
    }

    [Button]
    public void GetBindingId()
    {
        var inputList = m_playerInputReferences.referenceList;
        for(int x = 0; x < inputList.Count; x++)
        {
            var currentButton = inputList[x].action.bindings;
            for (int y = 0; y < currentButton.Count; y++)
            {
                if(inputList[x].action.actionMap.name == "Gameplay")
                {
                    if (currentButton[y].isComposite)
                    {
                  
                    }
                    else
                    {
                        if (currentButton[y].isPartOfComposite)
                        {
                            if (currentButton[y].path.Contains("<Gamepad>"))
                            {
                                var temp = new BindingInfo();
                                var index = inputList[x].action.bindings.IndexOf(z => z.id == currentButton[y].id);
                                temp.compositeName = inputList[x].name;
                                temp.bindingName = currentButton[y].name;
                                temp.bindingId = index;
                                temp.button = currentButton[y].path;
                                m_bindingIds.Add(temp);
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            if (currentButton[y].path.Contains("<Gamepad>"))
                            {
                                var temp = new BindingInfo();
                                var index = inputList[x].action.bindings.IndexOf(z => z.id == currentButton[y].id);
                                temp.compositeName = inputList[x].name;
                                temp.bindingName = currentButton[y].name;
                                temp.bindingId = index;
                                temp.button = currentButton[y].path;
                                m_bindingIds.Add(temp);
                            }
                            else
                            {

                            }
                        }
                    }
                   
                }
                else
                {

                }
            }
        }
    }
    //[Button]
    //public void GetBindingId()
    //{
    //    var bindingIdCount = m_actionAsset.actionMaps.Count;

    //    for (int x = 0; x < bindingIdCount; x++)
    //    {
    //        if (m_actionAsset.actionMaps[x].name == "Gameplay")
    //        {

    //            var currentActionMap = m_actionAsset.actionMaps[x].actions.Count;
    //            for (int indexs = 0; indexs < currentActionMap; indexs++)
    //            {
    //                var currentActions = m_actionAsset.actionMaps[x].actions[indexs].bindings.Count;
    //                for (int z = 0; z < currentActions; z++)
    //                {
    //                    var currentButton = m_actionAsset.actionMaps[x].actions[indexs].bindings[z];
    //                    if (currentButton.isComposite)
    //                    {

    //                        var temp = new BindingInfo();
    //                        var index = m_actionAsset.actionMaps[x].actions[indexs].bindings.IndexOf(y => y.id == currentButton.id);
    //                        //var index = m_leftAction.action.bindings.IndexOf(y => y.id == currentButton.id);
    //                        temp.compositeName = m_actionAsset.actionMaps[x].actions[indexs].name;
    //                        temp.bindingName = currentButton.name;
    //                        temp.bindingId = index;
    //                        m_bindingIds.Add(temp);
    //                    }
    //                    else
    //                    {
    //                        if (currentButton.isPartOfComposite)
    //                        {
    //                            if (currentButton.path.Contains("<Keyboard>") || currentButton.path.Contains("<Mouse>"))
    //                            {
    //                                var temp = new BindingInfo();
    //                                var index = m_actionAsset.actionMaps[x].actions[indexs].bindings.IndexOf(y => y.id == currentButton.id);
    //                                //var index = m_leftAction.action.bindings.IndexOf(y => y.id == currentButton.id);
    //                                temp.compositeName = m_actionAsset.actionMaps[x].actions[indexs].name;
    //                                temp.bindingName = currentButton.name;
    //                                temp.bindingId = index;
    //                                m_bindingIds.Add(temp);
    //                            }

    //                        }
    //                        else if (currentButton.isComposite || currentButton.isPartOfComposite)
    //                        {

    //                        }
    //                        else
    //                        {
    //                            if (currentButton.path.Contains("<Keyboard>") || currentButton.path.Contains("<Mouse>"))
    //                            {
    //                                var temp = new BindingInfo();
    //                                var index = m_actionAsset.actionMaps[x].actions[indexs].bindings.IndexOf(y => y.id == currentButton.id);
    //                                //var index = m_leftAction.action.bindings.IndexOf(y => y.id == currentButton.id);
    //                                temp.compositeName = m_actionAsset.actionMaps[x].actions[indexs].name;
    //                                temp.bindingName = currentButton.name;
    //                                temp.bindingId = index;
    //                                m_bindingIds.Add(temp);
    //                            }
    //                        }
    //                    }
    //                }


    //            }

    //        }

    //    }

    //}
    // Start is called before the first frame update
    void Awake()
    {
        //SetKeyBindings();
    }

}

