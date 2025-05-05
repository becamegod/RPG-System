using System;
using UnityEngine;

public class FilteredCollisionCallback : MonoBehaviour
{
    [SerializeField] TargetFilter targetFilter;
    public event Action<BaseCollider> onCollisionEnter, onCollisionExit, onCollisionStay;
    public event Action<BaseCollider> onTriggerEnter, onTriggerExit, onTriggerStay;
    public event Action<BaseCollider> OnEntered, OnExited, OnStaying;

    private void Awake()
    {
        onCollisionEnter += subject => OnEntered?.Invoke(subject);
        onCollisionExit += subject => OnExited?.Invoke(subject);
        onCollisionStay += subject => OnStaying?.Invoke(subject);
        onTriggerEnter += subject => OnEntered?.Invoke(subject);
        onTriggerExit += subject => OnExited?.Invoke(subject);
        onTriggerStay += subject => OnStaying?.Invoke(subject);
    }

    private void OnCollisionEnter(Collision collision) => Process(new(collision), onCollisionEnter);
    private void OnCollisionExit(Collision collision) => Process(new(collision), onCollisionExit);
    private void OnCollisionStay(Collision collision) => Process(new(collision), onCollisionStay);

    private void OnTriggerEnter(Collider other) => Process(new(other), onTriggerEnter);
    private void OnTriggerExit(Collider other) => Process(new(other), onTriggerExit);
    private void OnTriggerStay(Collider other) => Process(new(other), onTriggerStay);

    private void OnCollisionEnter2D(Collision2D collision) => Process(new(collision), onCollisionEnter);
    private void OnCollisionExit2D(Collision2D collision) => Process(new(collision), onCollisionExit);
    private void OnCollisionStay2D(Collision2D collision) => Process(new(collision), onCollisionStay);

    private void OnTriggerEnter2D(Collider2D other) => Process(new(other), onTriggerEnter);
    private void OnTriggerExit2D(Collider2D other) => Process(new(other), onTriggerExit);
    private void OnTriggerStay2D(Collider2D other) => Process(new(other), onTriggerStay);

    private void Process(BaseCollider subject, Action<BaseCollider> callback)
    {
        if (targetFilter && !targetFilter.Check(subject)) return;
        callback?.Invoke(subject);
    }
}