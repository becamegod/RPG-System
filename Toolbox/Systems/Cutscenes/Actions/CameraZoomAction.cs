using System.Collections;

using CutsceneSystem;

using Unity.Cinemachine;

using UnityEngine;

namespace GameCutscene
{
    public class CameraZoomAction : CutsceneAction
    {
        [SerializeField] string vcamId;
        [SerializeField] float fovTarget;
        [SerializeField] float duration = .5f;
        [SerializeField] float revertDuration = .5f;

        private CinemachineCamera Vcam => VcamIdManager.Instance.GetById(vcamId);

        private float originalFov = -1;

        public override IEnumerator Play()
        {
            yield return base.Play();
            if (originalFov == -1) originalFov = Vcam.Lens.FieldOfView;
            Vcam.DOFOV(fovTarget, duration);
            yield return null;
        }

        public override void Revert()
        {
            base.Revert();
            Vcam.DOFOV(originalFov, revertDuration);
        }
    }
}
