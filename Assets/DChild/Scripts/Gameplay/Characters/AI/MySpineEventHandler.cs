// Sample written for for Spine 3.7
using UnityEngine;
using Spine;
using Spine.Unity;

// Add this to the same GameObject as your SkeletonAnimation
public class MySpineEventHandler : MonoBehaviour
{

    // The [SpineEvent] attribute makes the inspector for this MonoBehaviour
    // draw the field as a dropdown list of existing event names in your SkeletonData.
    [SerializeField]
    private Transform m_wandTF;
    [SerializeField]
    private bool m_setParent;
    [SerializeField]
    private GameObject m_chargeFX;
    [SerializeField]
    private GameObject m_fireballFX;
    [SerializeField]
    private float m_projectileSpeed;
    [SpineEvent, SerializeField]
    private string m_chargeEventName;
    [SpineEvent, SerializeField]
    private string m_fireballEventName;

    void Start()
    {
        var skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null) return;

        // This is how you subscribe via a declared method.
        // The method needs the correct signature.
        skeletonAnimation.AnimationState.Event += HandleEvent;

        skeletonAnimation.AnimationState.Start += delegate (TrackEntry trackEntry) {
            // You can also use an anonymous delegate.
            Debug.Log(string.Format("track {0} started a new animation.", trackEntry.TrackIndex));
        };

        skeletonAnimation.AnimationState.End += delegate {
            // ... or choose to ignore its parameters.
            Debug.Log("An animation ended!");
        };
    }

    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // Play some sound if the event named "footstep" fired.
        if (e.Data.Name == m_chargeEventName)
        {
            Debug.Log("Play a charge fx!");
            GameObject chargeFX = Instantiate(m_chargeFX, m_wandTF.position, Quaternion.identity);
            if (m_setParent)
            {
                chargeFX.transform.SetParent(m_wandTF);
            }
        }
        else if (e.Data.Name == m_fireballEventName)
        {
            Debug.Log("Play a fireball fx!");
            //GameObject fireballFX = Instantiate(m_fireballFX, m_wandTF.position, Quaternion.identity);
            //fireballFX.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_projectileSpeed, 0f), ForceMode2D.Impulse);
        }
    }
}