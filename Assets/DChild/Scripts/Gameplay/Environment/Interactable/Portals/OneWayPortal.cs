using Holysoft;
using Sirenix.OdinInspector;
using UnityEngine;

public class OneWayPortal : Portal
{
    [SerializeField]
    [ReadOnly]
    private Vector3 m_destination;
#if UNITY_EDITOR
    [SerializeField]
    [HideInInspector]
    private bool m_instantiated;


#endif

    public sealed override Vector3 destination
    {
        get
        {
            return m_destination;
        }
    }

    public sealed override void PlayFX()
    {
        m_entranceFX.Play();
        m_exitFX.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            collision.GetComponentInParent<Actor>().transform.position = destination;
            PlayFX();
        }
    }
}

#if UNITY_EDITOR
namespace DChildEditor
{
    public static partial class Convention
    {
        public const string PORTAL_ONEWAY_DESTINATION_VARNAME = "m_destination";
        public const string PORTAL_ONEWAY_INSTANTIATED_VARNAME = "m_instantiated";
    }
}
#endif