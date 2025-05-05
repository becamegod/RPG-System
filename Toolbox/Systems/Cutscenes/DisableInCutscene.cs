using UnityEngine;

namespace CutsceneSystem
{
    public class DisableInCutscene : DisableInEvent
    {
        private CutsceneController Controller => CutsceneController.Instance;

        protected override void IgnoreEvent()
        {
            Controller.OnStarted -= TurnOff;
            Controller.OnEnded -= TurnOn;
        }

        protected override void ListenEvent()
        {
            Controller.OnStarted += TurnOff;
            Controller.OnEnded += TurnOn;
        }
    }
}
