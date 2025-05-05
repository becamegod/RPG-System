using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    public UnityEvent onCollisionEnter, onCollisionExit, onCollisionStay;
    public UnityEvent onTriggerEnter, onTriggerExit, onTriggerStay;

    private void OnCollisionEnter(Collision collision) => onCollisionEnter?.Invoke();
    private void OnCollisionExit(Collision collision) => onCollisionExit?.Invoke();
    private void OnCollisionStay(Collision collision) => onCollisionStay?.Invoke();
    private void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke();
    private void OnTriggerExit(Collider other) => onTriggerExit?.Invoke();
    private void OnTriggerStay(Collider other) => onTriggerStay?.Invoke();
}