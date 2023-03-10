using UnityEngine;
using Spine.Unity;

public class SpineEventQuer : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Animator animator;
    public string parameterName;
    public int parameterValue;
    public int parameterValueZero;

    private void Start()
    {
        skeletonAnimation.AnimationState.Event += AnimationState_Event;
    }

    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Dissolve")
        {
            animator.SetInteger(parameterName, parameterValue);
        }
       
        if (e.Data.Name == "Appear")
        {
            animator.SetInteger(parameterName, parameterValueZero);
        }
    }
}