using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FadeAnimation))]
public class ScreenFader : Singleton<ScreenFader>
{
    // props
    public float IntroDuration => fader.IntroDuration;
    public float OutroDuration => fader.OutroDuration;

    // fields
    private FadeAnimation fader;
    private Action action, postAction;

    private void Start() => fader = GetComponent<FadeAnimation>();

    public void FadeOut() => fader.Show();

    public void FadeIn() => fader.Hide();

    public void Transition(Action midTransition = null, Action postTransition = null)
    {
        action += midTransition;
        postAction += postTransition;
        fader.onShown += OnMidTransition;
        fader.onHidden += OnPostTransition;
        fader.Show();
    }

    public void TransitionToScene(int sceneIndex, Action onSceneLoaded = null)
    {
        fader.onShown += LoadSceneAsync;
        fader.Show();
        void LoadSceneAsync()
        {
            SceneManager.LoadSceneAsync(sceneIndex).completed += _ =>
            {
                onSceneLoaded?.Invoke();
                fader.Hide();
                fader.onShown -= LoadSceneAsync;
            };
        }
    }

    private void OnPostTransition()
    {
        postAction?.Invoke();
        postAction = null;
        fader.onHidden -= OnPostTransition;
    }

    private void OnMidTransition()
    {
        action?.Invoke();
        action = null;
        fader.Hide();
        fader.onShown -= OnMidTransition;
    }
}
