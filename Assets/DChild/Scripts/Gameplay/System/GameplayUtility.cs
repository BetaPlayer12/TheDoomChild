using UnityEngine;
using DChild.Gameplay.Characters;

namespace DChild.Gameplay
{
    public class GameplayUtility
    {
        public static RelativeDirection GetRelativeDirection(HorizontalDirection sourceFacing, Vector2 sourcePosition,Vector2 target)
        {
            if(sourceFacing == HorizontalDirection.Left)
            {
                return sourcePosition.x > target.x ? RelativeDirection.Front : RelativeDirection.Back;
            }
            else
            {
                return sourcePosition.x > target.x ? RelativeDirection.Back : RelativeDirection.Front;
            }
        }

        public static bool IsFacingTarget(HorizontalDirection sourceFacing, Vector2 sourcePosition, Vector2 target)
        {
            if (sourceFacing == HorizontalDirection.Left)
            {
                return sourcePosition.x > target.x ? true : false;
            }
            else
            {
                return sourcePosition.x > target.x ? false : true;
            }
        }
    }
}