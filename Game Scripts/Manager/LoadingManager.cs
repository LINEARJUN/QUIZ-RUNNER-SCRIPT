using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : Singleton<LoadingManager>
{
    //Coroutines
    private Coroutine quickAsync;
    private Coroutine loadLevelAsync;

    public void LoadLevel(string name)
    {
        if (loadLevelAsync != null)
        {
            StopCoroutine(loadLevelAsync);
        }
        loadLevelAsync = StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().name));
    }
    public void QuickLevel(string name, float delay = 1f)
    {
        if (quickAsync != null)
        {
            StopCoroutine(quickAsync);
        }
        quickAsync = StartCoroutine(QuickAsync(name, delay));
    }
    public void Reload()
    {
        if (loadLevelAsync != null)
        {
            StopCoroutine(loadLevelAsync);
        }
        loadLevelAsync = StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().name));
    }

    private IEnumerator QuickAsync(string name, float delay)
    {
        //Fade Out
        Fader.instance.ScreenFadeOut(delay / 2);
        yield return new WaitForSeconds(delay / 2);

        //Start async operation
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //Fade In
        Fader.instance.ScreenFadeIn(delay / 2);
        yield return new WaitForSeconds(delay / 2);

        yield break;
    }
    private IEnumerator LoadLevelAsync(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield break;
    }
}
