using Unity.Cinemachine;
using UnityEngine;

namespace CutsceneSystem
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class RegisterVcamId : RegisterBehaviorId<CinemachineCamera>
    {
        protected override BehaviorIdManager<CinemachineCamera> GetManager() => VcamIdManager.Instance;
    }
}
