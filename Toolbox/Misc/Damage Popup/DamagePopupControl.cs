using UnityEngine;

using Random = UnityEngine.Random;

public class DamagePopupControl : MonoBehaviour
{
    [SerializeField] RStats stats;
    [SerializeField] ScriptableReference health;
    [SerializeField] TextPopup prefab;
    [SerializeField] float radius = 1;

    RStat Health => stats[health];

    private void OnEnable() => Health.OnChanged += OnHealthChanged;
    private void OnDisable() => Health.OnChanged -= OnHealthChanged;

    private void OnHealthChanged(float delta)
    {
        TextPopupPools.Popup(prefab, transform.position + Random.insideUnitSphere * radius, Mathf.Abs(delta).ToString());
    }
}