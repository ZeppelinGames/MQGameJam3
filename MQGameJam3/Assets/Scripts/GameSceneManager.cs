using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeSpeed;

    private bool loadingScene;
    private float targetA;

    private bool fadeComplete = false;

    private void Start()
    {
        fadeCanvas.alpha = 1;
    }

    public void LoadScene(int sceneIndex)
    {
        if(loadingScene)
        {
            return;
        }

        targetA = 1;
        StartCoroutine(LoadAsyncScene(sceneIndex));
    }

    IEnumerator LoadAsyncScene(int sceneIndex)
    {
        loadingScene = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //scene has loaded as much as possible,
            // the last 10% can't be multi-threaded
            if (asyncLoad.progress >= 0.9f && fadeComplete)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // finished load
        loadingScene = false;
    }

    private void Update()
    {
        fadeCanvas.alpha = Mathf.Lerp(fadeCanvas.alpha, targetA, Time.deltaTime * fadeSpeed);
        fadeComplete = Mathf.Abs(fadeCanvas.alpha - targetA) < 0.1f;
    }
}
