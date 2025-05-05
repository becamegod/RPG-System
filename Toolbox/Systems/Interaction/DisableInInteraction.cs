namespace InteractionSystem
{
    public class DisableInInteraction : DisableInEvent
    {
        private InteractionManager Manager => InteractionManager.Instance;

        protected override void ListenEvent()
        {
            Manager.OnInteractionStarted += TurnOff;
            Manager.OnInteractionEnded += TurnOn;
        }

        protected override void IgnoreEvent()
        {
            Manager.OnInteractionStarted -= TurnOff;
            Manager.OnInteractionEnded -= TurnOn;
        }
    }
}
