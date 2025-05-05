using Unity.Cinemachine;

using UnityEngine;

namespace InteractionSystem
{
    public class InteractionCamera : MonoBehaviour
    {
        [SerializeField] CinemachineCamera vcam;
        [SerializeField] CinemachineTargetGroup targetGroup;
        [SerializeField] Transform player;

        // props
        private InteractionManager Manager => InteractionManager.Instance;

        private void Start()
        {
            Manager.OnInteractionStarted += OnInteractionStarted;
            Manager.OnInteractionEnded += OnInteractionEnded;
        }

        private void OnInteractionEnded() => vcam.enabled = false;

        private void OnInteractionStarted()
        {
            var currentInteraction = Manager.CurrentInteraction;
            if (!currentInteraction) return;
            targetGroup.Targets[0].Object = player;
            targetGroup.Targets[1].Object = currentInteraction.transform;
            vcam.enabled = true;
            if (currentInteraction.TryGetComponent<OverrideInteractionCamera>(out var overrider))
            {
                if (overrider.NoZoom)
                {
                    vcam.enabled = false;
                    return;
                }
                vcam.GetComponent<CinemachineFollow>().FollowOffset = overrider.DistanceOffset;
                if (overrider.OverrideTrackPointOffset)
                    vcam.GetComponent<CinemachineGroupFraming>().CenterOffset = overrider.TrackPointOffset;
            }
        }
    }
}
