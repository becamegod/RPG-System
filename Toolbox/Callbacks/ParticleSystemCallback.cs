using System;
using UnityEngine;

public class ParticleSystemCallback : MonoBehaviour
{
    public Action onStop;
    private Action onTrigger;
    private Action<GameObject> onCollide;

    private void OnParticleSystemStopped() => onStop?.Invoke();
    private void OnParticleCollision(GameObject other) => onCollide?.Invoke(other);
    private void OnParticleTrigger() => onTrigger?.Invoke();
}
