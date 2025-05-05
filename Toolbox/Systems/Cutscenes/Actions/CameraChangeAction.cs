using System.Collections;

using CutsceneSystem;

using Unity.Cinemachine;

using UnityEngine;

namespace GameCutscene
{
    public class CameraChangeAction : CutsceneAction
    {
        [SerializeField] string vcamId;
        [SerializeField] float transitionDuration = .5f;
        [SerializeField] float holdDuration = 2;
        [SerializeField] float revertDuration = .5f;

        // props
        private CinemachineCamera Vcam => VcamIdManager.Instance.GetById(vcamId);
        private CameraManager Manager => CameraManager.Instance;

        // fields
        private CinemachineCamera prevCamera;

        public override IEnumerator Play()
        {
            yield return base.Play();
            prevCamera = Manager.GetCurrentCamera();
            Manager.SetCamera(Vcam, transitionDuration);
            yield return new WaitForSeconds(transitionDuration + holdDuration);
        }

        public override void Revert()
        {
            base.Revert();
            Manager.SetCamera(prevCamera, revertDuration);
        }
    }
}
