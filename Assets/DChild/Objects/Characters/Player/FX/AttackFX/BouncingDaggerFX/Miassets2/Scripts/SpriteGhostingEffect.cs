using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGhostingEffect : MonoBehaviour
{
    public int ghostCount = 3;
    public float ghostSpawnInterval = 0.05f;
    public float ghostDuration = 0.5f;
    public float ghostAlphaFade = 0.5f;
    public SpriteRenderer spriteRenderer;
    public Vector3 ghostScale = Vector3.one;
    public Color ghostTintColor = Color.white;

    //private List<SpriteRenderer> ghostSprites = new List<SpriteRenderer>();

    //private void Awake()
    //{
    //    StopAllCoroutines();
    //    if (spriteRenderer == null)
    //    {
    //        spriteRenderer = GetComponent<SpriteRenderer>();
    //    }
    //    StartCoroutine(SpawnGhost());
    //}

    private void OnEnable()
    {
        StopAllCoroutines();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        StartCoroutine(SpawnGhost());
    }

    IEnumerator SpawnGhost()
    {
        while (true)
        {
            yield return new WaitForSeconds(ghostSpawnInterval);

            //if (ghostSprites.Count >= ghostCount)
            //{
            //    Destroy(ghostSprites[0].gameObject);
            //    ghostSprites.RemoveAt(0);
            //}

            GameObject ghostGO = new GameObject("Ghost");
            ghostGO.AddComponent<SpriteRenderer>();
            //ghostGO.transform.SetParent(this.transform);

            ghostGO.GetComponent<SpriteRenderer>().sortingLayerID = spriteRenderer.sortingOrder;
            ghostGO.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder-1;
            ghostGO.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            ghostGO.GetComponent<SpriteRenderer>().color = ghostTintColor * new Color(1f, 1f, 1f, ghostAlphaFade);
            ghostGO.GetComponent<SpriteRenderer>().transform.localScale = ghostScale;

            ghostGO.transform.position = transform.position;
            ghostGO.transform.rotation = transform.rotation;

            //ghostSprites.Add(ghostGO.GetComponent<SpriteRenderer>());

            ghostGO.AddComponent<FadeGhost>();
            //ghostGO.GetComponent<FadeGhost>().StartFadeGhost(ghostGO);
            //StartCoroutine(FadeGhost(ghostGO.GetComponent<SpriteRenderer>()));
            yield return null;
        }
    }

    //IEnumerator FadeGhost(SpriteRenderer ghostSR)
    //{
    //    float timeElapsed = 0f;
    //    Color ghostColor = ghostSR.color;

    //    while (timeElapsed < ghostDuration)
    //    {
    //        if (ghostSR != null)
    //        {
    //            timeElapsed += Time.deltaTime;
    //            ghostColor.a = Mathf.Lerp(ghostAlphaFade, 0f, timeElapsed / ghostDuration);
    //            ghostSR.color = ghostTintColor * ghostColor;
    //        }

    //        yield return null;
    //    }
        
    //    StopGhosting();
    //}

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
