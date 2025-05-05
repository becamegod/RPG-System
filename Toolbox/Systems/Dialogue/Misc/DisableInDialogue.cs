namespace DialogueSystem
{
    public class DisableInDialogue : DisableInEvent
    {
        protected override void IgnoreEvent()
        {
            DialogueController.Instance.OnStarted += TurnOff;
            DialogueController.Instance.OnEnded += TurnOn;
        }

        protected override void ListenEvent()
        {
            DialogueController.Instance.OnStarted -= TurnOff;
            DialogueController.Instance.OnEnded -= TurnOn;
        }
    }
}
