using UnityEngine;

namespace InteractionSystem
{
    [RequireComponent(typeof(Interaction))]
    public class OverrideInteractionCamera : MonoBehaviour
    {
        [SerializeField] bool noZoom;
        [SerializeField, ShowWhen("!noZoom")] Vector3 distanceOffset;
        [SerializeField, ShowWhen("!noZoom")] bool overrideTrackPointOffset;
        [SerializeField, ShowWhen("!noZoom && overrideTrackPointOffset")] Vector3 trackPointOffset;

        public bool NoZoom => noZoom;
        public Vector3 DistanceOffset => distanceOffset;
        public bool OverrideTrackPointOffset => overrideTrackPointOffset;
        public Vector3 TrackPointOffset => trackPointOffset;
    }
}
