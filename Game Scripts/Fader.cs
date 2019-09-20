using System.Collections;
using UnityEngine;

public class Fader : Singleton<Fader>
{
    [Header("Fader Setting")]
    public GameObject fadePlane;
    private CanvasGroup fadePlaneCG;

    //Coroutines
    Coroutine screenFadeIn, screenFadeOut, screenFadeInOut;

    private void Awake()
    {
        fadePlane.SetActive(true);
        fadePlaneCG = fadePlane.GetComponent<CanvasGroup>();
        if (fadePlaneCG.alpha > 0)
        {
            ScreenFadeIn(0.5f);
        }
    }
    public void FadeIn(float time, CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeInOutStart(time, canvasGroup.alpha, 0, canvasGroup));
    }
    public void FadeOut(float time, CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeInOutStart(time, canvasGroup.alpha, 1, canvasGroup));
    }
    public void FadeInOut(float time, float start, float end, CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeInOutStart(time, start, end, canvasGroup));
    }
    public void ScreenFadeIn(float time)
    {
        if (screenFadeIn != null)
        {
            StopCoroutine(screenFadeIn);
        }
        fadePlaneCG.interactable = false;
        fadePlaneCG.blocksRaycasts = false;
        StartCoroutine(FadeInOutStart(time, fadePlaneCG.alpha, 0, fadePlaneCG));
    }
    public void ScreenFadeOut(float time)
    {
        if (screenFadeOut != null)
        {
            StopCoroutine(screenFadeOut);
        }
        fadePlaneCG.interactable = true;
        fadePlaneCG.blocksRaycasts = true;
        StartCoroutine(FadeInOutStart(time, fadePlaneCG.alpha, 1, fadePlaneCG));
    }
    public void ScreenFadeInOut(float time, float start, float end)
    {
        if (screenFadeInOut != null)
        {
            StopCoroutine(screenFadeInOut);
        }
        screenFadeInOut = StartCoroutine(FadeInOutStart(time, start, end, fadePlaneCG));
    }
    private IEnumerator FadeInOutStart(float time, float start, float end, CanvasGroup canvasGroup)
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            canvasGroup.alpha = Mathf.Lerp(start, end, percent);
            yield return null;
        }
        canvasGroup.alpha = end;
        yield break;
    }
}
