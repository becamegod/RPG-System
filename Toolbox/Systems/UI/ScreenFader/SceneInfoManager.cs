using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfoManager : AutoSingleton<SceneInfoManager>
{
    readonly struct SceneInfo
    {
        public readonly string name;
        public readonly int buildIndex;

        public SceneInfo(string name, int buildIndex)
        {
            this.name = name;
            this.buildIndex = buildIndex;
        }
    }

    public event Action OnSceneInitialized;

    private List<SceneInfo> sceneInfos;

    private void Awake()
    {
        InitSceneInfos();
        SceneChanger.OnSceneLoaded += WaitForSceneSetup;
    }

    private void OnDestroy()
    {
        SceneChanger.OnSceneLoaded -= WaitForSceneSetup;        
    }

    private void WaitForSceneSetup()
    {
        StartCoroutine(WaitCR());
        IEnumerator WaitCR()
        {
            yield return new WaitForEndOfFrame();
            OnSceneInitialized?.Invoke();
        }
    }

    private void InitSceneInfos()
    {
        sceneInfos = new List<SceneInfo>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = Path.GetFileNameWithoutExtension(path);
            sceneInfos.Add(new(sceneName, i));
        }
    }

    public int GetBuildIndex(string sceneName)
    {
        var sceneInfo = sceneInfos.Find(sceneInfo => sceneInfo.name == sceneName);
        if (sceneInfo.name == default) return -1;
        return sceneInfo.buildIndex;
    }
}
