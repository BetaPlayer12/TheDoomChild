using DChild.Inputs;
using UnityEngine;

namespace DChild
{
    [CreateAssetMenu(fileName ="InputMapData", menuName = "DChild/Input Map Data")]
    public class InputMapData : ScriptableObject
    {
        [SerializeField]
        public InputMap inputMap;
    }
}
