using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BlackDeathCinematicPlayah : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector m_director;
    [SerializeField, TabGroup("Cinematic")]
    private PlayableAsset m_crumbleCinematic;
    [SerializeField, TabGroup("Cinematic")]
    private PlayableAsset m_groundFallCinematic;

    public void PlayCinematic(int sequence)
    {
        StartCoroutine(CinematicRoutine(sequence));
    }

    private IEnumerator CinematicRoutine(int sequence)
    {
        yield return new WaitForSeconds(1.5f);
        m_director.Play(m_crumbleCinematic);
        switch (sequence)
        {
            case 1:
                m_director.Play(m_crumbleCinematic);
                break;
            case 2:
                m_director.Play(m_groundFallCinematic);
                break;
        }
        yield return null;
    }
}
