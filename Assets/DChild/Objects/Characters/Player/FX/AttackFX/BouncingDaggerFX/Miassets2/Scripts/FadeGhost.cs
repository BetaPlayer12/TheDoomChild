using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeGhost : MonoBehaviour
{
    public float ghostDuration = 0.5f;
    public float ghostAlphaFade = 0.5f;
    public Color ghostTintColor = Color.white;

    //private List<SpriteRenderer> ghostSprites = new List<SpriteRenderer>();

    //private void Awake()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(FadeGhostRoutine(GetComponent<SpriteRenderer>()));
    //}

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(FadeGhostRoutine(GetComponent<SpriteRenderer>()));
    }

    //public void StartFadeGhost(GameObject ghostGO)
    //{
    //    StartCoroutine(FadeGhostRoutine(ghostGO.GetComponent<SpriteRenderer>()));
    //}

    private IEnumerator FadeGhostRoutine(SpriteRenderer ghostSR)
    {
        float timeElapsed = 0f;
        Color ghostColor = ghostSR.color;

        while (timeElapsed < ghostDuration)
        {
            if (ghostSR != null)
            {
                timeElapsed += Time.deltaTime;
                ghostColor.a = Mathf.Lerp(ghostAlphaFade, 0f, timeElapsed / ghostDuration);
                ghostSR.color = ghostTintColor * ghostColor;
            }

            yield return null;
        }

        //ghostSprites.Remove(ghostSR);
        Destroy(ghostSR.gameObject);
        //StopGhosting();
    }

    //public void StopGhosting()
    //{
    //    StopAllCoroutines();
    //    foreach (SpriteRenderer ghostSR in ghostSprites)
    //    {
    //        if (ghostSR != null)
    //        {
    //            Destroy(ghostSR.gameObject);
    //        }
    //    }
    //    ghostSprites.Clear();
    //}
}
