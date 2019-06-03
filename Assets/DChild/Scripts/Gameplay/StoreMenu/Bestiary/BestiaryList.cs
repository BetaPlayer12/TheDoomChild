using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace DChild.Menu.Bestiary
{
    [CreateAssetMenu(fileName = "BestiaryList", menuName = "DChild/Database/Bestiary List")]
    public class BestiaryList : DatabaseAssetList<BestiaryData>
    {  
    }
}