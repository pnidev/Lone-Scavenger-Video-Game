using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public string sceneToLoad = "MapSelecting";

    void Start()
    {
        string nextScene = PlayerPrefs.GetString("NextScene", "MapSelecting");
        StartCoroutine(LoadSceneAsync(nextScene));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}