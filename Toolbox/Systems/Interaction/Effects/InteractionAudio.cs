using UnityEngine;

namespace InteractionSystem
{
    public class InteractionAudio : GeneralAudio
    {
        [SerializeField] AudioClip interactSound;

        private void Start() => InteractionManager.Instance.OnInteractionStarted += () => Play(interactSound);
    }
}
