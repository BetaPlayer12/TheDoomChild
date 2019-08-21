using Holysoft;
using Sirenix.OdinInspector;
using UnityEngine;

public class LinkPortal : Portal
{
    [SerializeField]
    private Portal m_linkTo;
    [SerializeField]
    [ReadOnly]
    private Vector3 m_exitPosition;

#if UNITY_EDITOR
    [SerializeField]
    [HideInInspector]
    private bool m_instantiated;
#endif

    public sealed override Vector3 destination
    {
        get
        {
            return m_exitPosition;
        }
    }

    public sealed override void PlayFX() => m_exitFX.Play();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            collision.GetComponentInParent<Actor>().transform.position = m_linkTo.destination;
            m_entranceFX.Play();
            m_linkTo.PlayFX();
        }
    }
}

#if UNITY_EDITOR
namespace DChildEditor
{
    public static partial class Convention
    {
        public const string PORTAL_LINK_LINKTO_VARNAME = "m_linkTo";
        public const string PORTAL_LINK_EXITPOSITION_VARNAME = "m_exitPosition";
        public const string PORTAL_LINK_INSTANTIATED_VARNAME = "m_instantiated";
    }
}
#endif