using Unity.Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CutsceneSystem
{
    public class CutsceneController : Singleton<CutsceneController>
    {
        [SerializeField] UIAnimation[] cinematicBars;
        [SerializeField] float endOffsetRate = 1;
        [SerializeField, TagField] string triggerTagFilter;

        // events
        public event Action OnStarted, OnEnded;

        // props
        public string TriggerTagFilter => triggerTagFilter;
        public bool CinematicMode => cinematicMode;
        public bool IsPlaying => isPlaying;

        // fields
        private WaitForSeconds waitForBarHiding;
        private bool cinematicMode;
        private bool isPlaying;

        private new void Awake()
        {
            base.Awake();
            waitForBarHiding = new WaitForSeconds(cinematicBars.Max(bar => bar.OutroDuration) * endOffsetRate);
        }

        public void Play(Cutscene cutscene)
        {
            StartCoroutine(CutsceneCR());

            IEnumerator CutsceneCR()
            {
                isPlaying = true;
                cinematicMode = cutscene.ShowCinematicBars;
                if (cinematicMode) foreach (var bar in cinematicBars) bar.Show();
                OnStarted?.Invoke();

                // play through actions
                foreach (var action in cutscene.Actions) yield return StartCoroutine(action.Play());

                // revert changes
                for (int i = cutscene.Actions.Count - 1; i >= 0; i--) cutscene.Actions[i].Revert();

                if (cinematicMode)
                {
                    foreach (var bar in cinematicBars) bar.Hide();
                    yield return waitForBarHiding;
                }
                isPlaying = false;
                OnEnded?.Invoke();
                cinematicMode = false;
            }
        }
    }
}
