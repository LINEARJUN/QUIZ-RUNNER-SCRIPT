using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneLoader : MonoBehaviour
{
    public string levelName;
    public float delay;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(delay);
        LoadingManager.instance.QuickLevel(levelName);
    }
}
