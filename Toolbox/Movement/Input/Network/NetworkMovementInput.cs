using System;

using Unity.Netcode;

using UnityEngine;

namespace MovementSystem
{
    public class NetworkMovementInput : NetworkBehaviour
    {
        [SerializeField] MovementInput input;

        private void Reset() => input = GetComponent<MovementInput>();

        public override void OnNetworkSpawn()
        {
            input.enabled = IsOwner;
            if (input.enabled && !IsServer) input.Movement.OnMoved += OnMoved;
        }

        private void OnMoved(Vector3 motion)
        {
            MoveServerRpc(motion);
        }

        [ServerRpc]
        private void MoveServerRpc(Vector3 motion)
        {
            input.Movement.Move(motion);
        }
    }
}
