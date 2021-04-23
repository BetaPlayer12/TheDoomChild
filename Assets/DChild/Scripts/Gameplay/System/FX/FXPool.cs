/******************************
 * 
 * Manages FX that are instantiated rather than played;
 * 
 ******************************/
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class FXPool : ObjectPool<FX>
    {
    }

}