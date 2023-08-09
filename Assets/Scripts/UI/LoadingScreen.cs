using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;

    public void LoadScene(int sceneIndex)
    {
        // start loadAsynchronously coroutine
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        // loads scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // show loading screen
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            // divide by 0.9 since progress is out of 0.9, and clamp value between 0 and 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            // set loading bar value
            loadingBar.value = progress;
            // yield control back to main thread
            yield return null;
        }
    }
}
