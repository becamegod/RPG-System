using System;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static event Action OnScenePreLoad, OnSceneLoaded;

    public static void GoToScene(int sceneIndex)
    {
        OnScenePreLoad?.Invoke();
        ScreenFader.Instance.TransitionToScene(sceneIndex, () => OnSceneLoaded?.Invoke());
    }

    public static void GoToScene(string sceneName) => GoToScene(SceneInfoManager.Instance.GetBuildIndex(sceneName));

    public static void GoToNextScene() => GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
}