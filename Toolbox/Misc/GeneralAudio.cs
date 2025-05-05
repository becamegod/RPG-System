using UnityEngine;

public class GeneralAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void Play(AudioClip clip) => audioSource.PlayOneShot(clip);

    private void Reset() => audioSource = GetComponent<AudioSource>();
}
