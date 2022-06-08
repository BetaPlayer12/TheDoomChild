using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTest : MonoBehaviour
{

    [TableList(DrawScrollView = false)]
    public List<SomeCustomClass> AlwaysExpandedTable = new List<SomeCustomClass>()
{
    new SomeCustomClass(),
    new SomeCustomClass(),
};


    [Serializable]
    public class SomeCustomClass
    {

        //[TableColumnWidth(57, Resizable = false)]
        //[PreviewField(Alignment = ObjectFieldAlignment.Center)]
        //public Texture Icon = ExampleHelper.GetTexture();

        //[TextArea]
        //public string Description = ExampleHelper.GetString();

        [VerticalGroup("Combined Column"), LabelWidth(22)]
        public string A, B, C;

        [TableColumnWidth(60)]
        [Button, VerticalGroup("Actions")]
        public void Test1() { }

        [TableColumnWidth(60)]
        [Button, VerticalGroup("Actions")]
        public void Test2() { }

        [TableColumnWidth(60)]
        [Button, VerticalGroup("Actions")]
        public void Test() { }
    }
}
