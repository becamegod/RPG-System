using System;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    public Action<Collision> onCollisionEnter, onCollisionExit, onCollisionStay;
    public Action<Collider> onTriggerEnter, onTriggerExit, onTriggerStay;

    private void OnCollisionEnter(Collision collision) => onCollisionEnter?.Invoke(collision);
    private void OnCollisionExit(Collision collision) => onCollisionExit?.Invoke(collision);
    private void OnCollisionStay(Collision collision) => onCollisionStay?.Invoke(collision);
    private void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke(other);
    private void OnTriggerExit(Collider other) => onTriggerExit?.Invoke(other);
    private void OnTriggerStay(Collider other) => onTriggerStay?.Invoke(other);
}