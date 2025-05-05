using InteractionSystem;
using System;
using UnityEngine;

namespace DialogueSystem
{
    public class RegisterProfile : MonoBehaviour
    {
        [SerializeField] Profile profile;

        // props
        private DialogueController Controller => DialogueController.Instance;

        // fields
        private bool started;

        private void Start()
        {
            started = true;
            OnEnable();
        }

        private void OnEnable()
        {
            if (!started) return;
            Controller.RegisterTalker(profile, transform);
        }

        private void OnDisable() => Controller.DeregisterTalker(profile);
    }
}
