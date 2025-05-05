using System.Collections.Generic;
using UnityEngine;

namespace CutsceneSystem
{
    public class HideInCutscene : MonoBehaviour
    {
        [SerializeField] bool respectPrevState = true;
        [SerializeField] bool immediate;
        [SerializeField] UIAnimation[] toBeHiddenList;

        // fields
        private Dictionary<UIAnimation, bool> prevVisibilityMap;

        protected void Start()
        {
            prevVisibilityMap = new();
            ListenEvent();
        }

        protected void OnDestroy() => IgnoreEvent();

        private CutsceneController Controller => CutsceneController.Instance;
        protected void ListenEvent()
        {
            Controller.OnStarted += CheckHide;
            Controller.OnEnded += CheckShow;
        }

        protected void IgnoreEvent()
        {
            Controller.OnStarted -= CheckHide;
            Controller.OnEnded -= CheckShow;
        }

        private void CheckHide()
        {
            if (!Controller.CinematicMode) return;
            Hide();
        }

        private void CheckShow()
        {
            if (!Controller.CinematicMode) return;
            Show();
        }

        protected void Show()
        {
            foreach (var ui in toBeHiddenList)
            {
                var lastVisible = !respectPrevState || prevVisibilityMap[ui];
                if (lastVisible) ui.Show();
            }
        }

        protected void Hide()
        {
            foreach (var ui in toBeHiddenList)
            {
                if (respectPrevState) prevVisibilityMap[ui] = ui.Visible;
                if (immediate) ui.HideImmediate();
                else ui.Hide();
            }
        }
    }
}
