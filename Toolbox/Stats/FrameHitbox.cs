using UnityEngine;

public class FrameHitbox : MonoBehaviour
{
    [SerializeField] float damage = 1;
    [SerializeField] int liveFrames = 1;
    [SerializeField] LayerMask mask;
    private int frame;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        frame = liveFrames;
    }

    private void Update()
    {
        if (frame > 0) frame--;
        else gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!mask.Contains(collision.gameObject.layer)) return;

        var health = collision.GetComponentInChildren<Health>();
        if (!health) return;

        health.Value -= damage;
        gameObject.SetActive(false);
    }
}
