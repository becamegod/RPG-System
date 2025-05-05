using UISystem;

namespace DialogueSystem
{
    public class DisableInUIFlow : DisableInEvent
    {
        private UIController UI => UIController.Instance;

        protected override void ListenEvent()
        {
            UI.OnUIFlowEntered += TurnOff;
            UI.OnUIFlowExited += TurnOn;
        }

        protected override void IgnoreEvent()
        {
            UI.OnUIFlowEntered -= TurnOff;
            UI.OnUIFlowExited -= TurnOn;
        }
    }
}
